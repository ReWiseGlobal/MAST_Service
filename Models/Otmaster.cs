using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class Otmaster
{
    public int Otid { get; set; }

    public string? OtstartHr { get; set; }

    public string? OtendHr { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<OtdesignationJunction> OtdesignationJunctions { get; set; } = new List<OtdesignationJunction>();
}
