using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserRoleJunction
{
    public long UserRoleId { get; set; }

    public long? UserId { get; set; }

    public int? RoleId { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual RoleMaster? Role { get; set; }

    public virtual UserMaster? User { get; set; }
}
