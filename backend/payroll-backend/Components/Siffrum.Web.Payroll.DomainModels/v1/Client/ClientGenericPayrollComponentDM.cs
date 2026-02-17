namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientGenericPayrollComponentDM : BaseGenericPayrollComponentDM
    {
        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }

        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
