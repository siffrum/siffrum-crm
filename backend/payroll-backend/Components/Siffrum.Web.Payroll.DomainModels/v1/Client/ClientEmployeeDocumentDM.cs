using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientEmployeeDocumentDM : SiffrumPayrollDomainModelBase<int>
    {
        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        [Required(ErrorMessage = "Please Enter Document Name")]
        public string Name { get; set; }

        public EmployeeDocumentTypeDM EmployeeDocumentType { get; set; }


        [DisplayName("Document Description")]
        [MaxLength(100)]
        public string DocumentDescription { get; set; }

        [DisplayName("Document Extension")]
        public string Extension { get; set; }

        public string EmployeeDocumentPath { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }
    }
}
