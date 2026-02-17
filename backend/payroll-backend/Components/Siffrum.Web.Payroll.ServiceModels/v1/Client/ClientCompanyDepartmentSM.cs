namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientCompanyDepartmentSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public string DepartmentName { get; set; }
        public string DepartmenntLocation { get; set; }
        public string DepartmentDescription { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentContact { get; set; }
        public string DepartmentManager { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
