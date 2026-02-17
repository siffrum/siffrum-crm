using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeLeaveSM : SiffrumPayrollServiceModelBase<int>
    {
        public int ClientUserId { get; set; }
        public int ClientCompanyDetailId { get; set; }
        public LeaveTypeSM LeaveType { get; set; }
        public string EmployeeComment { get; set; }
        public bool? IsApproved { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovalComment { get; set; }
        public DateTime LeaveDateFromUTC { get; set; }
        public DateTime LeaveDateToUTC { get; set; }
    }
}
