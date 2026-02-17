using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1.License
{
    public class CompanyLicenseDetailsDM : SiffrumPayrollDomainModelBase<int>
    {
        [StringLength(100), DefaultValue("")]
        public string? StripeCustomerId { get; set; }
        [StringLength(100), DefaultValue("")]
        public string? StripeSubscriptionId { get; set; }
        [StringLength(100), DefaultValue("")]
        public string? StripePriceId { get; set; }
        [StringLength(50), DefaultValue("")]
        public string? SubscriptionPlanName { get; set; }
        [StringLength(100), DefaultValue("")]
        public int ValidityInDays { get; set; }
        public double DiscountInPercentage { get; set; }
        public decimal ActualPaidPrice { get; set; }
        [StringLength(10), DefaultValue("")]
        public string? Currency { get; set; }
        [StringLength(10), DefaultValue("")]
        public string? Status { get; set; }
        [DefaultValue(true)]
        public bool IsSuspended { get; set; }
        [DefaultValue(true)]
        public bool IsCancelled { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CancelAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CancelledOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartDateUTC { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ExpiryDateUTC { get; set; }

        [ForeignKey(nameof(LicenseType))]
        public int LicenseTypeId { get; set; }
        public virtual LicenseTypeDM LicenseType { get; set; }


        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        public virtual ICollection<CompanyInvoiceDM> CompanyInvoices { get; set; }
    }
}
