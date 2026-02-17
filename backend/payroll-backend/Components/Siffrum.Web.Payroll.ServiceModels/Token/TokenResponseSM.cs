using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;

namespace Siffrum.Web.Payroll.ServiceModels.Token
{
    public class TokenResponseSM : TokenResponseRoot
    {
        public LoginUserSM LoginUserDetails { get; set; }
        public int ClientCompantId { get; set; }
    }
}
