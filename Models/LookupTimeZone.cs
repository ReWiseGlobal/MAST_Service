using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class LookupTimeZone
{
    public long TimeZoneId { get; set; }

    public string? TimeZoneName { get; set; }

    public string? UtctimeZoneName { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string? IsArchive { get; set; }

    public virtual ICollection<RegionMaster> RegionMasters { get; set; } = new List<RegionMaster>();

    public virtual ICollection<ShiftMaster> ShiftMasters { get; set; } = new List<ShiftMaster>();

    public virtual ICollection<UserMaster> UserMasters { get; set; } = new List<UserMaster>();
}
