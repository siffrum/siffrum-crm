using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class CompanyAccountsTransactionDM : SiffrumPayrollDomainModelBase<int>
    {
        public CompanyAccountsTransactionDM()
        {
        }

        [StringLength(50, MinimumLength = 0)]
        public string ExpenseName { get; set; }

        [StringLength(150, MinimumLength = 0)]
        public string ExpensePurpose { get; set; }
        public float ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }

        [Required]
        [MaxLength(4)]
        public string CurrencyCode { get; set; }
        public ExpenseModeDM ExpenseMode { get; set; }

        public bool ExpensePaid { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
