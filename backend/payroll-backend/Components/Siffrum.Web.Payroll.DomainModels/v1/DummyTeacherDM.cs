using Siffrum.Web.Payroll.DomainModels.Base;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1
{
    public class DummyTeacherDM : SiffrumPayrollDomainModelBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        public int? ProfilePictureFileId { get; set; }
        public virtual HashSet<DummySubjectDM> DummySubjects { get; set; }
    }
}
