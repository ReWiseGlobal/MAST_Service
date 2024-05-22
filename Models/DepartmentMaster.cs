using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class DepartmentMaster
{
    public int DeptartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public long? BranchId { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual BranchMaster? Branch { get; set; }

    public virtual ICollection<UserDepartmentJunction> UserDepartmentJunctions { get; set; } = new List<UserDepartmentJunction>();

    public virtual ICollection<UserDepartmentTransferJunction> UserDepartmentTransferJunctions { get; set; } = new List<UserDepartmentTransferJunction>();

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
