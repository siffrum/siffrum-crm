namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class EmployeeCTCReportRequestSM : BaseReportFilterSM
    {
        public int ClientUserId { get; set; }
        public int EmployeeCtcCount { get; set; }
        public bool CurrentlyActive { get; set; }
    }
}
