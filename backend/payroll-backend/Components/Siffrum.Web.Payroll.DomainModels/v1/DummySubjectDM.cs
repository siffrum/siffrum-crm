using Siffrum.Web.Payroll.DomainModels.Base;


namespace Siffrum.Web.Payroll.DomainModels.v1
{
    // [IgnoreAutoMapAttribute]
    public class DummySubjectDM : SiffrumPayrollDomainModelBase<int>
    {
        [StringLength(100)]
        public string SubjectName { get; set; }
        public string? SubjectCode { get; set; }

        [ForeignKey(nameof(DummyTeacherDM))]
        public int? DummyTeacherID { get; set; }
        public virtual DummyTeacherDM? DummyTeacher { get; set; }
    }
}
