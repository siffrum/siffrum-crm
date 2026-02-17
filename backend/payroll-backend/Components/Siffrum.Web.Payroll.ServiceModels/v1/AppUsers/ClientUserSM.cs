using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class ClientUserSM : LoginUserSM
    {
        public ClientUserSM()
        {
        }
        public GenderSM Gender { get; set; }
        public string PersonalEmailId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime LastWorkingDay { get; set; }
        public DateTime DateOfResignation { get; set; }
        public string EmployeeCode { get; set; }
        //public int? ClientUserAddressId { get; set; }
        public int ClientCompanyDetailId { get; set; }
        public int? UserSettingId { get; set; }
        public int? ClientCompanyDepartmentId { get; set; }
        public int? ClientCompanyAttendanceShiftId { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string AttendanceShift { get; set; }
        public EmployeeStatusSM EmployeeStatus { get; set; }
        public bool IsPaymentAdmin { get; set; }

    }
}
