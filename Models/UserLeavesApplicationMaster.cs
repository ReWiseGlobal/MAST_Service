using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserLeavesApplicationMaster
{
    public long UserLeaveApplicationId { get; set; }

    public long? UserId { get; set; }

    public int? LeaveTypeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public double? NumberOfDays { get; set; }

    public string? LeaveApprovalStatus { get; set; }

    public string? Remarks { get; set; }

    public long? LeaveApprovedBy { get; set; }

    public DateTime? LeaveApprovalDate { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }
}
