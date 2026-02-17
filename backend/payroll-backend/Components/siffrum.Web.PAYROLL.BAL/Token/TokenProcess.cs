using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Token;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;

namespace Siffrum.Web.Payroll.BAL.Token
{
    public partial class TokenProcess : SiffrumPayrollBalBase
    {
        private readonly IPasswordEncryptHelper _passwordEncryptHelper;

        public TokenProcess(IMapper mapper, ApiDbContext context, IPasswordEncryptHelper passwordEncryptHelper) : base(mapper, context)
        {
            _passwordEncryptHelper = passwordEncryptHelper;
        }
        #region Token
        public async Task<(LoginUserSM, int)> ValidateLoginAndGenerateToken(TokenRequestSM tokenReq)
        {
            LoginUserSM? loginUserSM = null;
            int compId = default;
            // add hash
            var passwordHash = await _passwordEncryptHelper.ProtectAsync<string>(tokenReq.Password);
            switch (tokenReq.RoleType)
            {
                case RoleTypeSM.SuperAdmin:
                case RoleTypeSM.SystemAdmin:
                    var appUser = await _apiDbContext.ApplicationUsers
                        .FirstOrDefaultAsync(x => x.LoginId == tokenReq.LoginId && x.PasswordHash == passwordHash && x.RoleType == (RoleTypeDM)tokenReq.RoleType);
                    if (appUser != null)
                    { loginUserSM = _mapper.Map<ApplicationUserSM>(appUser); }

                    break;
                case RoleTypeSM.ClientAdmin:
                case RoleTypeSM.ClientEmployee:
                    {
                        var data = await (from comp in _apiDbContext.ClientCompanyDetails
                                          join user in _apiDbContext.ClientUsers
                                          on comp.Id equals user.ClientCompanyDetailId
                                          where user.LoginId == tokenReq.LoginId && user.PasswordHash == passwordHash
                                          && comp.CompanyCode == tokenReq.CompanyCode && user.RoleType == (RoleTypeDM)tokenReq.RoleType
                                          select new { User = user, CompId = comp.Id }).FirstOrDefaultAsync();
                        if (data != null && data.User != null)
                        {
                            loginUserSM = _mapper.Map<ClientUserSM>(data.User);
                            compId = data.CompId;
                        }
                    }
                    break;
            }

            return (loginUserSM, compId);
        }
        #endregion Token
    }
}
