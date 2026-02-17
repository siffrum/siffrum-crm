using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public class ClientEmployeeAdditionalReimbursementLogsController : ApiControllerWithOdataRoot<ClientEmployeeAdditionalReimbursementLogSM>
    {
        private readonly ClientEmployeeAdditionalReimbursementLogProcess _clientEmployeeAdditionalReimbursementLogProcess;
        public ClientEmployeeAdditionalReimbursementLogsController(ClientEmployeeAdditionalReimbursementLogProcess clientEmployeeAdditionalReimbursementLogProcess)
            : base(clientEmployeeAdditionalReimbursementLogProcess)
        {
            _clientEmployeeAdditionalReimbursementLogProcess = clientEmployeeAdditionalReimbursementLogProcess;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>>> GetAsOdata(ODataQueryOptions<ClientEmployeeAdditionalReimbursementLogSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>>> GetAll()
        {
            var listSM = await _clientEmployeeAdditionalReimbursementLogProcess.GetAllClientEmployeeAdditionalReimbursementLogs();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>>> GetById(int id)
        {
            var singleSM = await _clientEmployeeAdditionalReimbursementLogProcess.GetClientEmployeeAdditionalReimbursementLogById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("EmployeeId/{empId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>>> GetByEmpId(int empId)
        {
            var singleSM = await _clientEmployeeAdditionalReimbursementLogProcess.GetClientEmployeeAdditionalReimbursementLogsByUserId(empId);
            if (singleSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(singleSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #region --COUNT--

        [HttpGet("ClientEmployeeAdditionalReimbursementLogsCountResponse/{empId}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientEmployeeAdditionalReimbursementLogsCountsResponse(int empId)
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _clientEmployeeAdditionalReimbursementLogProcess.GetClientEmployeeAdditionalReimbursementLogsCounts(empId, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        #endregion --COUNT__

        #endregion Get Endpoints

        #region --My End-Points--

        [HttpGet("my/EmployeeAdditionalReimbursementLogsByEmployeeID/{empId}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>>> GetEmployeeAdditionalReimbursementLogsByEmployeeIdOfMyCompany(int empId)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAdditionalReimbursementLogProcess.GetClientEmployeeAdditionalReimbursementLogsByEmployeeIdOfMyCompany(currentCompanyId, empId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/AdditionalReimbursementDetails")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>>> GetMineEmployeeAdditionalReimbursementLogs()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetByEmpId(currentUserRecordId);
        }

        #endregion --MY ENDPOINTS-- 

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>>> Post([FromBody] ApiRequest<ClientEmployeeAdditionalReimbursementLogSM> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _clientEmployeeAdditionalReimbursementLogProcess.AddClientEmployeeAdditionalReimbursementLog(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientEmployeeAdditionalReimbursementLogsController.GetById), new
                {
                    id = subM.Id
                }, ModelConverter.FormNewSuccessResponse(subM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAdditionalReimbursementLogSM>>> Put(int id, [FromBody] ApiRequest<ClientEmployeeAdditionalReimbursementLogSM> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            if (id <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _clientEmployeeAdditionalReimbursementLogProcess.UpdateEmployeeAdditionalReimbursementDetail(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("AllEmployeeReimbursementLogReport")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>> GetAllEmployeeAdditionalReimbursementLog([FromBody] ApiRequest<EmployeeAdditionalReimbursementLogReportRequestSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAdditionalReimbursementLogProcess.GetTotalEmployeeAdditionalReimbursementReport(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpPost("my/ReimbursementLogReportByUserId")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAdditionalReimbursementLogSM>>>> GetAllEmployeeAdditionalReimbursementLogByUserId([FromBody] ApiRequest<EmployeeAdditionalReimbursementLogReportRequestSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAdditionalReimbursementLogProcess.GetTotalEmployeeAdditionalReimbursementReportByUserId(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion Add/Update Endpoints

        #region Delete Endpoints

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientEmployeeAdditionalReimbursementLogProcess.DeleteClientEmployeeAdditionalReimbursementLog(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Delete Endpoints

        #region --My Delete Endpoint--

        [HttpDelete("my/EmployeeAdditionalReimbursementLog/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyClientEmployeeAdditionalReimbursementLog(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientEmployeeAdditionalReimbursementLogProcess.DeleteMyClientEmployeeAdditionalReimbursementLogById(id, currentCompanyId);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --My Delete Endpoint--

        #region My/Mine-Delete EmployeeDocument

        [HttpDelete("my/DeleteReimbursementDocuments/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteReimbursementDocument(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientEmployeeAdditionalReimbursementLogProcess.DeleteMyReimburseDocumentById(id, currentCompanyId);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion My/Mine-Delete EmployeeDocument

    }
}
