namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientCompanyHolidaysSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
