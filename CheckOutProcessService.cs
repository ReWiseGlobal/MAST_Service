using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Azure.Core;
using System.Net;
using MAST_Service.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Configuration;

namespace MAST_Service
{
    public class CheckOutProcessService : ICheckOutProcessService
    {
        private readonly string? connectionString;
        private readonly ILogger<CheckOutProcessService> logger;
        private IConfiguration Configuration;
        private readonly MastContext _context = null;
        public CheckOutProcessService(MastContext context, IConfiguration configuration, ILogger<CheckOutProcessService> logger)
        {
            this.connectionString = configuration.GetConnectionString("dbConnection");
            this.logger = logger;
            Configuration = configuration;
            _context = context;
        }

        public async void checkExecutorProcess()
        {

            try
            {
                logger.LogInformation("Worker function start", DateTimeOffset.Now);

                var myConnectionString = this.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(myConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetListOfCheckInUsers", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool? ishalfDayLEave = false;
                            CheckoutProcessModel obj = new CheckoutProcessModel();
                            long UserDailyShiftJunctionId = Convert.ToInt64(reader["UserDailyShiftJunctionId"]);
                            long UserId = Convert.ToInt64(reader["UserId"]);
                            DateTime ShiftEndDateTime = Convert.ToDateTime(reader["ShiftEndDateTime"]);
                            DateTime ShiftStartDateTime = Convert.ToDateTime(reader["ShiftStartDateTime"]);
                            DateTime ActionDateTime = Convert.ToDateTime(reader["ActionDateTime"]);
                            using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                            {
                                SqlCommand cmd1 = new SqlCommand("sp_CheckIsHalfDayLeave", connection1);
                                cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd1.Parameters.Add("@Date", SqlDbType.DateTime).Value = ShiftStartDateTime.Date;
                                cmd1.Parameters.Add("@userid", SqlDbType.BigInt).Value = UserId;
                                connection1.Open();
                                using (var reader1 = cmd1.ExecuteReader())
                                {

                                    if (reader1.HasRows)
                                    {
                                        ishalfDayLEave = true;
                                    }
                                }
                                connection1.Close();
                            }
                            DateTime MindShiftTime = new DateTime();

                            using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                            {
                                SqlCommand cmd1 = new SqlCommand("sp_CheckIsCheckoutDone", connection1);
                                cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd1.Parameters.Add("@UserDailyShiftJunctionId", SqlDbType.BigInt).Value = UserDailyShiftJunctionId;
                                connection1.Open();

                                using (var reader1 = cmd1.ExecuteReader())
                                {

                                    if (!reader1.HasRows)
                                    {
                                        var endtime = Convert.ToDateTime(ShiftEndDateTime).AddHours(3);
                                        if (DateTime.Now > endtime)
                                        {
                                            if (ishalfDayLEave == true)
                                            {
                                                int halfdayHours = 0;
                                                TimeSpan diff = Convert.ToDateTime(ShiftEndDateTime).Subtract(Convert.ToDateTime(ShiftStartDateTime));
                                                halfdayHours = Convert.ToInt32(diff.TotalMinutes) / 2;
                                                MindShiftTime = ShiftStartDateTime.AddMinutes(Convert.ToInt32(halfdayHours));
                                                if (ActionDateTime >= MindShiftTime)
                                                {

                                                }
                                                else
                                                {
                                                    ShiftEndDateTime = MindShiftTime;
                                                }
                                                logger.LogInformation("half day leave", DateTimeOffset.Now);
                                            }
                                            using (SqlConnection connection2 = new SqlConnection(myConnectionString))
                                            {
                                                connection2.Open();
                                                SqlCommand cmdinsert = new SqlCommand("sp_insertCheckOutOfUser", connection2);
                                                cmdinsert.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmdinsert.Parameters.Add("@UserDailyShiftJunctionId", SqlDbType.BigInt).Value = UserDailyShiftJunctionId;
                                                cmdinsert.Parameters.Add("@UserId", SqlDbType.BigInt).Value = UserId;
                                                cmdinsert.Parameters.Add("@AttendanceActionStatusId", SqlDbType.Int).Value = 4;
                                                cmdinsert.Parameters.Add("@ActionDateTime", SqlDbType.DateTime).Value = ShiftEndDateTime;
                                                cmdinsert.Parameters.Add("@isInsert", SqlDbType.Int).Value = 1;
                                                cmdinsert.ExecuteNonQuery();
                                                connection2.Close();

                                            }

                                            bool? IsUpdateWorkTime = await UpdateWorkTime(UserDailyShiftJunctionId);
                                        }

                                    }
                                    else
                                    {
                                        //if (ishalfDayLEave == true)
                                        //{
                                        //    var endtime = Convert.ToDateTime(ShiftEndDateTime).AddHours(5);
                                        //    if (DateTime.Now > endtime)
                                        //    {
                                        //        DateTime _UserDailyShiftAttendance_CheckInDateTime = ActionDateTime;
                                        //        DateTime _UserDailyShiftAttendance_CheckOutDateTime = new DateTime();
                                        //        while (reader1.Read())
                                        //        {

                                        //            _UserDailyShiftAttendance_CheckOutDateTime = Convert.ToDateTime(reader1["ActionDateTime"]);
                                        //        }

                                        //        using (SqlConnection connection2 = new SqlConnection(myConnectionString))
                                        //        {
                                        //            connection2.Open();
                                        //            SqlCommand cmdinsert = new SqlCommand("sp_insertCheckOutOfUser", connection2);
                                        //            cmdinsert.CommandType = System.Data.CommandType.StoredProcedure;
                                        //            cmdinsert.Parameters.Add("@UserDailyShiftJunctionId", SqlDbType.BigInt).Value = UserDailyShiftJunctionId;
                                        //            cmdinsert.Parameters.Add("@UserId", SqlDbType.BigInt).Value = UserId;
                                        //            cmdinsert.Parameters.Add("@AttendanceActionStatusId", SqlDbType.Int).Value = 4;
                                        //            cmdinsert.Parameters.Add("@ActionDateTime", SqlDbType.DateTime).Value = ShiftEndDateTime;
                                        //            cmdinsert.Parameters.Add("@isInsert", SqlDbType.Int).Value = 0;
                                        //            cmdinsert.ExecuteNonQuery();
                                        //            connection2.Close();

                                        //        }
                                        //    }
                                        //}


                                    }
                                }

                                connection1.Close();
                            }


                        }
                    }

                    connection.Close();
                }


                //For OT
                using (SqlConnection connection = new SqlConnection(myConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetListOfOTCheckInUsers", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CheckoutProcessModel obj = new CheckoutProcessModel();
                            long UserDailyShiftJunctionId = Convert.ToInt64(reader["UserDailyShiftJunctionId"]);
                            long UserId = Convert.ToInt64(reader["UserId"]);
                            long UserDailyShiftAttendanceID = Convert.ToInt64(reader["UserDailyShiftAttendanceID"]);
                            DateTime? ActionDateTime = Convert.ToDateTime(reader["ActionDateTime"]);


                            int recordCount = 0;

                            using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                            {
                                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) AS RecordCount FROM UserDailyShiftAttendanceJunction WHERE UserDailyShiftJunctionID=" + UserDailyShiftJunctionId + " and AttendanceActionStatusID=6 and IsArchive!='1';", connection1))
                                {
                                    connection1.Open();
                                    recordCount = (int)command.ExecuteScalar();
                                    connection1.Close();
                                }
                            }
                            if (recordCount == 0)
                            {
                                int? OTHours = 0;
                                long? OTExtendRequestID = null;

                                //using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                                //{
                                //    using (SqlCommand command = new SqlCommand("SELECT TOP 1 UserOtextendInMinutes FROM OtextendRequestJunction WHERE UserDailyShiftAttendanceID=" + UserDailyShiftAttendanceID + " and IsArchive!='1';", connection1))
                                //    {
                                //        connection1.Open();
                                //        OTHours = (int)command.ExecuteScalar();
                                //        connection1.Close();
                                //    }
                                //}

                                using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                                {
                                    string query = "SELECT TOP 1 OtextendRequestId, UserOtextendInMinutes " + "FROM OtextendRequestJunction " + "WHERE UserDailyShiftAttendanceID=@UserDailyShiftAttendanceID AND IsArchive != '1';";

                                    using (SqlCommand command = new SqlCommand(query, connection1))
                                    {
                                        command.Parameters.AddWithValue("@UserDailyShiftAttendanceID", UserDailyShiftAttendanceID);
                                        connection1.Open();

                                        using (SqlDataReader sqlReader = command.ExecuteReader())
                                        {
                                            if (sqlReader.Read())
                                            {
                                                OTExtendRequestID = sqlReader["OtextendRequestId"] as long?;
                                                OTHours = sqlReader["UserOtextendInMinutes"] as int?;
                                            }
                                        }

                                        connection1.Close();
                                    }
                                }

                                if (OTHours != null && ActionDateTime != null)
                                {
                                    var endDate = Convert.ToDateTime(ActionDateTime).AddMinutes(Convert.ToDouble(OTHours));
                                    if (DateTime.Now > endDate)
                                    {
                                        using (SqlConnection connection2 = new SqlConnection(myConnectionString))
                                        {
                                            connection2.Open();
                                            SqlCommand cmdinsert = new SqlCommand("sp_insertCheckOutOfUser", connection2);
                                            cmdinsert.CommandType = System.Data.CommandType.StoredProcedure;
                                            cmdinsert.Parameters.Add("@UserDailyShiftJunctionId", SqlDbType.BigInt).Value = UserDailyShiftJunctionId;
                                            cmdinsert.Parameters.Add("@UserId", SqlDbType.BigInt).Value = UserId;
                                            cmdinsert.Parameters.Add("@AttendanceActionStatusId", SqlDbType.Int).Value = 6;
                                            cmdinsert.Parameters.Add("@ActionDateTime", SqlDbType.DateTime).Value = endDate;
                                            cmdinsert.ExecuteNonQuery();
                                            connection2.Close();

                                        }

                                        bool? IsUpdateWorkTime = await UpdateOTWorkTime(UserDailyShiftJunctionId);

                                        if (OTExtendRequestID > 0)
                                        {

                                            long? SupervisorID = 1;
                                            using (SqlConnection connection3 = new SqlConnection(myConnectionString))
                                            {
                                                using (SqlCommand command = new SqlCommand("sp_GetSupervisorID", connection3))
                                                {
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.Parameters.Add("@UserId", SqlDbType.BigInt).Value = UserId;
                                                    connection3.Open();
                                                    long? supervisorID = (long?)command.ExecuteScalar();
                                                    SupervisorID = supervisorID;
                                                    connection3.Close();
                                                }
                                            }

                                            var SenderName = "";
                                            string? SenderEmail = "";
                                            long? SenderUserID = 0;
                                            string? ReceiverName = "";
                                            string? ReceiverEmail = "";
                                            int? NotificationTypeID = 16;
                                            string? NotificationText = "";
                                            string? NotificationTitle = "Request for OT";

                                            using (SqlConnection connection4 = new SqlConnection(myConnectionString))
                                            {
                                                using (SqlCommand command = new SqlCommand("sp_GetUserDetails", connection4))
                                                {
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.Parameters.Add("@UserId", SqlDbType.BigInt).Value = UserId;
                                                    connection4.Open();
                                                    using (SqlDataReader senderReader = command.ExecuteReader())
                                                    {
                                                        if (senderReader.Read())
                                                        {
                                                            SenderUserID = senderReader["UserID"] as long?;
                                                            SenderName = senderReader["UserName"].ToString();
                                                            SenderEmail = senderReader["EmailAddress"].ToString();
                                                        }
                                                    }
                                                    connection4.Close();
                                                }

                                                using (SqlCommand command = new SqlCommand("sp_GetReceiverDetails", connection4))
                                                {
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.Parameters.Add("@UserId", SqlDbType.BigInt).Value = SupervisorID;
                                                    connection4.Open();
                                                    using (SqlDataReader receiverReader = command.ExecuteReader())
                                                    {
                                                        if (receiverReader.Read())
                                                        {
                                                            ReceiverName = receiverReader["UserName"].ToString();
                                                            ReceiverEmail = receiverReader["EmailAddress"].ToString();
                                                        }
                                                    }
                                                    connection4.Close();
                                                }
                                            }

                                            long? NotificationID = 0;
                                            using (SqlConnection connection5 = new SqlConnection(myConnectionString))
                                            {
                                                using (SqlCommand command = new SqlCommand("sp_CreateNotification", connection5))
                                                {
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.Parameters.Add("@UserID", SqlDbType.BigInt).Value = UserId;
                                                    command.Parameters.Add("@NotificationTypeID", SqlDbType.Int).Value = NotificationTypeID;
                                                    command.Parameters.Add("@NotificationText", SqlDbType.NVarChar).Value = "User " + SenderName + ", has sent you new request for OT. Please review and take appropriate action.";
                                                    command.Parameters.Add("@NotificationTitle", SqlDbType.NVarChar).Value = NotificationTitle;
                                                    command.Parameters.Add("@ActivityID", SqlDbType.BigInt).Value = OTExtendRequestID;
                                                    SqlParameter outputParam = new SqlParameter("@NotificationID", SqlDbType.BigInt)
                                                    {
                                                        Direction = ParameterDirection.Output
                                                    };
                                                    command.Parameters.Add(outputParam);
                                                    connection5.Open();
                                                    command.ExecuteNonQuery();
                                                    NotificationID = (long?)outputParam.Value;
                                                    connection5.Close();
                                                }
                                            }

                                            long? UserNotificationJunctionID = 0;
                                            using (SqlConnection connection6 = new SqlConnection(myConnectionString))
                                            {
                                                using (SqlCommand command = new SqlCommand("sp_SendUserNotification", connection6))
                                                {
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.Parameters.Add("@UserID", SqlDbType.BigInt).Value = UserId;
                                                    command.Parameters.Add("@NotificationID", SqlDbType.BigInt).Value = NotificationID;
                                                    command.Parameters.Add("@ReceivalID", SqlDbType.BigInt).Value = SupervisorID;
                                                    SqlParameter outputParam = new SqlParameter("@UserNotificationJunctionID", SqlDbType.BigInt)
                                                    {
                                                        Direction = ParameterDirection.Output
                                                    };
                                                    command.Parameters.Add(outputParam);
                                                    connection6.Open();
                                                    command.ExecuteNonQuery();
                                                    UserNotificationJunctionID = (long?)outputParam.Value;
                                                    connection6.Close();
                                                }
                                            }

                                            string? SenderComments = "";
                                            using (SqlConnection connection7 = new SqlConnection(myConnectionString))
                                            {
                                                using (SqlCommand command = new SqlCommand("SELECT SenderComments FROM OtextendRequestJunctions WHERE OtextendRequestId = @OTExtendRequestID AND IsArchive != 1", connection7))
                                                {
                                                    command.Parameters.Add("@OTExtendRequestID", SqlDbType.BigInt).Value = OTExtendRequestID;
                                                    connection7.Open();
                                                    SenderComments = (string)command.ExecuteScalar();
                                                    connection7.Close();
                                                }
                                            }
                                            bool? _SendEmailToSupervisorForRequestOT = await SendEmailToSupervisorForRequestOT(SenderName, SenderEmail, ReceiverName, ReceiverEmail, SenderComments, UserDailyShiftJunctionId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    connection.Close();
                }

                logger.LogInformation("Worker function end", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public async Task<bool?> UpdateWorkTime(long? UserDailyShiftJunctionID)
        {
            var myConnectionString = this.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];
            bool? IsUpdateWorkTime = false;
            try
            {
                DateTime TodaysDate = DateTime.Today;
                DateTime NextDate = TodaysDate.AddDays(1);
                string? TotalTimeFromCheckIn = "0H  0M";
                long? AttendanceActionStatusID = 0;
                double? _GrossTime = 0;
                double? _ActualWorkTime = 0;
                double? _TotalBreakTime = 0;
                //find current day and shift

                //Shift details                                   
                List<CheckoutProcessModel> list = new List<CheckoutProcessModel>();

                using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                {
                    SqlCommand cmd1 = new SqlCommand("sp_GetListOfAttendanceShifts", connection1);
                    cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@UserDailyShiftJunctionID", SqlDbType.BigInt).Value = UserDailyShiftJunctionID;
                    connection1.Open();

                    using (var reader1 = cmd1.ExecuteReader())
                    {

                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                CheckoutProcessModel obj = new CheckoutProcessModel();
                                obj.UserDailyShiftJunctionId = Convert.ToInt64(reader1["UserDailyShiftJunctionId"]);
                                obj.AttendanceActionStatusID = Convert.ToInt16(reader1["AttendanceActionStatusID"]);
                                obj.ActionDateTime = Convert.ToDateTime(reader1["ActionDateTime"]);
                                list.Add(obj);
                            }
                        }
                    }
                    connection1.Close();
                }
                int TotalNoOfShiftAttendanceRows = list.Count;
                DateTime PreviousStageDateTime = DateTime.Now;
                for (int row = 0; row < list.Count; row++)
                {
                    long UserDailyShiftJunctionId = Convert.ToInt64(list[row].UserDailyShiftJunctionId);
                    AttendanceActionStatusID = Convert.ToInt16(list[row].AttendanceActionStatusID);
                    DateTime ActionDateTime = Convert.ToDateTime(list[row].ActionDateTime);
                    PreviousStageDateTime = ActionDateTime;

                    if (AttendanceActionStatusID == 1)
                    {
                        //for checkIn
                        TimeSpan difference = (DateTime.Now - ActionDateTime);
                        int hours = (int)difference.TotalHours;
                        int minutes = difference.Minutes;
                        TotalTimeFromCheckIn = hours + " H" + " " + minutes + " M";
                        if (TotalNoOfShiftAttendanceRows == 1)
                        {
                            //if only checkin state happand  
                            _ActualWorkTime = _ActualWorkTime + await FindWorkTimeInMinutes(ActionDateTime, DateTime.Now);
                        }
                        else
                        {
                            //if only checkin state happand
                            var ActionDateTimeVar = list[row + 1].ActionDateTime;

                            DateTime NextActionDateTime = Convert.ToDateTime(ActionDateTimeVar);
                            _ActualWorkTime = _ActualWorkTime + await FindWorkTimeInMinutes(ActionDateTime, NextActionDateTime);
                        }

                    }
                    else
                    {
                        //if case of Pause,Resume,Check-Out
                        if (TotalNoOfShiftAttendanceRows == (row + 1))
                        {
                            //if last state
                            //if Pause action
                            if (AttendanceActionStatusID == 2)
                            {
                                //find Break Time
                                _TotalBreakTime = _TotalBreakTime + await FindWorkTimeInMinutes(ActionDateTime, DateTime.Now);
                            }
                            else if (AttendanceActionStatusID == 3)
                            {
                                //find Actual work Time
                                _ActualWorkTime = _ActualWorkTime + await FindWorkTimeInMinutes(ActionDateTime, DateTime.Now);
                            }
                            else if (AttendanceActionStatusID == 4)
                            {
                            }

                        }
                        else
                        {

                            if (AttendanceActionStatusID == 2)
                            {
                                //find Break Time
                                DateTime NextActionDateTime = (DateTime)list[row + 1].ActionDateTime;
                                _TotalBreakTime = _TotalBreakTime + await FindWorkTimeInMinutes(ActionDateTime, NextActionDateTime);

                            }
                            else if (AttendanceActionStatusID == 3)
                            {
                                //find Actual work Time
                                DateTime NextActionDateTime = (DateTime)list[row + 1].ActionDateTime;
                                _ActualWorkTime = _ActualWorkTime + await FindWorkTimeInMinutes(ActionDateTime, NextActionDateTime);
                            }
                            else if (AttendanceActionStatusID == 4)
                            {
                            }
                        }
                    }
                }





                if (UserDailyShiftJunctionID > 0)
                {
                    int recordCount = 0;

                    using (SqlConnection connection = new SqlConnection(myConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand("SELECT COUNT(*) AS RecordCount FROM UserDailyShiftJunction WHERE UserDailyShiftJunctionID=" + UserDailyShiftJunctionID + ";", connection))
                        {
                            connection.Open();
                            recordCount = (int)command.ExecuteScalar();
                        }
                    }
                    if (recordCount > 0)
                    {
                        using (SqlConnection connection2 = new SqlConnection(myConnectionString))
                        {
                            connection2.Open();
                            SqlCommand cmdinsert = new SqlCommand("sp_UpdateUserWorkTime", connection2);
                            cmdinsert.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdinsert.Parameters.Add("@ActualWorkTime", SqlDbType.Float).Value = _ActualWorkTime;
                            cmdinsert.Parameters.Add("@TotalBreakTime", SqlDbType.BigInt).Value = _TotalBreakTime;
                            cmdinsert.Parameters.Add("@UserDailyShiftJunctionID", SqlDbType.BigInt).Value = UserDailyShiftJunctionID;

                            cmdinsert.ExecuteNonQuery();
                            connection2.Close();
                            IsUpdateWorkTime = true;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            return IsUpdateWorkTime;
        }
        public async Task<double?> FindWorkTimeInMinutes(DateTime StartTime, DateTime EndTime)
        {
            double? TimeInMinutes = 0;
            try
            {
                TimeSpan difference = EndTime - StartTime;
                TimeInMinutes = difference.TotalMinutes;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
            return TimeInMinutes;
        }

        public async Task<bool?> UpdateOTWorkTime(long? UserDailyShiftJunctionID)
        {
            var myConnectionString = this.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];
            bool? IsUpdateWorkTime = false;
            try
            {
                //find current day and shift
                DateTime ActionDateTime_OTRequested = DateTime.Now;
                DateTime ActionDateTime_OTCheckOut = DateTime.Now;
                using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SELECT TOP 1 ActionDateTime FROM UserDailyShiftAttendanceJunction WHERE UserDailyShiftJunctionID=" + UserDailyShiftJunctionID + " and AttendanceActionStatusId=5 and IsArchive!='1';", connection1))
                    {
                        connection1.Open();
                        ActionDateTime_OTRequested = (DateTime)command.ExecuteScalar();
                        connection1.Close();
                    }
                }
                using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SELECT TOP 1 ActionDateTime FROM UserDailyShiftAttendanceJunction WHERE UserDailyShiftJunctionID=" + UserDailyShiftJunctionID + " and AttendanceActionStatusId=6 and IsArchive!='1';", connection1))
                    {
                        connection1.Open();
                        ActionDateTime_OTCheckOut = (DateTime)command.ExecuteScalar();
                        connection1.Close();
                    }
                }
                TimeSpan Time = (ActionDateTime_OTCheckOut - ActionDateTime_OTRequested);
                double? ShiftTotalMinutes = Time.TotalMinutes;
                //Shift details                                  

                if (UserDailyShiftJunctionID > 0)
                {
                    int recordCount = 0;

                    using (SqlConnection connection = new SqlConnection(myConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand("SELECT COUNT(*) AS RecordCount FROM UserDailyShiftJunction WHERE UserDailyShiftJunctionID=" + UserDailyShiftJunctionID + " and IsArchive!='1';", connection))
                        {
                            connection.Open();
                            recordCount = (int)command.ExecuteScalar();
                        }
                    }
                    if (recordCount > 0)
                    {
                        using (SqlConnection connection2 = new SqlConnection(myConnectionString))
                        {
                            connection2.Open();
                            SqlCommand cmdinsert = new SqlCommand("sp_UpdateUserOTTime", connection2);
                            cmdinsert.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdinsert.Parameters.Add("@TotalOverTime", SqlDbType.Float).Value = ShiftTotalMinutes;
                            cmdinsert.Parameters.Add("@UserDailyShiftJunctionID", SqlDbType.BigInt).Value = UserDailyShiftJunctionID;

                            cmdinsert.ExecuteNonQuery();
                            connection2.Close();
                            IsUpdateWorkTime = true;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }

            return IsUpdateWorkTime;
        }


        public async Task<bool?> SendEmailToSupervisorForRequestOT(string? SenderName, string? SenderEmail, string? ReceiverName, string? ReceiverEmail, string? SenderComments, long? UserDailyShiftJunctionID)
        {
            bool? sendEmailNotification = false;
            var myConnectionString = this.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];

            try
            {
                string ShiftName = "";
                string ShiftDate = "";

                using (SqlConnection connection = new SqlConnection(myConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetShiftDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserDailyShiftJunctionID", UserDailyShiftJunctionID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            ShiftName = reader["ShiftName"].ToString();
                            DateTime shiftDate = Convert.ToDateTime(reader["ShiftDate"]);
                            ShiftDate = shiftDate.ToString("MM/dd/yyyy");
                        }
                        connection.Close();
                    }
                }

                var appSettingsSectionWebsite = Configuration.GetSection("Wesite");
                var appSettingsSectionEmail = Configuration.GetSection("EmailConfig");
                var websiteUrl = appSettingsSectionWebsite.GetValue<string>("WesiteUrl");
                var apiKey = appSettingsSectionEmail.GetValue<string>("APIKey");
                var templateId = appSettingsSectionEmail.GetValue<string>("MAST_NotificationToSupervisorAboutRequestForApprovalOfOTSentByEmployee");
                var sendEmailFrom = appSettingsSectionEmail.GetValue<string>("Email_From");
                var sendEmailFromName = appSettingsSectionEmail.GetValue<string>("SendEmailFromName");
                var bcc1 = appSettingsSectionEmail.GetValue<string>("Email_Bcc1");
                var bcc2 = appSettingsSectionEmail.GetValue<string>("Email_Bcc2");
                var cc = appSettingsSectionEmail.GetValue<string>("Email_cc");

                SendGridClient client = new SendGridClient(apiKey);
                SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();
                message.From = new EmailAddress(sendEmailFrom, sendEmailFromName);
                message.AddTo(ReceiverEmail);
                message.AddBcc(bcc1, sendEmailFromName);
                message.AddBcc(bcc2, sendEmailFromName);

                message.Subject = "Request for approval Of OT";

                message.SetTemplateId(templateId);
                message.AddSubstitution("@Model.UserName", ReceiverName);
                message.AddSubstitution("@Model.ShiftName", ShiftName);
                message.AddSubstitution("@Model.ShiftDate", ShiftDate);
                message.AddSubstitution("@Model.EmployeeName", SenderName);
                message.AddSubstitution("@Model.EmployeeComment", SenderComments);
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
