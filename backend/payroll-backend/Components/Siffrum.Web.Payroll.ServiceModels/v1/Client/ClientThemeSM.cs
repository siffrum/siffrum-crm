namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientThemeSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public string Name { get; set; }
        public string Css { get; set; }
        public bool? IsSelected { get; set; }
    }
}
