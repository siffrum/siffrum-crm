using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class SQLReportMasterSM : SiffrumPayrollServiceModelBase<int>
    {
        public SQLReportMasterSM()
        {
        }
        public string ReportName { get; set; }
        public string SqlQuery { get; set; }
        public int? ClientCompanyDetailId { get; set; }
    }
}
