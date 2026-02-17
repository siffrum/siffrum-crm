using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class PayrollTransactionSM : SiffrumPayrollServiceModelBase<int>
    {
        public float PaymentAmount { get; set; }
        public string PaymentFor { get; set; }
        public PaymentModeSM PaymentMode { get; set; }
        public PaymentTypeSM PaymentType { get; set; }
        public float ToPay { get; set; }
        public float ToPaid { get; set; }
        public bool PaymentPaid { get; set; }
        public int ClientUserId { get; set; }
        public int ClientEmployeeCTCDetailId { get; set; }
        public int ClientCompanyDetailId { get; set; }
        public bool ErrorInGeneration { get; set; }
        public string ErrorMessage { get; set; }

    }
}
