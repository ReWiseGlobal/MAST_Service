using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class SupervisorUserJunction
{
    public long SupervisorUserJunctionId { get; set; }

    public long? ShiftId { get; set; }

    public long? SupervisorId { get; set; }

    public long? UserId { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ShiftMaster? Shift { get; set; }

    public virtual UserMaster? Supervisor { get; set; }
}
