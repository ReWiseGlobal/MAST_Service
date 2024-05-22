using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserLoginHistory
{
    public long UserLoginHistoryId { get; set; }

    public long UserId { get; set; }

    public string? Device { get; set; }

    public string? LastBrowserUsed { get; set; }

    public string? UserToken { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshtokenExpiryTime { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster User { get; set; } = null!;
}
