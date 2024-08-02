using MAST_Service.Models;
using MAST_Service.ServiceModels;
using Microsoft.Data.SqlClient;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAST_Service
{
    public class ReleaseDateReminderService : IReleaseDateReminderService
    {
        private readonly string? connectionString;
        private readonly ILogger<ReleaseDateReminderService> logger;
        private IConfiguration Configuration;
        private readonly MastContext _context = null;
        public ReleaseDateReminderService(MastContext context, IConfiguration configuration, ILogger<ReleaseDateReminderService> logger)
        {
            this.connectionString = configuration.GetConnectionString("dbConnection");
            this.logger = logger;
            Configuration = configuration;
            _context = context;
        }

        public async void checkEmployeeReleaseDate()
        {
            try
            {
                logger.LogInformation("Worker function start", DateTimeOffset.Now);

                var myConnectionString = this.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(myConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetEmployeeReleaseDateDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string IsExited = Convert.ToString(reader["IsExited"]);

                            DateTime releaseDate = Convert.ToDateTime(reader["DateOfRelease"]);
                            DateTime now = DateTime.Now;
                            long EmployeeUserID = Convert.ToInt64(reader["UserID"]);
                            string EmployeeCode = Convert.ToString(reader["EmployeeCode"]);
                            string EmployeeName = Convert.ToString(reader["UserFullName"]);
                            int EmployeeTypeID = Convert.ToInt32(reader["EmployeeTypeID"]);
                            DateTime DateOfJoining = Convert.ToDateTime(reader["DateOfJoining"]);

                            if (string.IsNullOrWhiteSpace(IsExited) || IsExited.Trim() == "0")
                            {
                                

                                bool hrActionTaken;

                                using (SqlConnection connectionCheck = new SqlConnection(myConnectionString))
                                {
                                    SqlCommand cmdCheck = new SqlCommand(
                                        "SELECT COUNT(*) FROM UserJobValidityExtendMaster WHERE UserID = @UserID AND IsHrExtendUserValidity != '0'",
                                        connectionCheck);
                                    cmdCheck.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;
                                    connectionCheck.Open();

                                    int count = (int)cmdCheck.ExecuteScalar();
                                    hrActionTaken = count > 0;
                                }

                                if (hrActionTaken == false)
                                {
                                    if (releaseDate <= DateTime.Now.Date)
                                    {
                                        using (SqlConnection connectionUpsert = new SqlConnection(myConnectionString))
                                        {
                                            connectionUpsert.Open();
                                            SqlCommand cmdUpdate = new SqlCommand(
                                                       "UPDATE UserMaster SET IsArchive = 1, IsExited = 1 WHERE UserID = @UserID AND IsArchive = 0",
                                                       connectionUpsert);
                                            cmdUpdate.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;

                                            cmdUpdate.ExecuteNonQuery();
                                            SqlCommand cmdCheckEntry = new SqlCommand(
                                                                               "SELECT TOP 1 UserJobValidityExtendID  FROM UserJobValidityExtendMaster WHERE UserID = @UserID AND IsArchive = 0 AND IsHrExtendUserValidity = 0",
                                                                                          connectionUpsert);
                                            cmdCheckEntry.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;

                                            long? entryCount = (long?)cmdCheckEntry.ExecuteScalar();

                                            if (entryCount > 0)
                                            {
                                                SqlCommand cmdUpdate1 = new SqlCommand(
                                                    "UPDATE UserJobValidityExtendMaster SET IsHrExtendUserValidity = 2 WHERE UserJobValidityExtendID = @UserJobValidityExtendID AND IsArchive = 0",
                                                    connectionUpsert);
                                                cmdUpdate1.Parameters.Add("@UserJobValidityExtendID", SqlDbType.BigInt).Value = entryCount;
                                                cmdUpdate.ExecuteNonQuery();
                                            }

                                            connectionUpsert.Close();

                                        }
                                    }

                                }


                                for (int daysBefore = 5; daysBefore >= 1; daysBefore--)
                                {
                                    DateTime notificationDate = releaseDate.AddDays(-daysBefore);

                                    if (now.Date == notificationDate.Date && now.Hour == 12)
                                    {
                                        if (hrActionTaken)
                                        {
                                            continue;
                                        }

                                        long userJobValidityExtendID = 0;

                                        using (SqlConnection connectionUpsert = new SqlConnection(myConnectionString))
                                        {
                                            long entryCount = 0;
                                            DateTime? Notifydate = null;
                                            connectionUpsert.Open();

                                            SqlCommand cmdCheckEntry = new SqlCommand(
    "SELECT TOP 1 UserJobValidityExtendID, NotifyDate FROM UserJobValidityExtendMaster WHERE UserID = @UserID AND IsArchive = 0",
    connectionUpsert);
                                            cmdCheckEntry.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;

                                            using (SqlDataReader reader1 = cmdCheckEntry.ExecuteReader())
                                            {
                                                if (reader1.Read())
                                                {
                                                     entryCount = reader1.GetInt64(0); // Assuming UserJobValidityExtendID is of type bigint
                                                     Notifydate = reader1.IsDBNull(1) ? (DateTime?)null : reader1.GetDateTime(1); // Handling nullable DateTime
                                                }
                                            }

                                            if (entryCount > 0)
                                            { 
                                                if (Notifydate != null && Convert.ToDateTime(Notifydate).Date == DateTime.Now.Date)
                                                {
                                                    continue;
                                                }

                                                SqlCommand cmdUpdate = new SqlCommand(
                                                    "UPDATE UserJobValidityExtendMaster SET NotifyDate = @NotifyDate, LastModifiedOn = @LastModifiedOn WHERE UserJobValidityExtendID = @UserJobValidityExtendID AND IsArchive = 0",
                                                    connectionUpsert);
                                                cmdUpdate.Parameters.Add("@UserJobValidityExtendID", SqlDbType.BigInt).Value = entryCount;
                                                cmdUpdate.Parameters.Add("@NotifyDate", SqlDbType.DateTime).Value = notificationDate;
                                                cmdUpdate.Parameters.Add("@LastModifiedOn", SqlDbType.DateTime).Value = now;

                                                cmdUpdate.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                SqlCommand cmdInsert = new SqlCommand("dbo.sp_InsertUserJobValidityExtend", connectionUpsert);
                                                cmdInsert.CommandType = CommandType.StoredProcedure;
                                                cmdInsert.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;
                                                cmdInsert.Parameters.Add("@NotifyDate", SqlDbType.DateTime).Value = notificationDate;
                                                cmdInsert.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = now;
                                                cmdInsert.Parameters.Add("@LastModifiedOn", SqlDbType.DateTime).Value = now;
                                                cmdInsert.Parameters.Add("@IsArchive", SqlDbType.Bit).Value = false;

                                                var result = cmdInsert.ExecuteScalar();
                                                userJobValidityExtendID = Convert.ToInt64(result);
                                            }
                                        }

                                        // Send notification
                                        using (SqlConnection connectionHR = new SqlConnection(myConnectionString))
                                        {
                                            SqlCommand cmdHR = new SqlCommand("dbo.sp_GetHRDetails", connectionHR);
                                            cmdHR.CommandType = CommandType.StoredProcedure;
                                            connectionHR.Open();

                                            using (var readerHR = cmdHR.ExecuteReader())
                                            {
                                                while (readerHR.Read())
                                                {
                                                    long HRUserID = Convert.ToInt64(readerHR["UserID"]);
                                                    string HRName = Convert.ToString(readerHR["FullName"]);
                                                    string HREmailAddress = Convert.ToString(readerHR["EmailAddress"]);

                                                    clsEncryptDecrypt objencry = new clsEncryptDecrypt();
                                                    string? Enc_ActivityID = objencry.Encrypt(Convert.ToString(userJobValidityExtendID));

                                                    long? NotificationID = null;
                                                    using (SqlConnection connection5 = new SqlConnection(myConnectionString))
                                                    {
                                                        SqlCommand command = new SqlCommand("sp_CreateNotification", connection5);
                                                        command.CommandType = CommandType.StoredProcedure;
                                                        command.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;
                                                        command.Parameters.Add("@NotificationTypeID", SqlDbType.Int).Value = 30;
                                                        command.Parameters.Add("@NotificationText", SqlDbType.NVarChar).Value = "Dear " + HRName + ", The release date for " + EmployeeCode + " (" + EmployeeName + ") is coming up on " + releaseDate.ToShortDateString() + ".";
                                                        command.Parameters.Add("@NotificationTitle", SqlDbType.NVarChar).Value = "Release Date Notification - " + EmployeeName;
                                                        command.Parameters.Add("@ActivityID", SqlDbType.BigInt).Value = userJobValidityExtendID;
                                                        command.Parameters.Add("@Enc_ActivityID", SqlDbType.NVarChar).Value = Enc_ActivityID;

                                                        SqlParameter outputParam = new SqlParameter("@NotificationID", SqlDbType.BigInt)
                                                        {
                                                            Direction = ParameterDirection.Output
                                                        };
                                                        command.Parameters.Add(outputParam);

                                                        connection5.Open();
                                                        command.ExecuteNonQuery();
                                                        NotificationID = (long?)outputParam.Value;
                                                    }

                                                    if (NotificationID.HasValue)
                                                    {
                                                        using (SqlConnection connection6 = new SqlConnection(myConnectionString))
                                                        {
                                                            SqlCommand command = new SqlCommand("sp_SendUserNotification", connection6);
                                                            command.CommandType = CommandType.StoredProcedure;
                                                            command.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;
                                                            command.Parameters.Add("@NotificationID", SqlDbType.BigInt).Value = NotificationID;
                                                            command.Parameters.Add("@ReceivalID", SqlDbType.BigInt).Value = HRUserID;

                                                            SqlParameter outputParam = new SqlParameter("@UserNotificationJunctionID", SqlDbType.BigInt)
                                                            {
                                                                Direction = ParameterDirection.Output
                                                            };
                                                            command.Parameters.Add(outputParam);

                                                            connection6.Open();
                                                            command.ExecuteNonQuery();
                                                            var UserNotificationJunctionID = (long?)outputParam.Value;

                                                            logger.LogInformation("Notification sent to HR: " + HRName + " with UserNotificationJunctionID: " + UserNotificationJunctionID);
                                                        }
                                                    }

                                                    bool? _SendEmailToHR = await SendEmailToHRForReleaseDate(EmployeeName, EmployeeCode, HRName, HREmailAddress, releaseDate);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            else
                            {
                                if (releaseDate <= DateTime.Now.Date)
                                {
                                    using (SqlConnection connectionUpsert = new SqlConnection(myConnectionString))
                                    {
                                        connectionUpsert.Open();
                                        SqlCommand cmdUpdate = new SqlCommand(
                                                   "UPDATE UserMaster SET IsArchive = 1 WHERE UserID = @UserID AND IsArchive = 0",
                                                   connectionUpsert);
                                    cmdUpdate.Parameters.Add("@UserID", SqlDbType.BigInt).Value = EmployeeUserID;

                                    cmdUpdate.ExecuteNonQuery();
                                        connectionUpsert.Close();
                                        
                                    }
                                }
                            }
                        }
                    }
                }

                logger.LogInformation("Worker function end", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
        public async Task<bool?> SendEmailToHRForReleaseDate(string EmployeeName, string EmployeeCode, string HRName, string HREmailAddress, DateTime releaseDate)
        {
            bool? sendEmailNotification = false;
            try
            {
                var appSettingsSectionWebsite = Configuration.GetSection("Website");
                var appSettingsSectionEmail = Configuration.GetSection("EmailConfig");
                var websiteUrl = appSettingsSectionWebsite.GetValue<string>("WebsiteUrl");
                var apiKey = appSettingsSectionEmail.GetValue<string>("APIKey");
                var templateId = appSettingsSectionEmail.GetValue<string>("MAST_NotificationToHRforReleaseDate");
                var sendEmailFrom = appSettingsSectionEmail.GetValue<string>("Email_From");
                var sendEmailFromName = appSettingsSectionEmail.GetValue<string>("SendEmailFromName");
                var bcc1 = appSettingsSectionEmail.GetValue<string>("Email_Bcc1");
                var bcc2 = appSettingsSectionEmail.GetValue<string>("Email_Bcc2");
                var cc = appSettingsSectionEmail.GetValue<string>("Email_cc");

                SendGridClient client = new SendGridClient(apiKey);
                SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();
                message.From = new EmailAddress(sendEmailFrom, sendEmailFromName);
                message.AddTo(HREmailAddress);
                message.AddBcc(bcc1, sendEmailFromName);
                message.AddBcc(bcc2, sendEmailFromName);
                if (!string.IsNullOrEmpty(cc))
                {
                    message.AddCc(cc);
                }

                message.Subject = "Release Date Notification - " + EmployeeCode + " - " + EmployeeName;

                message.SetTemplateId(templateId);
                message.AddSubstitution("@Model.EmployeeName", EmployeeName);
                message.AddSubstitution("@Model.EmployeeCode", EmployeeCode);
                message.AddSubstitution("@Model.HRName", HRName);
                message.AddSubstitution("@Model.releaseDate", releaseDate.ToString("dd-MM-yyyy"));
                await client.SendEmailAsync(message);
                sendEmailNotification = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return sendEmailNotification;
        }


    }
}
