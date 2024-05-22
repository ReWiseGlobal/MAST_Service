using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class DesignationMaster
{
    public int DesignationId { get; set; }

    public string? DesignationName { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<OtdesignationJunction> OtdesignationJunctions { get; set; } = new List<OtdesignationJunction>();

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
