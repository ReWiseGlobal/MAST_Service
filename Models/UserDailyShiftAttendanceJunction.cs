using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserDailyShiftAttendanceJunction
{
    public long UserDailyShiftAttendanceId { get; set; }

    public long? UserDailyShiftJunctionId { get; set; }

    public long? UserId { get; set; }

    public long? AttendanceActionStatusId { get; set; }

    public DateTime? ActionDateTime { get; set; }

    public int? WorkModeId { get; set; }

    public bool? IsRegularised { get; set; }

    public double? UserOtextendHours { get; set; }

    public bool? IsOtapproved { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual AttendanceActionStatus? AttendanceActionStatus { get; set; }

    public virtual ICollection<OtextendRequestJunction> OtextendRequestJunctions { get; set; } = new List<OtextendRequestJunction>();

    public virtual UserMaster? User { get; set; }

    public virtual UserDailyShiftJunction? UserDailyShiftJunction { get; set; }

    public virtual WorkModeMaster? WorkMode { get; set; }
}
