using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class LeaveTypeMaster
{
    public int LeaveTypeId { get; set; }

    public string? LeaveType { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsArchive { get; set; }
}
