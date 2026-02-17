using Siffrum.Web.Payroll.ServiceModels.Base;

namespace Siffrum.Web.Payroll.ServiceModels.v1
{
    //[IgnoreClassAutoMap]
    public class DummySubjectSM : SiffrumPayrollServiceModelBase<int>
    {
        public string SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public int? DummyTeacherID { get; set; }
    }
}
