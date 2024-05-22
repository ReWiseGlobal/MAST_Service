using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class PrefixMaster
{
    public int PrefixId { get; set; }

    public string? PrefixName { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
