using Siffrum.Web.Payroll.DomainModels.Base;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class DocumentsDM : SiffrumPayrollDomainModelBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] LetterData { get; set; }

        public string Extension { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
