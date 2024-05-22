using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserAdHocShiftRequestsJunction
{
    public long UserAdHocShiftRequestId { get; set; }

    public long? UserDailyShiftJunctionId { get; set; }

    public long? SenderId { get; set; }

    public long? SupervisorId { get; set; }

    public string? IsAdHocShiftApproved { get; set; }

    public string? SenderComments { get; set; }

    public string? ReceiverComments { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster? Sender { get; set; }

    public virtual UserMaster? Supervisor { get; set; }

    public virtual UserDailyShiftJunction? UserDailyShiftJunction { get; set; }
}
