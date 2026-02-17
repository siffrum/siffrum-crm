using Siffrum.Web.Payroll.Client.Base;
using Siffrum.Web.Payroll.Client.Constants;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Reno.Web.Foundation.ServiceModels.CommonResponse;

namespace Siffrum.Web.Payroll.Client.ApiClients
{
    public class ClientEmployeeDocumentClient : SiffrumPayrollApiClientBase
    {
        public ClientEmployeeDocumentClient(AccessingClientDetails accessingClientDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClientDetails, onExceptionInClient)
        {
        }
        public async Task<ApiResponse<List<ClientEmployeeDocumentSM>>> GetAllClientEmployeeDocuments(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeDocumentSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeDocumentSM>> GetClientEmployeeDocumentById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeDocumentSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}/{targetId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeDocumentSM>> GetClientEmployeeDocumentByEmpId(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int empId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, ClientEmployeeDocumentSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}/{empId}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<List<ClientEmployeeDocumentSM>>> GetPartialEmployeeDocuments(AuthClientWrapper authClientWrapper, CancellationToken cancelToken)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, List<ClientEmployeeDocumentSM>>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}",
                HttpMethod.Get, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);
            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeDocumentSM>> AddClientEmployeeDocument(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, ClientEmployeeDocumentSM targetObj)
        {
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for add", "Invalid object for add, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var addRequest = new ApiRequest<ClientEmployeeDocumentSM>() { ReqData = targetObj };
            addRequest.ReqData.Id = 0;
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeDocumentSM, ClientEmployeeDocumentSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}",
                HttpMethod.Post, addRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<ClientEmployeeDocumentSM>> UpdateClientEmployeeDocument(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId, ClientEmployeeDocumentSM targetObj)
        {
            if (targetId <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, $"invalid Id with value '{targetId}' passed for update", "Invalid id passed, please try again");
            if (targetObj == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.InvalidInputData_Log, "null object passed for update", "Invalid object for update, please try again");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var updateRequest = new ApiRequest<ClientEmployeeDocumentSM>() { ReqData = targetObj };
            updateRequest.ReqData.Id = 0;// ensure no id passed for reference
            var respEntity = await base.GetResponseEntityAsync<ClientEmployeeDocumentSM, ClientEmployeeDocumentSM>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}/{targetId}",
                HttpMethod.Put, updateRequest, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

        public async Task<ApiResponse<DeleteResponseRoot>> DeleteClientEmployeeDocumentById(AuthClientWrapper authClientWrapper, CancellationToken cancelToken, int targetId)
        {
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            var respEntity = await base.GetResponseEntityAsync<string, DeleteResponseRoot>
                ($"{ApiUrls.CLIENT_EMPLOYEE_DOCUMENT_URL}/{targetId}",
                HttpMethod.Delete, null, cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return respEntity;
        }

    }
}
