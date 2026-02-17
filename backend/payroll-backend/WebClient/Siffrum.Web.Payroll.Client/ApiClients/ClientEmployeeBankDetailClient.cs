using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientEmployeeBankDetailClient : SiffrumPayrollApiClientBase
    {
        public ClientEmployeeBankDetailClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }

        public async Task<ApiResponse<List<ClientEmployeeBankDetailSM>>> GetAllClientEmployeeBankDetail(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeBankDetailSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_BANK_DETAIL_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeBankDetailSM>> GetClientEmployeeBankDetailById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeBankDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_BANK_DETAIL_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeBankDetailSM>> GetClientEmployeeBankDetailByEmpId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int empId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeBankDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_BANK_DETAIL_URL}/{empId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeBankDetailSM>> AddClientEmployeeBankDetail(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientEmployeeBankDetailSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientEmployeeBankDetailSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeBankDetailSM, ClientEmployeeBankDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_BANK_DETAIL_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeBankDetailSM>> UpdateClientEmployeeBankDetail(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientEmployeeBankDetailSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientEmployeeBankDetailSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeBankDetailSM, ClientEmployeeBankDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_BANK_DETAIL_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientEmployeeBankDetailById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_EMPLOYEE_BANK_DETAIL_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

    }
}
