using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class LeaveReportRequestSM : BaseReportFilterSM
    {
        public int ClientEmployeeUserId { get; set; }
        public int LeaveCount { get; set; }
        public LeaveTypeSM LeaveType { get; set; }

    }
}
