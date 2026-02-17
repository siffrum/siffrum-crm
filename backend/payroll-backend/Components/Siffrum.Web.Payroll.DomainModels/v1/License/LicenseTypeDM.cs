using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1.License
{
    public class LicenseTypeDM : SiffrumPayrollDomainModelBase<int>
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int ValidityInDays { get; set; }

        [StringLength(150)]
        public string LicenseTypeCode { get; set; }

        [Required]
        [StringLength(150)]
        public string StripePriceId { get; set; }

        [Required]
        public RoleTypeDM ValidFor { get; set; }

        public double Amount { get; set; }
        public bool IsPreDefined { get; set; }
        public virtual ICollection<LicenseTypeDM_PermissionDM> LicenseTypeDM_PermissionDMs { get; set; }
    }
}
