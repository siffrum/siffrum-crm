using Siffrum.Web.Payroll.DomainModels.Base;

namespace Siffrum.Web.Payroll.DomainModels.v1.License
{
    public class CompanyInvoiceDM : SiffrumPayrollDomainModelBase<int>
    {
        [Required]
        [Key]
        public string StripeInvoiceId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDateUTC { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpiryDateUTC { get; set; }
        public double DiscountInPercentage { get; set; }
        public decimal ActualPaidPrice { get; set; }
        public long AmountDue { get; set; }
        public long AmountRemaining { get; set; }
        public string Currency { get; set; }
        [StringLength(50)]
        public string StripeCustomerId { get; set; }

        [ForeignKey(nameof(CompanyLicenseDetails))]
        public int CompanyLicenseDetailsId { get; set; }
        public virtual CompanyLicenseDetailsDM CompanyLicenseDetails { get; set; }
    }
}
