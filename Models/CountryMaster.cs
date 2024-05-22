using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class CountryMaster
{
    public long CountryId { get; set; }

    public string? CountryCode { get; set; }

    public string CountryName { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public string? IsArchive { get; set; }

    public virtual ICollection<RegionMaster> RegionMasters { get; set; } = new List<RegionMaster>();
}
