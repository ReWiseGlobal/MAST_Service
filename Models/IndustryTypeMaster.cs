using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class IndustryTypeMaster
{
    public long IndustryTypeId { get; set; }

    public string? IndustryTypeName { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<TenantMaster> TenantMasters { get; set; } = new List<TenantMaster>();
}
