using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class WorkModeMaster
{
    public int WorkModeId { get; set; }

    public string? WorkModeName { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<UserDailyShiftAttendanceJunction> UserDailyShiftAttendanceJunctions { get; set; } = new List<UserDailyShiftAttendanceJunction>();
}
