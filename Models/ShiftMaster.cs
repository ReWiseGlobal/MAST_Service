using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class ShiftMaster
{
    public long ShiftId { get; set; }

    public long? RegionId { get; set; }

    public long? BranchId { get; set; }

    public long? TenantId { get; set; }

    public long? TimeZoneId { get; set; }

    public string? ShiftTitle { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public bool? IsEligibleForShiftAllowance { get; set; }

    public double? ShiftAllowance { get; set; }

    public long? CurrencyId { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public bool? IsAdHocShift { get; set; }

    public virtual BranchMaster? Branch { get; set; }

    public virtual CurrencyMaster? Currency { get; set; }

    public virtual ICollection<DailyShiftMaster> DailyShiftMasters { get; set; } = new List<DailyShiftMaster>();

    public virtual RegionMaster? Region { get; set; }

    public virtual ICollection<ShiftChangeRequestMaster> ShiftChangeRequestMasters { get; set; } = new List<ShiftChangeRequestMaster>();

    public virtual ICollection<ShiftProposedChangeRequestMaster> ShiftProposedChangeRequestMasters { get; set; } = new List<ShiftProposedChangeRequestMaster>();

    public virtual ICollection<SupervisorUserJunction> SupervisorUserJunctions { get; set; } = new List<SupervisorUserJunction>();

    public virtual TenantMaster? Tenant { get; set; }

    public virtual LookupTimeZone? TimeZone { get; set; }
}
