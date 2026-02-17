using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientEmployeeAdditionalReimbursementLogDM : SiffrumPayrollDomainModelBase<int>
    {
        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        public ReimbursementTypeDM ReimbursementType { get; set; }

        [DisplayName("Enter Reimbursement DocumentName: ")]
        [StringLength(50, MinimumLength = 0)]
        public string ReimburseDocumentName { get; set; }

        [DisplayName("Document Extension")]
        public string Extension { get; set; }

        [DisplayName("Enter Reimbursement Description: ")]
        [StringLength(100, MinimumLength = 0)]
        public string ReimbursementDescription { get; set; }

        [Required(ErrorMessage = "Reimbursement Amount is Required")]
        [DisplayName("Enter Reimbursement Amount: ")]
        public decimal ReimbursementAmount { get; set; }

        [Required(ErrorMessage = "Date is Required")]
        [DisplayName("Enter Reimbursement Date")]
        [DataType(DataType.DateTime)]
        public DateTime ReimbursementDate { get; set; }

        public string ReimbursementDocumentPath { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }

        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        public virtual ClientUserDM ClientUser { get; set; }
    }
}
