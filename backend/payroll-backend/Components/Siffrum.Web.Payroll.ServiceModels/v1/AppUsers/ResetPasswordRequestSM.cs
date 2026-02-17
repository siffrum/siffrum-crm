using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class ResetPasswordRequestSM : SiffrumPayrollServiceModelBase<int>
    {
        public string NewPassword { get; set; }
        public string authCode { get; set; }
    }
}
