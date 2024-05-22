using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class AttendanceActionStatus
{
    public long AttendanceActionStatusId { get; set; }

    public string? AttendanceActionStatus1 { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<UserDailyShiftAttendanceJunction> UserDailyShiftAttendanceJunctions { get; set; } = new List<UserDailyShiftAttendanceJunction>();
}
