using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MAST_Service.Models;

public partial class MastContext : DbContext
{
    public MastContext()
    {
    }

    public MastContext(DbContextOptions<MastContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttendanceActionStatus> AttendanceActionStatuses { get; set; }

    public virtual DbSet<BloodGroupMaster> BloodGroupMasters { get; set; }

    public virtual DbSet<BranchMaster> BranchMasters { get; set; }

    public virtual DbSet<CountryMaster> CountryMasters { get; set; }

    public virtual DbSet<CurrencyMaster> CurrencyMasters { get; set; }

    public virtual DbSet<DailyShiftMaster> DailyShiftMasters { get; set; }

    public virtual DbSet<DepartmentMaster> DepartmentMasters { get; set; }

    public virtual DbSet<DesignationMaster> DesignationMasters { get; set; }

    public virtual DbSet<EmployeeTypeMaster> EmployeeTypeMasters { get; set; }

    public virtual DbSet<IndustryTypeMaster> IndustryTypeMasters { get; set; }

    public virtual DbSet<LeaveTypeMaster> LeaveTypeMasters { get; set; }

    public virtual DbSet<LookupTimeZone> LookupTimeZones { get; set; }

    public virtual DbSet<NotificationMaster> NotificationMasters { get; set; }

    public virtual DbSet<NotificationTypeMaster> NotificationTypeMasters { get; set; }

    public virtual DbSet<OrganizationMaster> OrganizationMasters { get; set; }

    public virtual DbSet<OtdesignationJunction> OtdesignationJunctions { get; set; }

    public virtual DbSet<OtextendRequestJunction> OtextendRequestJunctions { get; set; }

    public virtual DbSet<Otmaster> Otmasters { get; set; }

    public virtual DbSet<PrefixMaster> PrefixMasters { get; set; }

    public virtual DbSet<RegionMaster> RegionMasters { get; set; }

    public virtual DbSet<RoleMaster> RoleMasters { get; set; }

    public virtual DbSet<ShiftChangeRequestMaster> ShiftChangeRequestMasters { get; set; }

    public virtual DbSet<ShiftMaster> ShiftMasters { get; set; }

    public virtual DbSet<ShiftProposedChangeRequestMaster> ShiftProposedChangeRequestMasters { get; set; }

    public virtual DbSet<ShiftSupervisorJunction> ShiftSupervisorJunctions { get; set; }

    public virtual DbSet<SignUpOtpverificationJunction> SignUpOtpverificationJunctions { get; set; }

    public virtual DbSet<SupervisorUserJunction> SupervisorUserJunctions { get; set; }

    public virtual DbSet<TenantMaster> TenantMasters { get; set; }

    public virtual DbSet<UserAdHocShiftRequestsJunction> UserAdHocShiftRequestsJunctions { get; set; }

    public virtual DbSet<UserBranchJunction> UserBranchJunctions { get; set; }

    public virtual DbSet<UserDailyShiftAttendanceJunction> UserDailyShiftAttendanceJunctions { get; set; }

    public virtual DbSet<UserDailyShiftJunction> UserDailyShiftJunctions { get; set; }

    public virtual DbSet<UserDepartmentJunction> UserDepartmentJunctions { get; set; }

    public virtual DbSet<UserDepartmentTransferJunction> UserDepartmentTransferJunctions { get; set; }

    public virtual DbSet<UserLeavesApplicationMaster> UserLeavesApplicationMasters { get; set; }

    public virtual DbSet<UserLoginHistory> UserLoginHistories { get; set; }

    public virtual DbSet<UserMaster> UserMasters { get; set; }

    public virtual DbSet<UserNotificationJunction> UserNotificationJunctions { get; set; }

    public virtual DbSet<UserOtpjunction> UserOtpjunctions { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserRoleJunction> UserRoleJunctions { get; set; }

    public virtual DbSet<WeekDayMaster> WeekDayMasters { get; set; }

    public virtual DbSet<WorkModeMaster> WorkModeMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-22O7208;Initial Catalog=MAST;Integrated Security=True;MultipleActiveResultSets=true;User ID=sa;Password=etech123;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceActionStatus>(entity =>
        {
            entity.ToTable("AttendanceActionStatus");

            entity.Property(e => e.AttendanceActionStatusId).HasColumnName("AttendanceActionStatusID");
            entity.Property(e => e.AttendanceActionStatus1)
                .HasMaxLength(500)
                .HasColumnName("AttendanceActionStatus");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<BloodGroupMaster>(entity =>
        {
            entity.HasKey(e => e.BloodGroupId);

            entity.ToTable("BloodGroupMaster");

            entity.Property(e => e.BloodGroupId).HasColumnName("BloodGroupID");
            entity.Property(e => e.BloodGroupName).HasMaxLength(500);
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<BranchMaster>(entity =>
        {
            entity.HasKey(e => e.BranchId);

            entity.ToTable("BranchMaster");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.BranchName).HasMaxLength(500);
            entity.Property(e => e.ContactNo).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(500)
                .HasColumnName("WebsiteURL");

            entity.HasOne(d => d.Organization).WithMany(p => p.BranchMasters)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_BranchMaster_OrganizationMaster");

            entity.HasOne(d => d.Region).WithMany(p => p.BranchMasters)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK_BranchMaster_RegionMaster");
        });

        modelBuilder.Entity<CountryMaster>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK_CountryMaster_1");

            entity.ToTable("CountryMaster");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.CountryName).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive)
                .HasMaxLength(1)
                .HasDefaultValueSql("((0))")
                .IsFixedLength();
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CurrencyMaster>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK_CurrencyMaster_1");

            entity.ToTable("CurrencyMaster");

            entity.Property(e => e.CurrencyId).HasColumnName("Currency_ID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.IsSystemgenerated)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OwnerLicenseeId).HasColumnName("OwnerLicenseeID");
            entity.Property(e => e.Sign).HasMaxLength(50);
        });

        modelBuilder.Entity<DailyShiftMaster>(entity =>
        {
            entity.HasKey(e => e.DailyShiftId);

            entity.ToTable("DailyShiftMaster");

            entity.Property(e => e.DailyShiftId).HasColumnName("DailyShiftID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsHolidayOrWeeklyOff)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ShiftDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftEndDateTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.ShiftStartDateTime).HasColumnType("datetime");
            entity.Property(e => e.WeekDayId).HasColumnName("WeekDayID");

            entity.HasOne(d => d.Branch).WithMany(p => p.DailyShiftMasters)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_DailyShiftMaster_BranchMaster");

            entity.HasOne(d => d.Shift).WithMany(p => p.DailyShiftMasters)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_DailyShiftMaster_ShiftMaster");

            entity.HasOne(d => d.WeekDay).WithMany(p => p.DailyShiftMasters)
                .HasForeignKey(d => d.WeekDayId)
                .HasConstraintName("FK_DailyShiftMaster_WeekDayMaster");
        });

        modelBuilder.Entity<DepartmentMaster>(entity =>
        {
            entity.HasKey(e => e.DeptartmentId);

            entity.ToTable("DepartmentMaster");

            entity.Property(e => e.DeptartmentId).HasColumnName("DeptartmentID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName).HasMaxLength(200);
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Branch).WithMany(p => p.DepartmentMasters)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_DepartmentMaster_BranchMaster");
        });

        modelBuilder.Entity<DesignationMaster>(entity =>
        {
            entity.HasKey(e => e.DesignationId);

            entity.ToTable("DesignationMaster");

            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DesignationName).HasMaxLength(200);
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<EmployeeTypeMaster>(entity =>
        {
            entity.HasKey(e => e.EmployeeTypeId);

            entity.ToTable("EmployeeTypeMaster");

            entity.Property(e => e.EmployeeTypeId).HasColumnName("EmployeeTypeID");
            entity.Property(e => e.EmployeeType).HasMaxLength(50);
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<IndustryTypeMaster>(entity =>
        {
            entity.HasKey(e => e.IndustryTypeId);

            entity.ToTable("IndustryTypeMaster");

            entity.Property(e => e.IndustryTypeId).HasColumnName("IndustryTypeID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IndustryTypeName).HasMaxLength(1000);
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LeaveTypeMaster>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId);

            entity.ToTable("LeaveTypeMaster");

            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LeaveType)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<LookupTimeZone>(entity =>
        {
            entity.HasKey(e => e.TimeZoneId);

            entity.ToTable("LookupTimeZone");

            entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive)
                .HasMaxLength(1)
                .HasDefaultValueSql("((0))")
                .IsFixedLength();
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.TimeZoneName).HasMaxLength(1000);
            entity.Property(e => e.UtctimeZoneName)
                .HasMaxLength(1000)
                .HasColumnName("UTCTimeZoneName");
        });

        modelBuilder.Entity<NotificationMaster>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.ToTable("NotificationMaster");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EncActivityId)
                .HasMaxLength(2000)
                .HasColumnName("Enc_ActivityID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NotificationTitle).HasMaxLength(500);
            entity.Property(e => e.NotificationTypeId).HasColumnName("NotificationTypeID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.NotificationMasters)
                .HasForeignKey(d => d.NotificationTypeId)
                .HasConstraintName("FK_NotificationMaster_NotificationTypeMaster");
        });

        modelBuilder.Entity<NotificationTypeMaster>(entity =>
        {
            entity.HasKey(e => e.NotificationTypeId);

            entity.ToTable("NotificationTypeMaster");

            entity.Property(e => e.NotificationTypeId).HasColumnName("NotificationTypeID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.NotificationText).HasMaxLength(500);
        });

        modelBuilder.Entity<OrganizationMaster>(entity =>
        {
            entity.HasKey(e => e.OrganizationId);

            entity.ToTable("OrganizationMaster");

            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.OrganizationLogoFilePath).HasMaxLength(500);
            entity.Property(e => e.OrganizationLogoName).HasMaxLength(500);
            entity.Property(e => e.OrganizationName).HasMaxLength(1000);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");

            entity.HasOne(d => d.Tenant).WithMany(p => p.OrganizationMasters)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK_OrganizationMaster_TenantMaster");
        });

        modelBuilder.Entity<OtdesignationJunction>(entity =>
        {
            entity.ToTable("OTDesignationJunction");

            entity.Property(e => e.OtdesignationJunctionId).HasColumnName("OTDesignationJunctionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OtchargesInRupees).HasColumnName("OTChargesInRupees");
            entity.Property(e => e.Otid).HasColumnName("OTID");

            entity.HasOne(d => d.Designation).WithMany(p => p.OtdesignationJunctions)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK_OTDesignationJunction_DesignationMaster");

            entity.HasOne(d => d.Ot).WithMany(p => p.OtdesignationJunctions)
                .HasForeignKey(d => d.Otid)
                .HasConstraintName("FK_OTDesignationJunction_OTMaster");
        });

        modelBuilder.Entity<OtextendRequestJunction>(entity =>
        {
            entity.HasKey(e => e.OtextendRequestId);

            entity.ToTable("OTExtendRequestJunction");

            entity.Property(e => e.OtextendRequestId).HasColumnName("OTExtendRequestID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsOtapproved)
                .HasMaxLength(1)
                .HasDefaultValueSql("((0))")
                .IsFixedLength()
                .HasColumnName("IsOTApproved");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");
            entity.Property(e => e.UserDailyShiftAttendanceId).HasColumnName("UserDailyShiftAttendanceID");
            entity.Property(e => e.UserOtextendInMinutes).HasColumnName("UserOTExtendInMinutes");

            entity.HasOne(d => d.Sender).WithMany(p => p.OtextendRequestJunctionSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_OTExtendRequestJunction_UserMaster_Sender");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.OtextendRequestJunctionSupervisors)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK_OTExtendRequestJunction_UserMaster_Receiver");

            entity.HasOne(d => d.UserDailyShiftAttendance).WithMany(p => p.OtextendRequestJunctions)
                .HasForeignKey(d => d.UserDailyShiftAttendanceId)
                .HasConstraintName("FK_OTExtendRequestJunction_UserDailyShiftAttendanceJunction");
        });

        modelBuilder.Entity<Otmaster>(entity =>
        {
            entity.HasKey(e => e.Otid);

            entity.ToTable("OTMaster");

            entity.Property(e => e.Otid).HasColumnName("OTID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OtendHr)
                .HasMaxLength(50)
                .HasColumnName("OTEndHR");
            entity.Property(e => e.OtstartHr)
                .HasMaxLength(50)
                .HasColumnName("OTStartHR");
        });

        modelBuilder.Entity<PrefixMaster>(entity =>
        {
            entity.HasKey(e => e.PrefixId);

            entity.ToTable("PrefixMaster");

            entity.Property(e => e.PrefixId).HasColumnName("PrefixID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.PrefixName).HasMaxLength(500);
        });

        modelBuilder.Entity<RegionMaster>(entity =>
        {
            entity.HasKey(e => e.RegionId);

            entity.ToTable("RegionMaster");

            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.RegionName).HasMaxLength(1000);
            entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");

            entity.HasOne(d => d.Country).WithMany(p => p.RegionMasters)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_RegionMaster_CountryMaster");

            entity.HasOne(d => d.TimeZone).WithMany(p => p.RegionMasters)
                .HasForeignKey(d => d.TimeZoneId)
                .HasConstraintName("FK_RegionMaster_LookupTimeZone");
        });

        modelBuilder.Entity<RoleMaster>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("RoleMaster");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.RoleName).HasMaxLength(500);
        });

        modelBuilder.Entity<ShiftChangeRequestMaster>(entity =>
        {
            entity.HasKey(e => e.ShiftChangeRequestId);

            entity.ToTable("ShiftChangeRequestMaster");

            entity.Property(e => e.ShiftChangeRequestId).HasColumnName("ShiftChangeRequestID");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ProposedShiftChangeId).HasColumnName("ProposedShiftChangeID");
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShiftChangeRequestMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_ShiftChangeRequestMaster_UserMaster1");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.ShiftChangeRequestMasterLastModifiedByNavigations)
                .HasForeignKey(d => d.LastModifiedBy)
                .HasConstraintName("FK_ShiftChangeRequestMaster_UserMaster2");

            entity.HasOne(d => d.ProposedShiftChange).WithMany(p => p.ShiftChangeRequestMasters)
                .HasForeignKey(d => d.ProposedShiftChangeId)
                .HasConstraintName("FK_ShiftChangeRequestMaster_ShiftProposedChangeRequestMaster");

            entity.HasOne(d => d.Shift).WithMany(p => p.ShiftChangeRequestMasters)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_ShiftChangeRequestMaster_ShiftMaster");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.ShiftChangeRequestMasterSupervisors)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK_ShiftChangeRequestMaster_UserMaster");
        });

        modelBuilder.Entity<ShiftMaster>(entity =>
        {
            entity.HasKey(e => e.ShiftId);

            entity.ToTable("ShiftMaster");

            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.IsAdHocShift)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IsAd_hocShift");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsWeekOffShift)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IsWeek_OffShift");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.ShiftTitle).HasMaxLength(500);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
            entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");

            entity.HasOne(d => d.Branch).WithMany(p => p.ShiftMasters)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_ShiftMaster_BranchMaster");

            entity.HasOne(d => d.Currency).WithMany(p => p.ShiftMasters)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_ShiftMaster_CurrencyMaster");

            entity.HasOne(d => d.Region).WithMany(p => p.ShiftMasters)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK_ShiftMaster_RegionMaster");

            entity.HasOne(d => d.Tenant).WithMany(p => p.ShiftMasters)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK_ShiftMaster_TenantMaster");

            entity.HasOne(d => d.TimeZone).WithMany(p => p.ShiftMasters)
                .HasForeignKey(d => d.TimeZoneId)
                .HasConstraintName("FK_ShiftMaster_LookupTimeZone");
        });

        modelBuilder.Entity<ShiftProposedChangeRequestMaster>(entity =>
        {
            entity.HasKey(e => e.ProposedShiftChangeId);

            entity.ToTable("ShiftProposedChangeRequestMaster");

            entity.Property(e => e.ProposedShiftChangeId).HasColumnName("ProposedShiftChangeID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.ShiftTitle).HasMaxLength(500);

            entity.HasOne(d => d.Shift).WithMany(p => p.ShiftProposedChangeRequestMasters)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_ShiftProposedChangeRequestMaster_ShiftMaster");
        });

        modelBuilder.Entity<ShiftSupervisorJunction>(entity =>
        {
            entity.HasKey(e => e.ShiftSupervisorId).HasName("PK_ShiftSupversiorJunction");

            entity.ToTable("ShiftSupervisorJunction");

            entity.Property(e => e.ShiftSupervisorId).HasColumnName("ShiftSupervisorID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ShiftSupervisorJunctions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ShiftSupervisorJunction_UserMaster");
        });

        modelBuilder.Entity<SignUpOtpverificationJunction>(entity =>
        {
            entity.ToTable("SignUpOTPVerificationJunction");

            entity.Property(e => e.SignUpOtpverificationJunctionId).HasColumnName("SignUpOTPVerificationJunctionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(500);
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsOtpexpired)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IsOTPExpired");
            entity.Property(e => e.MobileNumber).HasMaxLength(50);
            entity.Property(e => e.Otp)
                .HasMaxLength(10)
                .HasColumnName("OTP");
        });

        modelBuilder.Entity<SupervisorUserJunction>(entity =>
        {
            entity.ToTable("SupervisorUserJunction");

            entity.Property(e => e.SupervisorUserJunctionId).HasColumnName("SupervisorUserJunctionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Shift).WithMany(p => p.SupervisorUserJunctions)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_SupervisorUserJunction_ShiftMaster");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.SupervisorUserJunctions)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK_SupervisorUserJunction_UserMaster");
        });

        modelBuilder.Entity<TenantMaster>(entity =>
        {
            entity.HasKey(e => e.TenantId);

            entity.ToTable("TenantMaster");

            entity.Property(e => e.TenantId).HasColumnName("TenantID");
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Fax).HasMaxLength(500);
            entity.Property(e => e.IndustryTypeId).HasColumnName("IndustryTypeID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.SuburbName).HasMaxLength(500);
            entity.Property(e => e.TenantLogoUrl)
                .HasMaxLength(1000)
                .HasColumnName("TenantLogoURL");
            entity.Property(e => e.TenantName).HasMaxLength(1000);
            entity.Property(e => e.Website).HasMaxLength(1000);

            entity.HasOne(d => d.IndustryType).WithMany(p => p.TenantMasters)
                .HasForeignKey(d => d.IndustryTypeId)
                .HasConstraintName("FK_TenantMaster_IndustryTypeMaster");
        });

        modelBuilder.Entity<UserAdHocShiftRequestsJunction>(entity =>
        {
            entity.HasKey(e => e.UserAdHocShiftRequestId);

            entity.ToTable("UserAd_HocShiftRequestsJunction");

            entity.Property(e => e.UserAdHocShiftRequestId).HasColumnName("UserAd_HocShiftRequestID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsAdHocShiftApproved)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("IsAd_HocShiftApproved");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");
            entity.Property(e => e.UserDailyShiftJunctionId).HasColumnName("UserDailyShiftJunctionID");

            entity.HasOne(d => d.Sender).WithMany(p => p.UserAdHocShiftRequestsJunctionSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_UserAd_HocShiftRequestsJunction_UserMaster_Sender");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.UserAdHocShiftRequestsJunctionSupervisors)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK_UserAd_HocShiftRequestsJunction_UserMaster_Receiver");

            entity.HasOne(d => d.UserDailyShiftJunction).WithMany(p => p.UserAdHocShiftRequestsJunctions)
                .HasForeignKey(d => d.UserDailyShiftJunctionId)
                .HasConstraintName("FK_UserAd_HocShiftRequestsJunction_UserDailyShiftJunction");
        });

        modelBuilder.Entity<UserBranchJunction>(entity =>
        {
            entity.HasKey(e => e.UserBranchId);

            entity.ToTable("UserBranchJunction");

            entity.Property(e => e.UserBranchId).HasColumnName("UserBranchID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Branch).WithMany(p => p.UserBranchJunctions)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_UserBranchJunction_BranchMaster");

            entity.HasOne(d => d.User).WithMany(p => p.UserBranchJunctions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserBranchJunction_UserMaster");
        });

        modelBuilder.Entity<UserDailyShiftAttendanceJunction>(entity =>
        {
            entity.HasKey(e => e.UserDailyShiftAttendanceId);

            entity.ToTable("UserDailyShiftAttendanceJunction");

            entity.Property(e => e.UserDailyShiftAttendanceId).HasColumnName("UserDailyShiftAttendanceID");
            entity.Property(e => e.ActionDateTime).HasColumnType("datetime");
            entity.Property(e => e.AttendanceActionStatusId).HasColumnName("AttendanceActionStatusID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsOtapproved)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IsOTApproved");
            entity.Property(e => e.IsRegularised).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.UserDailyShiftJunctionId).HasColumnName("UserDailyShiftJunctionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserOtextendHours).HasColumnName("UserOTExtendHours");
            entity.Property(e => e.WorkModeId).HasColumnName("WorkModeID");

            entity.HasOne(d => d.AttendanceActionStatus).WithMany(p => p.UserDailyShiftAttendanceJunctions)
                .HasForeignKey(d => d.AttendanceActionStatusId)
                .HasConstraintName("FK_UserDailyShiftAttendanceJunction_AttendanceActionStatus");

            entity.HasOne(d => d.UserDailyShiftJunction).WithMany(p => p.UserDailyShiftAttendanceJunctions)
                .HasForeignKey(d => d.UserDailyShiftJunctionId)
                .HasConstraintName("FK_UserDailyShiftAttendanceJunction_UserDailyShiftJunction");

            entity.HasOne(d => d.User).WithMany(p => p.UserDailyShiftAttendanceJunctions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserDailyShiftAttendanceJunction_UserMaster");

            entity.HasOne(d => d.WorkMode).WithMany(p => p.UserDailyShiftAttendanceJunctions)
                .HasForeignKey(d => d.WorkModeId)
                .HasConstraintName("FK_UserDailyShiftAttendanceJunction_WorkModeMaster");
        });

        modelBuilder.Entity<UserDailyShiftJunction>(entity =>
        {
            entity.ToTable("UserDailyShiftJunction");

            entity.Property(e => e.UserDailyShiftJunctionId).HasColumnName("UserDailyShiftJunctionID");
            entity.Property(e => e.ActualWorkTime).HasDefaultValueSql("((0))");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DailyShiftId).HasColumnName("DailyShiftID");
            entity.Property(e => e.IsAdHocShift)
                .HasDefaultValueSql("((0))")
                .HasColumnName("IsAd_hocShift");
            entity.Property(e => e.IsAdHocShiftApproved)
                .HasMaxLength(1)
                .HasDefaultValueSql("((0))")
                .IsFixedLength()
                .HasColumnName("IsAd_hocShiftApproved");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsHolidayOrWeeklyOff)
                .HasMaxLength(1)
                .HasDefaultValueSql("((0))")
                .IsFixedLength();
            entity.Property(e => e.IsLeaveApproved).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.TotalBreakTime).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalOverTime).HasDefaultValueSql("((0))");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.DailyShift).WithMany(p => p.UserDailyShiftJunctions)
                .HasForeignKey(d => d.DailyShiftId)
                .HasConstraintName("FK_UserDailyShiftJunction_DailyShiftMaster");

            entity.HasOne(d => d.User).WithMany(p => p.UserDailyShiftJunctions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserDailyShiftJunction_UserMaster");
        });

        modelBuilder.Entity<UserDepartmentJunction>(entity =>
        {
            entity.HasKey(e => e.UserDepartmentId);

            entity.ToTable("UserDepartmentJunction");

            entity.Property(e => e.UserDepartmentId).HasColumnName("UserDepartmentID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Department).WithMany(p => p.UserDepartmentJunctions)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_UserDepartmentJunction_DepartmentMaster");

            entity.HasOne(d => d.User).WithMany(p => p.UserDepartmentJunctions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserDepartmentJunction_UserMaster");
        });

        modelBuilder.Entity<UserDepartmentTransferJunction>(entity =>
        {
            entity.ToTable("UserDepartmentTransferJunction");

            entity.Property(e => e.UserDepartmentTransferJunctionId).HasColumnName("UserDepartmentTransferJunctionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsApproved)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.RequestedDepartmentId).HasColumnName("RequestedDepartmentID");
            entity.Property(e => e.RequestedSupervisorId).HasColumnName("RequestedSupervisorID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserDepartmentTransferJunctionCreatedByNavigations).HasForeignKey(d => d.CreatedBy);

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.UserDepartmentTransferJunctionLastModifiedByNavigations).HasForeignKey(d => d.LastModifiedBy);

            entity.HasOne(d => d.RequestedDepartment).WithMany(p => p.UserDepartmentTransferJunctions)
                .HasForeignKey(d => d.RequestedDepartmentId)
                .HasConstraintName("FK_UserDepartmentTransferJunction_DepartmentMaster");

            entity.HasOne(d => d.RequestedSupervisor).WithMany(p => p.UserDepartmentTransferJunctionRequestedSupervisors)
                .HasForeignKey(d => d.RequestedSupervisorId)
                .HasConstraintName("FK_UserDepartmentTransferJunction_UserMaster1");

            entity.HasOne(d => d.User).WithMany(p => p.UserDepartmentTransferJunctionUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserDepartmentTransferJunction_UserMaster");
        });

        modelBuilder.Entity<UserLeavesApplicationMaster>(entity =>
        {
            entity.HasKey(e => e.UserLeaveApplicationId);

            entity.ToTable("UserLeavesApplicationMaster");

            entity.Property(e => e.UserLeaveApplicationId).HasColumnName("UserLeaveApplicationID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.LeaveApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.LeaveApprovalStatus)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UserLoginHistory>(entity =>
        {
            entity.ToTable("UserLoginHistory");

            entity.Property(e => e.UserLoginHistoryId).HasColumnName("UserLoginHistoryID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastBrowserUsed).HasMaxLength(500);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.RefreshtokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLoginHistory_UserMaster");
        });

        modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserMaster");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.ContactNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateofBirth).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(1000);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(500);
            entity.Property(e => e.LoginUserName).HasMaxLength(500);
            entity.Property(e => e.MiddleName).HasMaxLength(500);
            entity.Property(e => e.MobileNumber).HasMaxLength(50);
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PrefixId).HasColumnName("PrefixID");
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
            entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");
            entity.Property(e => e.UserPassword).HasMaxLength(1000);

            entity.HasOne(d => d.Branch).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_UserMaster_BranchMaster");

            entity.HasOne(d => d.Organization).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_UserMaster_OrganizationMaster");

            entity.HasOne(d => d.Prefix).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.PrefixId)
                .HasConstraintName("FK_UserMaster_PrefixMaster");

            entity.HasOne(d => d.Tenant).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK_UserMaster_TenantMaster");

            entity.HasOne(d => d.TimeZone).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.TimeZoneId)
                .HasConstraintName("FK_UserMaster_LookupTimeZone");
        });

        modelBuilder.Entity<UserNotificationJunction>(entity =>
        {
            entity.HasKey(e => e.UserNotificationJunctionId).HasName("PK_UserNotificaionJunction");

            entity.ToTable("UserNotificationJunction");

            entity.Property(e => e.UserNotificationJunctionId).HasColumnName("UserNotificationJunctionID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfReceived).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.ReceivalId).HasColumnName("ReceivalID");

            entity.HasOne(d => d.Notification).WithMany(p => p.UserNotificationJunctions)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK_UserNotificaionJunction_NotificationMaster");

            entity.HasOne(d => d.Receival).WithMany(p => p.UserNotificationJunctions)
                .HasForeignKey(d => d.ReceivalId)
                .HasConstraintName("FK_UserNotificaionJunction_UserMaster");
        });

        modelBuilder.Entity<UserOtpjunction>(entity =>
        {
            entity.HasKey(e => e.UserOtpid);

            entity.ToTable("UserOTPJunction");

            entity.Property(e => e.UserOtpid).HasColumnName("UserOTPID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsFirstTimeOtp).HasColumnName("IsFirstTimeOTP");
            entity.Property(e => e.IsOtpexpired).HasColumnName("IsOTPExpired");
            entity.Property(e => e.Otp)
                .HasMaxLength(200)
                .HasColumnName("OTP");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserOtpjunctions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserOTPJunction_UserMaster");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId);

            entity.ToTable("UserProfile");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.BloodGroupId).HasColumnName("BloodGroupID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.DateOfRelease).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.EmployeeTypeId).HasColumnName("EmployeeTypeID");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LastModifyOn).HasColumnType("datetime");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.ProfilePic).HasMaxLength(500);
            entity.Property(e => e.SkypeId)
                .HasMaxLength(500)
                .HasColumnName("SkypeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.BloodGroupId)
                .HasConstraintName("FK_UserProfile_BloodGroupMaster");

            entity.HasOne(d => d.Department).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_UserProfile_DepartmentMaster");

            entity.HasOne(d => d.Designation).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK_UserProfile_DesignationMaster");

            entity.HasOne(d => d.EmployeeType).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.EmployeeTypeId)
                .HasConstraintName("FK_UserProfile_EmployeeTypeMaster");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserProfile_UserMaster");
        });

        modelBuilder.Entity<UserRoleJunction>(entity =>
        {
            entity.HasKey(e => e.UserRoleId);

            entity.ToTable("UserRoleJunction");

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoleJunctions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoleJunction_RoleMaster");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleJunctions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoleJunction_UserMaster");
        });

        modelBuilder.Entity<WeekDayMaster>(entity =>
        {
            entity.HasKey(e => e.WeekDayId);

            entity.ToTable("WeekDayMaster");

            entity.Property(e => e.WeekDayId).HasColumnName("WeekDayID");
            entity.Property(e => e.DayText).HasMaxLength(10);
        });

        modelBuilder.Entity<WorkModeMaster>(entity =>
        {
            entity.HasKey(e => e.WorkModeId);

            entity.ToTable("WorkModeMaster");

            entity.Property(e => e.WorkModeId).HasColumnName("WorkModeID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((0))");
            entity.Property(e => e.WorkModeName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
