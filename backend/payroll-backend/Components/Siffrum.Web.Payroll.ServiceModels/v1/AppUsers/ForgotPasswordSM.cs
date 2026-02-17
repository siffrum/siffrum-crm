using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class ForgotPasswordSM : SiffrumPayrollServiceModelBase<int>
    {
        public string CompanyCode { get; set; }
        public string UserName { get; set; }
        public DateTime Expiry { get; set; }
    }
}
