using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class WeekDayMaster
{
    public long WeekDayId { get; set; }

    public string? DayText { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<DailyShiftMaster> DailyShiftMasters { get; set; } = new List<DailyShiftMaster>();
}
