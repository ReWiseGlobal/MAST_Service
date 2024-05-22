using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class ShiftChangeRequestMaster
{
    public long ShiftChangeRequestId { get; set; }

    public long? ShiftId { get; set; }

    public long? SupervisorId { get; set; }

    public long? ProposedShiftChangeId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? AdminComments { get; set; }

    public string? SupervisorComments { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster? CreatedByNavigation { get; set; }

    public virtual UserMaster? LastModifiedByNavigation { get; set; }

    public virtual ShiftProposedChangeRequestMaster? ProposedShiftChange { get; set; }

    public virtual ShiftMaster? Shift { get; set; }

    public virtual UserMaster? Supervisor { get; set; }
}
