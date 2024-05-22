using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserDepartmentJunction
{
    public long UserDepartmentId { get; set; }

    public long? UserId { get; set; }

    public int? DepartmentId { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual DepartmentMaster? Department { get; set; }

    public virtual UserMaster? User { get; set; }
}
