using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientEmployeeAdditionalReimbursementLogClient : SiffrumPayrollApiClientBase
    {
        public ClientEmployeeAdditionalReimbursementLogClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }
        public async Task<ApiResponse<List<ClientEmployeeAdditionalReimbursementLogSM>>> GetAllClientEmployeeAdditionalReimbursementLog(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeAdditionalReimbursementLogSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_ADDITIONAL_REIMBURSEMENTLOG_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> GetClientEmployeeAdditionalReimbursementLogById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeAdditionalReimbursementLogSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_ADDITIONAL_REIMBURSEMENTLOG_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> GetClientEmployeeAdditionalReimbursementLogByEmpId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int empId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeAdditionalReimbursementLogSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_ADDITIONAL_REIMBURSEMENTLOG_URL}/{empId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> AddClientEmployeeAdditionalReimbursementLog(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientEmployeeAdditionalReimbursementLogSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientEmployeeAdditionalReimbursementLogSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeAdditionalReimbursementLogSM, ClientEmployeeAdditionalReimbursementLogSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_ADDITIONAL_REIMBURSEMENTLOG_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>> UpdateClientEmployeeAdditionalReimbursementLog(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientEmployeeAdditionalReimbursementLogSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientEmployeeAdditionalReimbursementLogSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeAdditionalReimbursementLogSM, ClientEmployeeAdditionalReimbursementLogSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_ADDITIONAL_REIMBURSEMENTLOG_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientEmployeeAdditionalReimbursementLogById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_EMPLOYEE_ADDITIONAL_REIMBURSEMENTLOG_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }


    }
}
