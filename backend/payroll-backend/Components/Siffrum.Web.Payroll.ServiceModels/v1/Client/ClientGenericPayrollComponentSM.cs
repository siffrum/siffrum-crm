using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientGenericPayrollComponentSM : SiffrumPayrollServiceModelBase<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float Percentage { get; set; }

        public ComponentCalculationTypeSM ComponentCalculationType { get; set; }

        public ComponentPeriodTypeSM ComponentPeriodType { get; set; }

        public int ClientCompanyDetailId { get; set; }
    }
}
