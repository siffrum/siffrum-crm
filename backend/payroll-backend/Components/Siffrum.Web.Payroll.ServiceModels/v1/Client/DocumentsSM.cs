using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class DocumentsSM : SiffrumPayrollServiceModelBase<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LetterData { get; set; }
        public string Extension { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
