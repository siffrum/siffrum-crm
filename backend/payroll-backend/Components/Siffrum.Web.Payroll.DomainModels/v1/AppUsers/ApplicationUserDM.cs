
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers.Login;

namespace Siffrum.Web.Payroll.DomainModels.v1.AppUsers
{
    [Index(nameof(LoginId), IsUnique = true)]
    [Index(nameof(EmailId), IsUnique = true)]
    public class ApplicationUserDM : LoginUserDM
    {
        public ApplicationUserDM()
        {
        }

        [ForeignKey(nameof(ApplicationUserAddress))]
        public int? ApplicationUserAddressId { get; set; }
        public virtual ApplicationUserAddressDM? ApplicationUserAddress { get; set; }
    }
}
