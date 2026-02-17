namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientCompanyDepartmentDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        public string DepartmentName { get; set; }
        public string DepartmenntLocation { get; set; }
        public string DepartmentDescription { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentContact { get; set; }
        public string DepartmentManager { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int? ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
    }
}
