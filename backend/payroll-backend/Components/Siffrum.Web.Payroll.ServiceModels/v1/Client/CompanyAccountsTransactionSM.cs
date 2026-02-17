using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class CompanyAccountsTransactionSM : SiffrumPayrollServiceModelBase<int>
    {
        public string ExpenseName { get; set; }
        public string ExpensePurpose { get; set; }
        public float ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CurrencyCode { get; set; }
        public ExpenseModeSM ExpenseMode { get; set; }
        public bool ExpensePaid { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
