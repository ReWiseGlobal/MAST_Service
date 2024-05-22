using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class ShiftSupervisorJunction
{
    public long ShiftSupervisorId { get; set; }

    public long? UserId { get; set; }

    public long? ShiftId { get; set; }

    public long? AssignedBy { get; set; }

    public long? CreateBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual UserMaster? User { get; set; }
}
