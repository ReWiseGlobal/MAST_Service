using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class TenantMaster
{
    public long TenantId { get; set; }

    public string? TenantName { get; set; }

    public long? IndustryTypeId { get; set; }

    public string? SuburbName { get; set; }

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }

    public string? Website { get; set; }

    public string? TenantLogoUrl { get; set; }

    public string? Fax { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual IndustryTypeMaster? IndustryType { get; set; }

    public virtual ICollection<OrganizationMaster> OrganizationMasters { get; set; } = new List<OrganizationMaster>();

    public virtual ICollection<ShiftMaster> ShiftMasters { get; set; } = new List<ShiftMaster>();

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
