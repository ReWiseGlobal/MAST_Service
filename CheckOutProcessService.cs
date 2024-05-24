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
                            CheckoutProcessModel obj = new CheckoutProcessModel();
                            long UserDailyShiftJunctionId = Convert.ToInt64(reader["UserDailyShiftJunctionId"]);
                            long UserId = Convert.ToInt64(reader["UserId"]);
                            DateTime ShiftEndDateTime = Convert.ToDateTime(reader["ShiftEndDateTime"]);
                            DateTime ShiftStartDateTime = Convert.ToDateTime(reader["ShiftStartDateTime"]);


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
                                        var endtime = Convert.ToDateTime(ShiftEndDateTime).AddHours(5);
                                        if (DateTime.Now > endtime)
                                        {
                                            

                                            using (SqlConnection connection2 = new SqlConnection(myConnectionString))
                                            {
                                                connection2.Open();
                                                SqlCommand cmdinsert = new SqlCommand("sp_insertCheckOutOfUser", connection2);
                                                cmdinsert.CommandType = System.Data.CommandType.StoredProcedure;
                                                cmdinsert.Parameters.Add("@UserDailyShiftJunctionId", SqlDbType.BigInt).Value = UserDailyShiftJunctionId;
                                                cmdinsert.Parameters.Add("@UserId", SqlDbType.BigInt).Value = UserId;
                                                cmdinsert.Parameters.Add("@AttendanceActionStatusId", SqlDbType.Int).Value = 4;
                                                cmdinsert.Parameters.Add("@ActionDateTime", SqlDbType.DateTime).Value = ShiftEndDateTime;
                                                cmdinsert.ExecuteNonQuery();
                                                connection2.Close();

                                            }

                                            bool? IsUpdateWorkTime = await UpdateWorkTime(UserDailyShiftJunctionId);
                                        }

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
                                using (SqlConnection connection1 = new SqlConnection(myConnectionString))
                                {
                                    using (SqlCommand command = new SqlCommand("SELECT TOP 1 UserOtextendInMinutes FROM OtextendRequestJunction WHERE UserDailyShiftAttendanceID=" + UserDailyShiftAttendanceID + " and IsArchive!='1';", connection1))
                                    {
                                        connection1.Open();
                                        OTHours = (int)command.ExecuteScalar();
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

        public async Task<bool?> UpdateWorkTime( long? UserDailyShiftJunctionID)
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
                        using (SqlCommand command = new SqlCommand("SELECT COUNT(*) AS RecordCount FROM UserDailyShiftJunction WHERE UserDailyShiftJunctionID="+UserDailyShiftJunctionID+";", connection))
                        {
                            connection.Open();
                            recordCount = (int)command.ExecuteScalar();
                        }
                    }
                    if(recordCount > 0)
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
    }
}
