using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientEmployeeLeaveClient : SiffrumPayrollApiClientBase
    {
        public ClientEmployeeLeaveClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }
        public async Task<ApiResponse<List<ClientEmployeeLeaveSM>>> GetAllClientEmployeeLeave(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeLeaveSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<List<ClientEmployeeLeaveSM>>> GetClientEmployeeLeaveByUserId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeLeaveSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeLeaveCountEndPointSM>> GetClientAllEmployeeLeaveCountsResponse(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeLeaveCountEndPointSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeLeaveSM>> GetClientEmployeeLeaveById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeLeaveSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeLeaveCountEndPointSM>> GetEmployeeLeaveCountByUserId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeLeaveCountEndPointSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeLeaveSM>> GetAllLeavesReport(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, LeaveReportRequestSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<LeaveReportRequestSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<LeaveReportRequestSM, ClientEmployeeLeaveSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }


        public async Task<ApiResponse<ClientEmployeeLeaveSM>> AddClientEmployeeLeave(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientEmployeeLeaveSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientEmployeeLeaveSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeLeaveSM, ClientEmployeeLeaveSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeLeaveSM>> UpdateClientEmployeeLeave(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientEmployeeLeaveSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientEmployeeLeaveSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeLeaveSM, ClientEmployeeLeaveSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientEmployeeCTCDetailId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_EMPLOYEE_LEAVE_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

    }
}
