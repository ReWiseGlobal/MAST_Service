using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class OtextendRequestJunction
{
    public long OtextendRequestId { get; set; }

    public long? UserDailyShiftAttendanceId { get; set; }

    public long? SenderId { get; set; }

    public long? SupervisorId { get; set; }

    public int? UserOtextendInMinutes { get; set; }

    public string? IsOtapproved { get; set; }

    public string? SenderComments { get; set; }

    public string? ReceiverComments { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster? Sender { get; set; }

    public virtual UserMaster? Supervisor { get; set; }

    public virtual UserDailyShiftAttendanceJunction? UserDailyShiftAttendance { get; set; }
}
