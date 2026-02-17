using Siffrum.Web.Payroll.DAL.Seeds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Siffrum.Web.Payroll.DomainModels.v1;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.DomainModels.v1.FilesInDb;
using Siffrum.Web.Payroll.DomainModels.v1.General;
using Siffrum.Web.Payroll.DomainModels.v1.License;

namespace Siffrum.Web.Payroll.DAL.Contexts
{
    public class ApiDbContext : EfCoreContextRoot
    {
        public ApiDbContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        #region User Tables

        public DbSet<ApplicationUserDM> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserAddressDM> ApplicationUserAddresss { get; set; }
        public DbSet<ClientUserDM> ClientUsers { get; set; }
        public DbSet<ClientUserAddressDM> ClientUserAddresss { get; set; }
        public DbSet<ClientCompanyDetailDM> ClientCompanyDetails { get; set; }
        public DbSet<ClientCompanyAddressDM> ClientCompanyAddresss { get; set; }

        #endregion User Tables

        #region Log Tables
        public DbSet<ErrorLogRoot> ErrorLogRoots { get; set; }

        #endregion Log Tables

        #region Files Table
        public DbSet<ApplicationFileDM> ApplicationFiles { get; set; }

        #endregion Files Table       

        #region Dummy Tables
        public DbSet<DummySubjectDM> DummySubjects { get; set; }
        public DbSet<DummyTeacherDM> DummyTeachers { get; set; }
        #endregion Dummy Tables

        #region Application Specific Tables

        public DbSet<ClientEmployeeBankDetailDM> ClientEmployeeBankDetails { get; set; }
        public DbSet<DocumentsDM> Documents { get; set; }
        public DbSet<ClientGenericPayrollComponentDM> ClientGenericPayrollComponents { get; set; }
        public DbSet<ClientEmployeeAdditionalReimbursementLogDM> ClientEmployeeAdditionalReimbursementLogs { get; set; }
        public DbSet<ClientEmployeePayrollComponentDM> ClientEmployeePayrollComponents { get; set; }
        public DbSet<ClientEmployeeCTCDetailDM> ClientEmployeeCTCDetails { get; set; }
        public DbSet<ClientEmployeeLeaveDM> ClientEmployeeLeaves { get; set; }
        public DbSet<ClientEmployeeDocumentDM> ClientEmployeeDocuments { get; set; }
        public DbSet<PayrollTransactionDM> PayrollTransactions { get; set; }
        public DbSet<CompanyAccountsTransactionDM> CompanyAccountsTransactions { get; set; }
        public DbSet<CompanyModulesDM> CompanyModules { get; set; }
        public DbSet<ClientEmployeeAttendanceDM> ClientEmployeeAttendances { get; set; }
        public DbSet<ClientCompanyAttendanceShiftDM> ClientCompanyAttendanceShifts { get; set; }
        public DbSet<PermissionDM> Permissions { get; set; }
        public DbSet<ClientThemeDM> ClientThemes { get; set; }
        public DbSet<UserSettingDM> UserSettings { get; set; }
        public DbSet<ClientCompanyDepartmentDM> ClientCompanyDepartments { get; set; }
        public DbSet<ClientCompanyHolidaysDM> ClientCompanyHolidays { get; set; }
        public DbSet<LicenseTypeDM> LicenseTypes { get; set; }
        public DbSet<LicenseTypeDM_PermissionDM> LicenseTypes_Permissions { get; set; }
        //public DbSet<UserInvoiceDM> UserInvoices { get; set; }
        public DbSet<CompanyInvoiceDM> CompanyInvoices { get; set; }
        //public DbSet<UserLicenseDetailsDM> UserLicenseDetails { get; set; }
        public DbSet<CompanyLicenseDetailsDM> CompanyLicenseDetails { get; set; }
        public DbSet<SQLReportMasterDM> SQLReportMasters { get; set; }
        public DbSet<ContactUsDM> ContactUs { get; set; }

        #endregion Application Specific Tables


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //https://www.entityframeworktutorial.net/efcore/configure-one-to-many-relationship-using-fluent-api-in-ef-core.aspx
            //https://www.entityframeworktutorial.net/code-first/configure-many-to-many-relationship-in-code-first.aspx

            base.OnModelCreating(builder);
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<DummyTeacherDM>()
                .HasOne<ApplicationFileDM>()
                .WithMany()
                .HasForeignKey(s => s.ProfilePictureFileId);
            //without property as below
            //.HasForeignKey("TestDisplayFileId");

            builder.Entity<DummySubjectDM>()
                .HasMany<ApplicationFileDM>()
            .WithMany();

            //builder.Entity<PayrollTransactionDM>()
            //    .HasOne(e => e.ClientUser)
            //    .WithMany()
            //.OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PayrollTransactionDM>()
                .HasOne(e => e.ClientEmployeeCTCDetail)
                .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClientEmployeeLeaveDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClientUserAddressDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClientEmployeeBankDetailDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClientEmployeeDocumentDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PayrollTransactionDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<ClientEmployeeCTCDetailDM>()
            //    .HasOne(e => e.ClientCompanyDetail)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClientEmployeeAdditionalReimbursementLogDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClientEmployeeAttendanceDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PermissionDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<PermissionDM>()
             .HasOne(e => e.CompanyModules)
             .WithMany()
             .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ClientCompanyAttendanceShiftDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ClientCompanyDepartmentDM>()
                .HasOne(e => e.ClientCompanyDetail)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ClientUserDM>()
                .HasIndex(u => u.EmployeeCode)
            .IsUnique();

            builder.Entity<ClientUserDM>()
                .HasOne(e => e.ClientCompanyAttendanceShift)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ClientUserDM>()
                .HasOne(e => e.ClientCompanyDepartment)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);


            //foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            builder.Entity<LicenseTypeDM_PermissionDM>()
          .HasKey(bc => new { bc.PermissionId, bc.LicenseTypeId });

            builder.Entity<LicenseTypeDM_PermissionDM>()
                .HasOne(f => f.Permission)
                .WithMany(b => b.LicenseTypeDM_PermissionDMs)
                .HasForeignKey(fk => fk.PermissionId);

            builder.Entity<LicenseTypeDM_PermissionDM>()
                .HasOne(lt => lt.LicenseType)
                .WithMany(b => b.LicenseTypeDM_PermissionDMs)
                .HasForeignKey(fk => fk.LicenseTypeId);

            builder.Entity<CompanyInvoiceDM>()
         .HasKey(m => new { m.StripeInvoiceId, m.Id });

            // Ensure DateTime properties are consistently treated as UTC when stored/retrieved to avoid Npgsql Unspecified DateTime errors for timestamptz
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)) : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var properties = clrType.GetProperties().Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));
                foreach (var prop in properties)
                {
                    if (prop.PropertyType == typeof(DateTime))
                        builder.Entity(clrType).Property(prop.Name).HasConversion(dateTimeConverter);
                    else
                        builder.Entity(clrType).Property(prop.Name).HasConversion(nullableDateTimeConverter);
                }
            }

            DatabaseSeeder<ApiDbContext> seeder = new DatabaseSeeder<ApiDbContext>();
            seeder.SetupDatabaseWithSeedData(builder);
        }
    }
}
