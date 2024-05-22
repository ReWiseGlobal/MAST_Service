using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserNotificationJunction
{
    public long UserNotificationJunctionId { get; set; }

    public long? NotificationId { get; set; }

    public long? ReceivalId { get; set; }

    public DateTime? DateOfReceived { get; set; }

    public bool? IsRead { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual NotificationMaster? Notification { get; set; }

    public virtual UserMaster? Receival { get; set; }
}
