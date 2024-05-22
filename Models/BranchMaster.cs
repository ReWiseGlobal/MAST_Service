using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class BranchMaster
{
    public long BranchId { get; set; }

    public long? OrganizationId { get; set; }

    public long? RegionId { get; set; }

    public string? BranchName { get; set; }

    public string? Address { get; set; }

    public string? ContactNo { get; set; }

    public string? WebsiteUrl { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<DailyShiftMaster> DailyShiftMasters { get; set; } = new List<DailyShiftMaster>();

    public virtual ICollection<DepartmentMaster> DepartmentMasters { get; set; } = new List<DepartmentMaster>();

    public virtual OrganizationMaster? Organization { get; set; }

    public virtual RegionMaster? Region { get; set; }

    public virtual ICollection<ShiftMaster> ShiftMasters { get; set; } = new List<ShiftMaster>();

    public virtual ICollection<UserBranchJunction> UserBranchJunctions { get; set; } = new List<UserBranchJunction>();

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
