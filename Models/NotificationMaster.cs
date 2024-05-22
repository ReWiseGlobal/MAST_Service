using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class NotificationMaster
{
    public long NotificationId { get; set; }

    public int? NotificationTypeId { get; set; }

    public string? NotificationTitle { get; set; }

    public string? NotificationText { get; set; }

    public long? SenderId { get; set; }

    public long? ActivityId { get; set; }

    public string? EncActivityId { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual NotificationTypeMaster? NotificationType { get; set; }

    public virtual ICollection<UserNotificationJunction> UserNotificationJunctions { get; set; } = new List<UserNotificationJunction>();
}
