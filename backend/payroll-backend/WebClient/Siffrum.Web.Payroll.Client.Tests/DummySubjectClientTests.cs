using Reno.Web.Foundation.ServiceModels.CommonResponse;
using Siffrum.Web.Payroll.Client.ApiClients;
using Siffrum.Web.Payroll.ServiceModels.v1;
using Xunit;

namespace Siffrum.Web.Payroll.Client.Tests
{
    public class DummySubjectClientTests
    {
        private readonly DummySubjectClient _targetClient;
        private readonly AuthClientWrapper _authClientWrapper;
        public DummySubjectClientTests()
        {
            _authClientWrapper = new AuthClientWrapper() { AuthDetails = TestHelper.GetAuthClientDetails(), LoggedInUserToken = "" };
            _targetClient = new DummySubjectClient(TestHelper.GetAccessingClientDetails(), TestHelper.GetExceptionFunc());
        }

        [Fact]
        public List<DummySubjectSM> GetAllDummySubjectsOdataTests()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var odataFilter = new OdataQueryFilter() { Skip = 1, Top = 5, OrderByCommand = "SubjectName,Id desc", FilterByCommand = "Id gt 1" };
            ApiResponse<List<DummySubjectSM>> respEntity = _targetClient.GetServiceModelByOdata<DummySubjectSM>(_authClientWrapper, odataFilter, cancellationTokenSource.Token)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(respEntity);
            Assert.True(respEntity.SuccessData.Count == 1);
            Assert.False(respEntity.IsError);
            Assert.NotNull(respEntity.SuccessData);
            return respEntity.SuccessData;
        }

        [Fact]
        public List<DummySubjectSM> GetAllDummySubjectsTests()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ApiResponse<List<DummySubjectSM>> respEntity = _targetClient.GetAllDummySubjects(_authClientWrapper, cancellationTokenSource.Token)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(respEntity);
            Assert.False(respEntity.IsError);
            Assert.NotNull(respEntity.SuccessData);
            return respEntity.SuccessData;
        }

        [Fact]
        public DummySubjectSM GetAllDummySubjectsByIdTests()
        {
            int idToTest = 1;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ApiResponse<DummySubjectSM> respEntity = _targetClient.GetDummySubjectById(_authClientWrapper, cancellationTokenSource.Token, idToTest)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(respEntity);
            Assert.False(respEntity.IsError);
            Assert.NotNull(respEntity.SuccessData);
            Assert.Equal(idToTest, respEntity.SuccessData.Id);
            return respEntity.SuccessData;
        }

        [Fact]
        public void AddUpdateDeleteDummySubjectTests()
        {
            var addResult = AddDummySubject();
            Assert.NotNull(addResult);
            Assert.False(addResult.IsError);
            Assert.NotNull(addResult.SuccessData);
            Assert.True(addResult.SuccessData.Id > 0);

            var updateResult = UpdateDummySubject(addResult.SuccessData.Id);
            Assert.NotNull(updateResult);
            Assert.False(updateResult.IsError);
            Assert.NotNull(updateResult.SuccessData);
            Assert.True(updateResult.SuccessData.Id > 0);
            Assert.Equal(addResult.SuccessData.Id, updateResult.SuccessData.Id);
            Assert.True(updateResult.SuccessData.SubjectName.StartsWith("Updated"));

            var deleteResult = DeleteDummySubject(addResult.SuccessData.Id);
            Assert.NotNull(deleteResult);
            Assert.False(deleteResult.IsError);
            Assert.NotNull(deleteResult.SuccessData);
            Assert.True(deleteResult.SuccessData.DeleteResult);
        }

        private ApiResponse<DummySubjectSM> AddDummySubject()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var targetObj = new DummySubjectSM()
            {
                SubjectName = "TC_SubName_" + Random.Shared.NextDouble().ToString(),
                SubjectCode = Guid.NewGuid().ToString()
            };
            ApiResponse<DummySubjectSM> respEntity = _targetClient.AddDummySubject(_authClientWrapper, cancellationTokenSource.Token, targetObj)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return respEntity;
        }
        private ApiResponse<DummySubjectSM> UpdateDummySubject(int targetId)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var targetObj = new DummySubjectSM()
            {
                SubjectName = "Updated_TC_SubName_" + Random.Shared.NextDouble().ToString(),
                SubjectCode = "Updated_" + Guid.NewGuid().ToString()
            };
            ApiResponse<DummySubjectSM> respEntity = _targetClient.UpdateDummySubject(_authClientWrapper, cancellationTokenSource.Token, targetId, targetObj)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return respEntity;
        }
        private ApiResponse<DeleteResponseRoot> DeleteDummySubject(int targetId)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ApiResponse<DeleteResponseRoot> respEntity = _targetClient.DeleteDummySubjectById(_authClientWrapper, cancellationTokenSource.Token, targetId)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return respEntity;
        }
    }
}
