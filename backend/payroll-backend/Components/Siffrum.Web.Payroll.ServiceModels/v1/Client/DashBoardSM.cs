using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class DashBoardSM
    {
        public int NumberOfEmployees { get; set; }
        public int NumberOfAdmins { get; set; }
        public int NumberOfLeavesApproved { get; set; }
        public int NumberOfLeavesPending { get; set; }
        public int NumberOfLeavesRejected { get; set; }
        public int NumberOfDepartments { get; set; }
        public int NumberOfEmployeesPresent { get; set; }
        public int NumberOfEmployeesAbsent { get; set; }
        public int NumberOfEmployeeOnLeave { get; set; }
        public List<ClientCompanyDepartmentReportSM> ClientCompanyDepartment { get; set; }
    }
}
