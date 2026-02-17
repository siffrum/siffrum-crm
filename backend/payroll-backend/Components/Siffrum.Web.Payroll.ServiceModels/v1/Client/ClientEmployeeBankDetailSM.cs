using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeBankDetailSM : SiffrumPayrollServiceModelBase<int>
    {
        public string BankName { get; set; }

        public string Branch { get; set; }

        public long AccountNo { get; set; }

        public string IfscCode { get; set; }

        public int ClientUserId { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
