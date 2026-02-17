using BrainGateway.Authentication.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siffrum.Web.Payroll.BAL.Token;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Token;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers.Login;
using System.Security.Claims;

namespace Siffrum.Web.Payroll.API.Controllers.Token
{
    [Route("api/[controller]")]
    public partial class TokenController : ApiControllerRoot
    {
        private readonly TokenProcess _tokenProcess;
        private readonly JwtHandler _jwtHandler;
        private readonly APIConfiguration _apiConfiguration;
        public TokenController(TokenProcess TokenProcess, JwtHandler jwtHandler, APIConfiguration aPIConfiguration)
        {
            _tokenProcess = TokenProcess;
            _jwtHandler = jwtHandler;
            _apiConfiguration = aPIConfiguration;
        }
        [HttpPost]
        [Route("ValidateLoginAndGenerateToken")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<TokenResponseSM>>> ValidateLoginAndGenerateToken(ApiRequest<TokenRequestSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_Log));
            }

            if (string.IsNullOrWhiteSpace(innerReq.LoginId) || string.IsNullOrWhiteSpace(innerReq.Password) || innerReq.RoleType == RoleTypeSM.Unknown)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessages.Display_InvalidRequiredDataInputs));
            }

            #endregion Check Request

            (LoginUserSM userSM, int compId) = await _tokenProcess.ValidateLoginAndGenerateToken(innerReq);
            if (userSM == null)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessages.Display_UserNotFound,
                    ApiErrorTypeSM.InvalidInputData_Log));
            }
            else if (userSM.LoginStatus == LoginStatusSM.Disabled)
            {
                return Unauthorized(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessages.Display_UserDisabled, ApiErrorTypeSM.Access_Denied_Log));
            }
            //else if (userSM.LoginStatus == LoginStatusSM.PasswordResetRequired)
            //{
            //    return Unauthorized(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessages.Display_UserPasswordResetRequired, ApiErrorTypeSM.Access_Denied_Log));
            //}
            else if (!userSM.IsEmailConfirmed || !userSM.IsPhoneNumberConfirmed)
            {
                return Unauthorized(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessages.Display_UserNotVerified, ApiErrorTypeSM.Access_Denied_Log));
            }
            else
            {
                ICollection<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,innerReq.LoginId),
                    new Claim(ClaimTypes.Role,innerReq.RoleType.ToString()),
                    new Claim(ClaimTypes.GivenName,userSM.FirstName + " " + userSM.MiddleName + " " +userSM.LastName ),
                    new Claim(ClaimTypes.Email,userSM.EmailId),
                    new Claim(DomainConstants.ClaimsRoot.Claim_DbRecordId,userSM.Id.ToString())
                };
                if (compId != default)
                {
                    claims.Add(new Claim(DomainConstants.ClaimsRoot.Claim_ClientCode, innerReq.CompanyCode));
                    claims.Add(new Claim(DomainConstants.ClaimsRoot.Claim_ClientId, compId.ToString()));
                }
                var expiryDate = DateTime.Now.AddDays(_apiConfiguration.DefaultTokenValidityDays);
                var token = await _jwtHandler.ProtectAsync(_apiConfiguration.JwtTokenSigningKey, claims, new DateTimeOffset(DateTime.Now), new DateTimeOffset(expiryDate), "SiffrumPayroll");
                // here if user is derived class, all properties will be sent
                var tokenResponse = new TokenResponseSM()
                {
                    AccessToken = token,
                    LoginUserDetails = userSM,
                    ExpiresUtc = expiryDate,
                    ClientCompantId = compId
                };
                return Ok(ModelConverter.FormNewSuccessResponse(tokenResponse));
            }
        }

    }
}
