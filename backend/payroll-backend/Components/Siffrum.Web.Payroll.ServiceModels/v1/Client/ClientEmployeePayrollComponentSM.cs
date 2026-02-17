namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeePayrollComponentSM : ClientGenericPayrollComponentSM
    {
        public decimal AmountYearly { get; set; }

        public int? ClientEmployeeCTCDetailId { get; set; }
    }
}
