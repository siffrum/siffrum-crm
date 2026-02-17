using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.Token;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class AccountsClient : SiffrumPayrollApiClientBase
    {
        public AccountsClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null)
            : base(accessingClientDetails, onExceptionInClient)
        {
        }
        public async Task<ApiResponse<TokenResponseSM>> GenerateTokenAsync(TokenRequestSM tokenRequest, CancellationToken cancelToken)
        {
            if (tokenRequest == null)
            {
                throw new ApiExceptionRoot(ApiErrorTypeSM.InvalidInputData_Log, "TokenRequest is null", DomainConstants.DisplayMessages.Display_GlobalErrorClient);
            }

            var ApiRequest = new ApiRequest<TokenRequestSM>() { ReqData = tokenRequest };
            var tokenResp = await GetResponseEntityAsync<TokenRequestSM, TokenResponseSM>(ApiUrls.TOKEN_URL, HttpMethod.Post,
                ApiRequest, cancelToken, null, false);
            return tokenResp;
        }
    }
}
