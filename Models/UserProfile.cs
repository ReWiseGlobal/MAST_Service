using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserProfile
{
    public long ProfileId { get; set; }

    public long? UserId { get; set; }

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public int? EmployeeTypeId { get; set; }

    public string? ProfilePic { get; set; }

    public string? AboutUser { get; set; }

    public string? Gender { get; set; }

    public string? MaritalStatus { get; set; }

    public int? BloodGroupId { get; set; }

    public string? SkypeId { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public DateTime? DateOfRelease { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifyBy { get; set; }

    public DateTime? LastModifyOn { get; set; }

    public bool? IsArchive { get; set; }

    public virtual BloodGroupMaster? BloodGroup { get; set; }

    public virtual DepartmentMaster? Department { get; set; }

    public virtual DesignationMaster? Designation { get; set; }

    public virtual EmployeeTypeMaster? EmployeeType { get; set; }

    public virtual UserMaster? User { get; set; }
}
