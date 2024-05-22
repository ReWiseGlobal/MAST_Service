using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class RegionMaster
{
    public long RegionId { get; set; }

    public long? CountryId { get; set; }

    public long? TimeZoneId { get; set; }

    public string? RegionName { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<BranchMaster> BranchMasters { get; set; } = new List<BranchMaster>();

    public virtual CountryMaster? Country { get; set; }

    public virtual ICollection<ShiftMaster> ShiftMasters { get; set; } = new List<ShiftMaster>();

    public virtual LookupTimeZone? TimeZone { get; set; }
}
