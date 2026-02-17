using Siffrum.Web.Payroll.Client.ApiClients;
using Siffrum.Web.Payroll.ServiceModels.Token;
using Xunit;

namespace Siffrum.Web.Payroll.Client.Tests
{
    public class TokenClientTests
    {
        private readonly AccountsClient _targetClient;
        private readonly AuthClientWrapper _authClientWrapper;
        public TokenClientTests()
        {
            _authClientWrapper = new AuthClientWrapper() { AuthDetails = TestHelper.GetAuthClientDetails(), LoggedInUserToken = "" };
            _targetClient = new AccountsClient(TestHelper.GetAccessingClientDetails(), TestHelper.GetExceptionFunc());
        }
        [Fact]
        public TokenResponseSM GenerateTokenTests()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            TokenRequestSM tokenRequest = new TokenRequestSM()
            {
                LoginId = TestHelper.USERNAME,
                Password = TestHelper.PASSWORD,
                CompanyCode = TestHelper.COMP_CODE,
                RoleType = TestHelper.USERTYPE
            };
            ApiResponse<TokenResponseSM> resp = _targetClient.GenerateTokenAsync(tokenRequest, cancellationTokenSource.Token)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(resp);
            Assert.False(resp.IsError);
            Assert.NotNull(resp.SuccessData);
            return resp.SuccessData;
        }
    }
}
