namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientCompanyHolidaysDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
