using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers.Login;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1.AppUsers
{
    [Index(nameof(LoginId), IsUnique = true)]
    [Index(nameof(EmailId), IsUnique = true)]
    public class ClientUserDM : LoginUserDM
    {
        public ClientUserDM()
        {
        }
        public GenderDM Gender { get; set; }

        public EmployeeStatusDM EmployeeStatus { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string PersonalEmailId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfJoining { get; set; }

        public bool IsPaymentAdmin { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastWorkingDay { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfResignation { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string EmployeeCode { get; set; }

        [StringLength(50)]
        public string Designation { get; set; }

        //[ForeignKey(nameof(ClientUserAddress))]
        //public int? ClientUserAddressId { get; set; }
        public virtual HashSet<ClientUserAddressDM> ClientUserAddress { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        [ForeignKey(nameof(ClientCompanyAttendanceShift))]
        public int? ClientCompanyAttendanceShiftId { get; set; }
        public virtual ClientCompanyAttendanceShiftDM ClientCompanyAttendanceShift { get; set; }

        [ForeignKey(nameof(UserSetting))]
        public int? UserSettingId { get; set; }
        public virtual UserSettingDM UserSetting { get; set; }

        [ForeignKey(nameof(ClientCompanyDepartment))]
        public int? ClientCompanyDepartmentId { get; set; }
        public virtual ClientCompanyDepartmentDM ClientCompanyDepartment { get; set; }

        public virtual HashSet<ClientEmployeeBankDetailDM> ClientEmployeeBankDetail { get; set; }

        public virtual HashSet<ClientEmployeeLeaveDM> ClientEmployeeLeave { get; set; }

        public virtual HashSet<ClientEmployeeDocumentDM> ClientEmployeeDocument { get; set; }
        public virtual HashSet<ClientEmployeeCTCDetailDM> ClientEmployeeCTCDetail { get; set; }

        public virtual HashSet<PayrollTransactionDM> PayrollTransaction { get; set; }

    }
}
