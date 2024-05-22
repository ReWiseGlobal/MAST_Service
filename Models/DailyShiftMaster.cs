using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class DailyShiftMaster
{
    public long DailyShiftId { get; set; }

    public long? ShiftId { get; set; }

    public long? WeekDayId { get; set; }

    public DateTime? ShiftDate { get; set; }

    public TimeSpan? ShiftStartTime { get; set; }

    public TimeSpan? ShiftEndTime { get; set; }

    public DateTime? ShiftStartDateTime { get; set; }

    public DateTime? ShiftEndDateTime { get; set; }

    public string? IsHolidayOrWeeklyOff { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public long? BranchId { get; set; }

    public virtual BranchMaster? Branch { get; set; }

    public virtual ShiftMaster? Shift { get; set; }

    public virtual ICollection<UserDailyShiftJunction> UserDailyShiftJunctions { get; set; } = new List<UserDailyShiftJunction>();

    public virtual WeekDayMaster? WeekDay { get; set; }
}
