using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public class CheckoutProcessModel
{
    public long UserDailyShiftJunctionId { get; set; }

    public long UserId { get; set; }

    public DateTime? ShiftEndDateTime { get; set; }

    public DateTime? ShiftStartDateTime { get; set; }
    public int? AttendanceActionStatusID { get; set; }

    public DateTime? ActionDateTime { get; set; }
}
