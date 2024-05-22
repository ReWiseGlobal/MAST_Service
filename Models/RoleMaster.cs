using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class RoleMaster
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<UserRoleJunction> UserRoleJunctions { get; set; } = new List<UserRoleJunction>();
}
