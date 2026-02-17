using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientEmployeeCTCDetailClient : SiffrumPayrollApiClientBase
    {
        public ClientEmployeeCTCDetailClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }

        public async Task<ApiResponse<List<ClientEmployeeCTCDetailSM>>> GetAllClientEmployeeCTCDetail(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeCTCDetailSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_CTC_DETAIL_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeCTCDetailSM>> GetClientEmployeeCTCDetailById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeCTCDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_CTC_DETAIL_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeCTCDetailSM>> GetClientEmployeeCTCDetailByEmpId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int empId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeCTCDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_CTC_DETAIL_URL}/{empId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeCTCDetailSM>> AddClientEmployeeCTCDetail(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientEmployeeCTCDetailSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientEmployeeCTCDetailSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeCTCDetailSM, ClientEmployeeCTCDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_CTC_DETAIL_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeCTCDetailSM>> UpdateClientEmployeeCTCDetail(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientEmployeeCTCDetailSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientEmployeeCTCDetailSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeCTCDetailSM, ClientEmployeeCTCDetailSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_CTC_DETAIL_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientEmployeeCTCDetailId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_EMPLOYEE_CTC_DETAIL_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }


    }
}
