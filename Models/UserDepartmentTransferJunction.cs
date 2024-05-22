using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserDepartmentTransferJunction
{
    public long UserDepartmentTransferJunctionId { get; set; }

    public long? UserId { get; set; }

    public int? RequestedDepartmentId { get; set; }

    public long? RequestedSupervisorId { get; set; }

    public string? IsApproved { get; set; }

    public string? AdminComments { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster? CreatedByNavigation { get; set; }

    public virtual UserMaster? LastModifiedByNavigation { get; set; }

    public virtual DepartmentMaster? RequestedDepartment { get; set; }

    public virtual UserMaster? RequestedSupervisor { get; set; }

    public virtual UserMaster? User { get; set; }
}
