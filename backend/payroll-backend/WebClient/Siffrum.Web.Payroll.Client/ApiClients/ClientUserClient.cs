using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientUserClient : SiffrumPayrollApiClientBase
    {
        public ClientUserClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }

        public async Task<ApiResponse<List<ClientUserSM>>> GetAllClientUsers(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientUserSM>>
                ($"{ApiUrls.CLIENT_USER_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientUserSM>> GetClientUserById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientUserSM>
                ($"{ApiUrls.CLIENT_USER_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientUserSM>> AddClientUser(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientUserSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientUserSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientUserSM, ClientUserSM>
                ($"{ApiUrls.CLIENT_USER_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientUserSM>> UpdateClientUser(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientUserSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientUserSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientUserSM, ClientUserSM>
                ($"{ApiUrls.CLIENT_USER_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientUserById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_USER_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

    }
}
