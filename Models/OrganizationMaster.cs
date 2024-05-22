using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class OrganizationMaster
{
    public long OrganizationId { get; set; }

    public long? TenantId { get; set; }

    public string? OrganizationName { get; set; }

    public string? OrganizationLogoName { get; set; }

    public string? OrganizationLogoFilePath { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<BranchMaster> BranchMasters { get; set; } = new List<BranchMaster>();

    public virtual TenantMaster? Tenant { get; set; }

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
