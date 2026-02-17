namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class EmployeeAdditionalReimbursementLogReportRequestSM : BaseReportFilterSM
    {
        public int ClientUserId { get; set; }
        public int ReimbursementCount { get; set; }
    }
}
