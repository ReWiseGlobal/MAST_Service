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
                var data = (from a in _context.UserDailyShiftAttendanceJunctions
                            where a.IsArchive != true && a.AttendanceActionStatusId == 1
                            select a).ToList();
                for(int i = 0; i < data.Count; i++)
                {
                    UserDailyShiftAttendanceJunction? isCheckOut = _context.UserDailyShiftAttendanceJunctions.Where(a => a.UserDailyShiftJunctionId == data[i].UserDailyShiftJunctionId && a.IsArchive != true && a.AttendanceActionStatusId == 4).FirstOrDefault();
                    if (isCheckOut == null)
                    {
                        UserDailyShiftAttendanceJunction userDailyShift = new UserDailyShiftAttendanceJunction();
                        userDailyShift.UserDailyShiftJunctionId = data[i].UserDailyShiftJunctionId;
                        userDailyShift.UserId = data[i].UserId;
                        userDailyShift.AttendanceActionStatusId = 4;
                        userDailyShift.ActionDateTime = (from x in _context.UserDailyShiftJunctions
                                                         join y in _context.DailyShiftMasters on x.DailyShiftId equals y.DailyShiftId
                                                         where x.IsArchive != true && y.IsArchive != true && x.UserDailyShiftJunctionId == data[i].UserDailyShiftJunctionId
                                                         select y.ShiftEndDateTime).FirstOrDefault();
                        userDailyShift.CreatedOn= DateTime.Now;
                        userDailyShift.IsArchive = false;
                        _context.UserDailyShiftAttendanceJunctions.Add(userDailyShift);
                        _context.SaveChanges();

                        bool? IsUpdateWorkTime= await UpdateWorkTime(data[i].UserDailyShiftJunctionId);
                    }
                }

                //For OT
                var OTdata = (from a in _context.UserDailyShiftAttendanceJunctions
                            where a.IsArchive != true && a.AttendanceActionStatusId == 5
                            select a).ToList();
                for (int i = 0; i < OTdata.Count; i++)
                {
                    UserDailyShiftAttendanceJunction? isCheckOut = _context.UserDailyShiftAttendanceJunctions.Where(a => a.UserDailyShiftJunctionId == OTdata[i].UserDailyShiftJunctionId && a.IsArchive != true && a.AttendanceActionStatusId == 6).FirstOrDefault();
                    if (isCheckOut == null)
                    {
                        UserDailyShiftAttendanceJunction userDailyShift = new UserDailyShiftAttendanceJunction();
                        userDailyShift.UserDailyShiftJunctionId = OTdata[i].UserDailyShiftJunctionId;
                        userDailyShift.UserId = OTdata[i].UserId;
                        userDailyShift.AttendanceActionStatusId = 6;
                        var OTHours = (from x in _context.OtextendRequestJunctions
                                                         
                                                         where x.IsArchive != true  && x.UserDailyShiftAttendanceId == OTdata[i].UserDailyShiftAttendanceId
                                                         select x.UserOtextendInMinutes).FirstOrDefault();
                        if(OTHours != null && OTdata[i].ActionDateTime!=null)
                        {
                            userDailyShift.ActionDateTime = Convert.ToDateTime(OTdata[i].ActionDateTime).AddMinutes(Convert.ToDouble(OTHours));
                        }
                        userDailyShift.CreatedOn = DateTime.Now;
                        userDailyShift.IsArchive = false;
                        _context.UserDailyShiftAttendanceJunctions.Add(userDailyShift);
                        _context.SaveChanges();

                        bool? IsUpdateWorkTime = await UpdateOTWorkTime(OTdata[i].UserDailyShiftJunctionId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public async Task<bool?> UpdateWorkTime( long? UserDailyShiftJunctionID)
        {
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
                var UserDailyShiftAttendance = await (from UDDSAJ in _context.UserDailyShiftAttendanceJunctions
                                                      where UDDSAJ.IsArchive != true && UDDSAJ.UserDailyShiftJunctionId == UserDailyShiftJunctionID
                                                      orderby UDDSAJ.UserDailyShiftAttendanceId ascending
                                                      select new
                                                      {
                                                          UserDailyShiftAttendanceID = UDDSAJ.UserDailyShiftAttendanceId,
                                                          AttendanceActionStatusID = UDDSAJ.AttendanceActionStatusId,
                                                          ActionDateTime = UDDSAJ.ActionDateTime,

                                                      }).ToListAsync();

                if (Convert.ToInt16(UserDailyShiftAttendance.Count) != 0)
                {

                    int TotalNoOfShiftAttendanceRows = UserDailyShiftAttendance.Count;
                    DateTime PreviousStageDateTime = DateTime.Now;
                    for (int row = 0; row < UserDailyShiftAttendance.Count; row++)
                    {
                        long? UserDailyShiftAttendanceID = UserDailyShiftAttendance[row].UserDailyShiftAttendanceID;
                        AttendanceActionStatusID = UserDailyShiftAttendance[row].AttendanceActionStatusID;
                        DateTime ActionDateTime = (DateTime)UserDailyShiftAttendance[row].ActionDateTime;
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
                                var ActionDateTimeVar= UserDailyShiftAttendance[row + 1].ActionDateTime;

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
                                    DateTime NextActionDateTime = (DateTime)UserDailyShiftAttendance[row + 1].ActionDateTime;
                                    _TotalBreakTime = _TotalBreakTime + await FindWorkTimeInMinutes(ActionDateTime, NextActionDateTime);

                                }
                                else if (AttendanceActionStatusID == 3)
                                {
                                    //find Actual work Time
                                    DateTime NextActionDateTime = (DateTime)UserDailyShiftAttendance[row + 1].ActionDateTime;
                                    _ActualWorkTime = _ActualWorkTime + await FindWorkTimeInMinutes(ActionDateTime, NextActionDateTime);
                                }
                                else if (AttendanceActionStatusID == 4)
                                {
                                }
                            }
                        }
                    }
                }
                if (UserDailyShiftJunctionID > 0)
                {
                    var UserDailyShiftJunctionData = await _context.UserDailyShiftJunctions.FirstOrDefaultAsync(x => x.UserDailyShiftJunctionId == UserDailyShiftJunctionID);
                    if (UserDailyShiftJunctionData != null)
                    {
                        UserDailyShiftJunctionData.ActualWorkTime = _ActualWorkTime;
                        UserDailyShiftJunctionData.TotalBreakTime = _TotalBreakTime;
                        //UserDailyShiftJunctionData.LastModifiedBy = UserID;
                        UserDailyShiftJunctionData.LastModifiedOn = DateTime.Now;
                        await _context.SaveChangesAsync();
                        IsUpdateWorkTime = true;
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
            bool? IsUpdateWorkTime = false;
            try
            {
                //find current day and shift
                DateTime ActionDateTime_OTRequested = Convert.ToDateTime(await _context.UserDailyShiftAttendanceJunctions.Where(x => x.UserDailyShiftJunctionId == UserDailyShiftJunctionID && x.AttendanceActionStatusId == 5 && x.IsArchive != true).Select(x => x.ActionDateTime).FirstOrDefaultAsync());
                DateTime ActionDateTime_OTCheckOut = Convert.ToDateTime(await _context.UserDailyShiftAttendanceJunctions.Where(x => x.UserDailyShiftJunctionId == UserDailyShiftJunctionID && x.AttendanceActionStatusId == 6 && x.IsArchive != true).Select(x => x.ActionDateTime).FirstOrDefaultAsync());
                TimeSpan Time = (ActionDateTime_OTCheckOut - ActionDateTime_OTRequested);
                double? ShiftTotalMinutes = Time.TotalMinutes;
                //Shift details                                  

                if (UserDailyShiftJunctionID > 0)
                {
                    var UserDailyShiftJunctionData = await _context.UserDailyShiftJunctions.FirstOrDefaultAsync(x => x.UserDailyShiftJunctionId == UserDailyShiftJunctionID);
                    if (UserDailyShiftJunctionData != null)
                    {
                        UserDailyShiftJunctionData.TotalOverTime = ShiftTotalMinutes;
                        //UserDailyShiftJunctionData.LastModifiedBy = UserID;
                        UserDailyShiftJunctionData.LastModifiedOn = DateTime.Now;
                        await _context.SaveChangesAsync();
                        IsUpdateWorkTime = true;
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
