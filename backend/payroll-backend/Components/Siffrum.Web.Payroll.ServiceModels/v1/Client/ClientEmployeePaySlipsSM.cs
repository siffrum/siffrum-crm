using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeePaySlipsSM : ClientCompanyDetailSM
    {
        public string EmployeeName { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string Designation { get; set; }
        public string PaymentFor { get; set; }
        public float PaymentAmount { get; set; }
        public bool PaymentPaid { get; set; }
        public string EmployeeCode { get; set; }
        public float CtcAmount { get; set; }
        public string CompanyAddress { get; set; }
        public List<ClientEmployeePayrollComponentSM> ClientEmployeePayrollComponents { get; set; }

    }
}
