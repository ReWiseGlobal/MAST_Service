using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class SignUpOtpverificationJunction
{
    public long SignUpOtpverificationJunctionId { get; set; }

    public string? EmailAddress { get; set; }

    public string? MobileNumber { get; set; }

    public string? Otp { get; set; }

    public bool? IsOtpexpired { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsArchive { get; set; }
}
