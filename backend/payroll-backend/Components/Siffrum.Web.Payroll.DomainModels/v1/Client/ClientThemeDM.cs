namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientThemeDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        public string Name { get; set; }
        public string Css { get; set; }
        public bool? IsSelected { get; set; }
    }
}
