using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class BaseGenericPayrollComponentDM : SiffrumPayrollDomainModelBase<int>
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [MaxLength(15)]
        public float Percentage { get; set; }

        [Required]
        public ComponentCalculationTypeDM ComponentCalculationType { get; set; }

        [Required]
        public ComponentPeriodTypeDM ComponentPeriodType { get; set; }
    }
}
