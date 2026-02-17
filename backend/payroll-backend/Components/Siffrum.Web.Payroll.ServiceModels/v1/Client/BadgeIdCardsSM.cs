using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class BadgeIdCardsSM : SiffrumPayrollServiceModelBase<int>
    {
        public string EmployeeName { get; set; }
        public string EmployeeMail { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeDesignation { get; set; }
        public string ProfilePicture { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyWebsite { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
