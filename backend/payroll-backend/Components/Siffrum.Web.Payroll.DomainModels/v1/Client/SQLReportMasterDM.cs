using Siffrum.Web.Payroll.DomainModels.Base;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class SQLReportMasterDM : SiffrumPayrollDomainModelBase<int>
    {
        public SQLReportMasterDM()
        {
        }
        public string ReportName { get; set; }
        public string SQLQuery { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int? ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
