using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class PayrollTransactionDM : SiffrumPayrollDomainModelBase<int>
    {
        public float PaymentAmount { get; set; }
        public string PaymentFor { get; set; }
        public PaymentModeDM PaymentMode { get; set; }
        public PaymentTypeDM PaymentType { get; set; }
        public bool PaymentPaid { get; set; }
        public float ToPay { get; set; }
        public float ToPaid { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }

        [ForeignKey(nameof(ClientEmployeeCTCDetail))]
        public int ClientEmployeeCTCDetailId { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }

        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
        public virtual ClientEmployeeCTCDetailDM ClientEmployeeCTCDetail { get; set; }

    }
}
