using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    // [IgnoreAutoMapAttribute]
    public class ClientEmployeeBankDetailDM : SiffrumPayrollDomainModelBase<int>
    {
        [Required]
        [StringLength(20)]
        public string BankName { get; set; }

        [Required]
        [StringLength(20)]
        public string Branch { get; set; }

        [Required]
        [MaxLength(16)]
        public long AccountNo { get; set; }

        [Required]
        [StringLength(11)]
        public string IfscCode { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }

        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }
    }
}
