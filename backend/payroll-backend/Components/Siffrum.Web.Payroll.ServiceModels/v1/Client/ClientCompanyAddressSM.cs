namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientCompanyAddressSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public ClientCompanyAddressSM()
        {
        }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PinCode { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
