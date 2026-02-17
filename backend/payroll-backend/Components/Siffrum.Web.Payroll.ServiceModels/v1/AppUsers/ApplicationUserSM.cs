using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;

namespace Siffrum.Web.Payroll.ServiceModels.v1.AppUsers
{
    public class ApplicationUserSM : LoginUserSM
    {
        public ApplicationUserSM()
        {
        }

        public int? ApplicationUserAddressId { get; set; }
    }
}
