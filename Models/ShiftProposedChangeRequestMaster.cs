using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class ShiftProposedChangeRequestMaster
{
    public long ProposedShiftChangeId { get; set; }

    public long? ShiftId { get; set; }

    public string? ShiftTitle { get; set; }

    public TimeSpan? ShiftStartTime { get; set; }

    public TimeSpan? ShiftEndTime { get; set; }

    public double? ShiftAllownce { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ShiftMaster? Shift { get; set; }

    public virtual ICollection<ShiftChangeRequestMaster> ShiftChangeRequestMasters { get; set; } = new List<ShiftChangeRequestMaster>();
}
