using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class BloodGroupMaster
{
    public int BloodGroupId { get; set; }

    public string? BloodGroupName { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
