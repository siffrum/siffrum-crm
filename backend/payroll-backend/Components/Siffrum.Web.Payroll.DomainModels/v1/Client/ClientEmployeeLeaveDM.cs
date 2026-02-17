using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientEmployeeLeaveDM : SiffrumPayrollDomainModelBase<int>
    {
        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public LeaveTypeDM LeaveType { get; set; }

        [DisplayName("Enter Comment: ")]
        [StringLength(100, MinimumLength = 3)]
        public string EmployeeComment { get; set; }

        [DisplayName("Select Approved: ")]
        public bool? IsApproved { get; set; }

        [DisplayName("Enter UserName: ")]
        public string ApprovedByUserName { get; set; }

        [DisplayName("Enter Approval Comment: ")]
        public string ApprovalComment { get; set; }

        [Required(ErrorMessage = "Date is Required")]
        [DisplayName("Enter Leave Date To:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LeaveDateFromUTC { get; set; }

        [Required(ErrorMessage = "Date is Required")]
        [DisplayName("Enter Leave Date To:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LeaveDateToUTC { get; set; }

        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        public virtual ClientUserDM ClientUser { get; set; }
    }
}
