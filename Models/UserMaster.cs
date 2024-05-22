using System;
using System.Collections.Generic;

namespace MAST_Service.Models;

public partial class UserMaster
{
    public long UserId { get; set; }

    public long? TenantId { get; set; }

    public long? OrganizationId { get; set; }

    public long? BranchId { get; set; }

    public int? PrefixId { get; set; }

    public long? TimeZoneId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? LoginUserName { get; set; }

    public string? EmailAddress { get; set; }

    public string? Password { get; set; }

    public string? UserPassword { get; set; }

    public Guid? UserGuid { get; set; }

    public string? Address { get; set; }

    public string? MobileNumber { get; set; }

    public string? ContactNumber { get; set; }

    public DateTime? DateofBirth { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsArchive { get; set; }

    public virtual BranchMaster? Branch { get; set; }

    public virtual OrganizationMaster? Organization { get; set; }

    public virtual ICollection<OtextendRequestJunction> OtextendRequestJunctionSenders { get; set; } = new List<OtextendRequestJunction>();

    public virtual ICollection<OtextendRequestJunction> OtextendRequestJunctionSupervisors { get; set; } = new List<OtextendRequestJunction>();

    public virtual PrefixMaster? Prefix { get; set; }

    public virtual ICollection<ShiftChangeRequestMaster> ShiftChangeRequestMasterCreatedByNavigations { get; set; } = new List<ShiftChangeRequestMaster>();

    public virtual ICollection<ShiftChangeRequestMaster> ShiftChangeRequestMasterLastModifiedByNavigations { get; set; } = new List<ShiftChangeRequestMaster>();

    public virtual ICollection<ShiftChangeRequestMaster> ShiftChangeRequestMasterSupervisors { get; set; } = new List<ShiftChangeRequestMaster>();

    public virtual ICollection<ShiftSupervisorJunction> ShiftSupervisorJunctions { get; set; } = new List<ShiftSupervisorJunction>();

    public virtual ICollection<SupervisorUserJunction> SupervisorUserJunctions { get; set; } = new List<SupervisorUserJunction>();

    public virtual TenantMaster? Tenant { get; set; }

    public virtual LookupTimeZone? TimeZone { get; set; }

    public virtual ICollection<UserAdHocShiftRequestsJunction> UserAdHocShiftRequestsJunctionSenders { get; set; } = new List<UserAdHocShiftRequestsJunction>();

    public virtual ICollection<UserAdHocShiftRequestsJunction> UserAdHocShiftRequestsJunctionSupervisors { get; set; } = new List<UserAdHocShiftRequestsJunction>();

    public virtual ICollection<UserBranchJunction> UserBranchJunctions { get; set; } = new List<UserBranchJunction>();

    public virtual ICollection<UserDailyShiftAttendanceJunction> UserDailyShiftAttendanceJunctions { get; set; } = new List<UserDailyShiftAttendanceJunction>();

    public virtual ICollection<UserDailyShiftJunction> UserDailyShiftJunctions { get; set; } = new List<UserDailyShiftJunction>();

    public virtual ICollection<UserDepartmentJunction> UserDepartmentJunctions { get; set; } = new List<UserDepartmentJunction>();

    public virtual ICollection<UserDepartmentTransferJunction> UserDepartmentTransferJunctionCreatedByNavigations { get; set; } = new List<UserDepartmentTransferJunction>();

    public virtual ICollection<UserDepartmentTransferJunction> UserDepartmentTransferJunctionLastModifiedByNavigations { get; set; } = new List<UserDepartmentTransferJunction>();

    public virtual ICollection<UserDepartmentTransferJunction> UserDepartmentTransferJunctionRequestedSupervisors { get; set; } = new List<UserDepartmentTransferJunction>();

    public virtual ICollection<UserDepartmentTransferJunction> UserDepartmentTransferJunctionUsers { get; set; } = new List<UserDepartmentTransferJunction>();

    public virtual ICollection<UserLoginHistory> UserLoginHistories { get; set; } = new List<UserLoginHistory>();

    public virtual ICollection<UserNotificationJunction> UserNotificationJunctions { get; set; } = new List<UserNotificationJunction>();

    public virtual ICollection<UserOtpjunction> UserOtpjunctions { get; set; } = new List<UserOtpjunction>();

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

    public virtual ICollection<UserRoleJunction> UserRoleJunctions { get; set; } = new List<UserRoleJunction>();
}
