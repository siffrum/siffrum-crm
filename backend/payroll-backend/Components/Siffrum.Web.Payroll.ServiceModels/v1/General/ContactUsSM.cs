namespace Siffrum.Web.Payroll.ServiceModels.v1.General
{
    public class ContactUsSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
    }
}
