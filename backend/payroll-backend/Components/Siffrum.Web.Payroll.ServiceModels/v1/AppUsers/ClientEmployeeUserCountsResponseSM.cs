using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class ClientEmployeeUserCountsResponseSM : SiffrumPayrollServiceModelBase<int>
    {
        public int AllEmployeeCount { get; set; }
        public int AdminEmployeeCount { get; set; }
        public int SimpleEmployeeCount { get; set; }
        public int ActiveEmployeeCount { get; set; }
        public int ResignedEmployeeCount { get; set; }
        public int SuspendedEmployeeUserCount { get; set; }
    }
}
