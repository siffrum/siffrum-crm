using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientUserAddressClient : SiffrumPayrollApiClientBase
    {
        public ClientUserAddressClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }
        public async Task<ApiResponse<List<ClientUserAddressSM>>> GetAllClientUserAddress(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientUserAddressSM>>
                ($"{ApiUrls.CLIENT_USER_ADDRESS_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientUserAddressSM>> GetClientUserAddressById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientUserAddressSM>
                ($"{ApiUrls.CLIENT_USER_ADDRESS_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientUserAddressSM>> AddClientUserAddress(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientUserAddressSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientUserAddressSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientUserAddressSM, ClientUserAddressSM>
                ($"{ApiUrls.CLIENT_USER_ADDRESS_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientUserAddressSM>> UpdateClientUserAddress(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientUserAddressSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientUserAddressSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientUserAddressSM, ClientUserAddressSM>
                ($"{ApiUrls.CLIENT_USER_ADDRESS_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientUserAddressById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_USER_ADDRESS_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

    }
}
