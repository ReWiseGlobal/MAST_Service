using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class CurrencyMaster
{
    public long CurrencyId { get; set; }

    public string Name { get; set; } = null!;

    public string Sign { get; set; } = null!;

    public double? ExchangeRate { get; set; }

    public int? DecimalPlaces { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public long? OwnerLicenseeId { get; set; }

    public string? IsArchive { get; set; }

    public string? IsSystemgenerated { get; set; }

    public virtual ICollection<ShiftMaster> ShiftMasters { get; set; } = new List<ShiftMaster>();
}
