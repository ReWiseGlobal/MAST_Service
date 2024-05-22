using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserDailyShiftJunction
{
    public long UserDailyShiftJunctionId { get; set; }

    public long? UserId { get; set; }

    public long? DailyShiftId { get; set; }

    public double? ActualWorkTime { get; set; }

    public double? TotalBreakTime { get; set; }

    public double? TotalOverTime { get; set; }

    public bool? IsAdHocShift { get; set; }

    public string? IsAdHocShiftApproved { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public string? IsHolidayOrWeeklyOff { get; set; }

    public bool? IsLeaveApproved { get; set; }

    public virtual DailyShiftMaster? DailyShift { get; set; }

    public virtual UserMaster? User { get; set; }

    public virtual ICollection<UserAdHocShiftRequestsJunction> UserAdHocShiftRequestsJunctions { get; set; } = new List<UserAdHocShiftRequestsJunction>();

    public virtual ICollection<UserDailyShiftAttendanceJunction> UserDailyShiftAttendanceJunctions { get; set; } = new List<UserDailyShiftAttendanceJunction>();
}
