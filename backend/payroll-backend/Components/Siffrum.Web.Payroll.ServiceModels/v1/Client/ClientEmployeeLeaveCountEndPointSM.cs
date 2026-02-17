using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeLeaveCountEndPointSM : SiffrumPayrollServiceModelBase<int>
    {
        public int AllEmployeeLeaveCount { get; set; }
        public int ApprovedLeaveCount { get; set; }
        public int RejectedLeaveCount { get; set; }
        public int ApprovedEmployeeLeaveCount { get; set; }
        public int LeaveReportCount { get; set; }
    }
}
