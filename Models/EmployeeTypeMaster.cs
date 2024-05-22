using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class EmployeeTypeMaster
{
    public int EmployeeTypeId { get; set; }

    public string? EmployeeType { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
