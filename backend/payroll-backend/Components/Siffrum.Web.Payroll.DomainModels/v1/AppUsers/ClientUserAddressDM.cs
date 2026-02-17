using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.Client;

namespace Siffrum.Web.Payroll.DomainModels.v1.AppUsers
{
    public class ClientUserAddressDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        [StringLength(100, MinimumLength = 0)]
        public string Country { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string State { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string City { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Address1 { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 0)]
        [RegularExpression(@"^\d{6}$")]
        public string PinCode { get; set; }

        [Required]
        public ClientUserAddressTypeDM ClientUserAddressType { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }
    }
}
