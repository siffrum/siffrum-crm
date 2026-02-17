using Siffrum.Web.Payroll.Client.ApiClients;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Xunit;

namespace Siffrum.Web.Payroll.Client.Tests
{
    public class ClientUserClientTests
    {
        private readonly ClientUserClient _targetClient;
        private readonly AuthClientWrapper _authClientWrapper;

        public ClientUserClientTests()
        {
            _authClientWrapper = new AuthClientWrapper() { AuthDetails = TestHelper.GetAuthClientDetails(), LoggedInUserToken = "" };
            _targetClient = new ClientUserClient(TestHelper.GetAccessingClientDetails(), TestHelper.GetExceptionFunc());
        }

        [Fact]
        public List<ClientUserSM> GetAllClientUsersOdataTests()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var odataFilter = new OdataQueryFilter() { Skip = 1, Top = 5, OrderByCommand = "FirstName,Id desc", FilterByCommand = "Id gt 1" };
            ApiResponse<List<ClientUserSM>> resp = _targetClient.GetServiceModelByOdata<ClientUserSM>(_authClientWrapper, odataFilter, cancellationTokenSource.Token)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(resp);
            Assert.True(resp.SuccessData.Count == 1);
            Assert.False(resp.IsError);
            Assert.NotNull(resp.SuccessData);
            return resp.SuccessData;
        }

        [Fact]
        public List<ClientUserSM> GetAllClientUsersTests()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ApiResponse<List<ClientUserSM>> resp = _targetClient.GetAllClientUsers(_authClientWrapper, cancellationTokenSource.Token)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(resp);
            Assert.False(resp.IsError);
            Assert.NotNull(resp.SuccessData);
            return resp.SuccessData;
        }

        [Fact]
        public ClientUserSM GetAllClientUsersByIdTests()
        {
            int idToTest = 1;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ApiResponse<ClientUserSM> respEntity = _targetClient.GetClientUserById(_authClientWrapper, cancellationTokenSource.Token, idToTest)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(respEntity);
            Assert.False(respEntity.IsError);
            Assert.NotNull(respEntity.SuccessData);
            Assert.Equal(idToTest, respEntity.SuccessData.Id);
            return respEntity.SuccessData;
        }

        //[Fact]
        //public void AddUpdateDeleteClientUsersTests()
        //{
        //    var addResult = AddClientUser();
        //    Assert.NotNull(addResult);
        //    Assert.False(addResult.IsError);
        //    Assert.NotNull(addResult.SuccessData);
        //    Assert.True(addResult.SuccessData.Id > 0);

        //    var updateResult = UpdateClientUser(addResult.SuccessData.Id);
        //    Assert.NotNull(updateResult);
        //    Assert.False(updateResult.IsError);
        //    Assert.NotNull(updateResult.SuccessData);
        //    Assert.True(updateResult.SuccessData.Id > 0);
        //    Assert.Equal(addResult.SuccessData.Id, updateResult.SuccessData.Id);
        //    Assert.True(updateResult.SuccessData.SubjectName.StartsWith("Updated"));

        //    var deleteResult = DeleteClientUser(addResult.SuccessData.Id);
        //    Assert.NotNull(deleteResult);
        //    Assert.False(deleteResult.IsError);
        //    Assert.NotNull(deleteResult.SuccessData);
        //    Assert.True(deleteResult.SuccessData.DeleteResult);
        //}

        //private ApiResponse<ClientUserSM> AddClientUser()
        //{
        //    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //    var targetObj = new ClientUserAddressSM()
        //    {
        //        SubjectName = "TC_SubName_" + Random.Shared.NextDouble().ToString(),
        //        SubjectCode = Guid.NewGuid().ToString()
        //    };
        //    ApiResponse<ClientUserSM> respEntity = _targetClient.AddClientUser(_authClientWrapper, cancellationTokenSource.Token, targetObj)
        //        .ConfigureAwait(false).GetAwaiter().GetResult();
        //    return respEntity;
        //}
        //private ApiResponse<ClientUserSM> UpdateClientUser(int targetId)
        //{
        //    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //    var targetObj = new ClientUserSM()
        //    {
        //        SubjectName = "Updated_TC_SubName_" + Random.Shared.NextDouble().ToString(),
        //        SubjectCode = "Updated_" + Guid.NewGuid().ToString()
        //    };
        //    ApiResponse<ClientUserSM> respEntity = _targetClient.UpdateClientUser(_authClientWrapper, cancellationTokenSource.Token, targetId, targetObj)
        //        .ConfigureAwait(false).GetAwaiter().GetResult();
        //    return respEntity;
        //}
        //private ApiResponse<DeleteResponseRoot> DeleteClientUserAddress(int targetId)
        //{
        //    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //    ApiResponse<DeleteResponseRoot> respEntity = _targetClient.DeleteClientUserById(_authClientWrapper, cancellationTokenSource.Token, targetId)
        //        .ConfigureAwait(false).GetAwaiter().GetResult();
        //    return respEntity;
        //}

    }
}
