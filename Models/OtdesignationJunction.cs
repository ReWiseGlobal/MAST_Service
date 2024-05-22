using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class OtdesignationJunction
{
    public long OtdesignationJunctionId { get; set; }

    public int? Otid { get; set; }

    public int? DesignationId { get; set; }

    public double? OtchargesInRupees { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual DesignationMaster? Designation { get; set; }

    public virtual Otmaster? Ot { get; set; }
}
