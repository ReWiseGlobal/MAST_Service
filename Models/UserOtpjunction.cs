using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserOtpjunction
{
    public long UserOtpid { get; set; }

    public long UserId { get; set; }

    public string? Otp { get; set; }

    public bool? IsFirstTimeOtp { get; set; }

    public bool? IsOtpexpired { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster User { get; set; } = null!;
}
