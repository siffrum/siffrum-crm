using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class GeneratePayrollTransactionSM : PayrollTransactionSM
    {
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public EmployeeStatusSM EmployeeStatus { get; set; }
    }
}
