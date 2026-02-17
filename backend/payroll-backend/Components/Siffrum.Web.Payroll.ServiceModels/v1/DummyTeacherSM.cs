using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1
{
    public class DummyTeacherSM : SiffrumPayrollServiceModelBase<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public int? ProfilePictureFileId { get; set; }
    }
}
