using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class BaseReportFilterSM : SiffrumPayrollServiceModelBase<int>
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string SearchString { get; set; }
        public DateFilterTypeSM DateFilterType { get; set; }
    }
}
