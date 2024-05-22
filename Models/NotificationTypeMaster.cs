using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class NotificationTypeMaster
{
    public int NotificationTypeId { get; set; }

    public string? NotificationText { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<NotificationMaster> NotificationMasters { get; set; } = new List<NotificationMaster>();
}
