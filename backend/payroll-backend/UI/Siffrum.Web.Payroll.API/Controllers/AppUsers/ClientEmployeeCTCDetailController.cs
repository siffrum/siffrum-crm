using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public class ClientEmployeeCTCDetailController : ApiControllerWithOdataRoot<ClientEmployeeCTCDetailSM>
    {
        private readonly ClientEmployeeCTCDetailProcess _clientEmployeeCTCDetailProcess;
        public ClientEmployeeCTCDetailController(ClientEmployeeCTCDetailProcess process)
            : base(process)
        {
            _clientEmployeeCTCDetailProcess = process;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailSM>>>> GetAsOdata(ODataQueryOptions<ClientEmployeeCTCDetailSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailSM>>>> GetAll()
        {
            var listSM = await _clientEmployeeCTCDetailProcess.GetAllClientEmployeeCTCDetails();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeCTCDetailSM>>> GetById(int id)
        {
            var singleSM = await _clientEmployeeCTCDetailProcess.GetClientEmployeeCTCDetailById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("ClientEmployeeCTCDetailByUserId/{empid}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailSM>>>> GetClientEmployeeCTCDetailByUserId(int empid)
        {
            var singleSM = await _clientEmployeeCTCDetailProcess.GetClientEmployeeCtcDetailByEmpId(empid);
            if (singleSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(singleSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }


        #endregion Get Endpoints

        #region --COUNT--

        [HttpGet("ClientEmployeeCtcDetailCountResponse/{empId}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientEmployeeCtcDetailCountsResponse(int empId)
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _clientEmployeeCTCDetailProcess.GetClientEmployeeCtcDetailCounts(empId, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        [HttpPost("EmployeeCtcReportCount")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetLeaveReportCountsResponse([FromBody] ApiRequest<EmployeeCTCReportRequestSM> employeeCTCReportRequestSM)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = employeeCTCReportRequestSM?.ReqData;
            var countResp = await _clientEmployeeCTCDetailProcess.GetEmployeeCTCReportCount(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));

        }

        #endregion --COUNT--

        #region --My End-Points--

        [HttpGet("my/EmployeesCTCDetailByEmployeeID/{empId}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailSM>>>> GetEmployeeCTCDetailByEmployeeIdOfMyCompany(int empId)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeCTCDetailProcess.GetClientUsersCtcDetailByEmployeeIdOfMyCompany(currentCompanyId, empId);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("my/AllEmployeesCTCDetails")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserSM>>>> GetAllEmployeesCTCDetailsOfMyCompany()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeCTCDetailProcess.GetEmployeesCTCDetailsOfMyCompany(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/CTCDetails")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailSM>>>> GetMineClientEmployeeCTCDetails()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetClientEmployeeCTCDetailByUserId(currentUserRecordId);
        }

        #endregion --My End-Points--

        #region --Reports--

        [HttpPost("AllEmployeeCTCReport/{skip}/{top}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailExtendedUserSM>>>> GetAllEmployeeCTCReport([FromBody] ApiRequest<EmployeeCTCReportRequestSM> apiRequest, int skip, int top)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeCTCDetailProcess.GetTotalEmployeeCTCReport(innerReq, currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpPost("CTCReportByClientUserId/{skip}/{top}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeLeaveExtendedUserSM>>>> GetEmployeeCTCReportByClientUserId([FromBody] ApiRequest<EmployeeCTCReportRequestSM> apiRequest, int skip, int top)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeCTCDetailProcess.GetEmployeeCtcReportByClientId(innerReq, currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        #endregion --Report--

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeCTCDetailSM>>> Post([FromBody] ApiRequest<ClientEmployeeCTCDetailSM> apiRequest)
        {
            #region Check Request
            //int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            //apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _clientEmployeeCTCDetailProcess.AddClientEmployeeCTCDetail(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientEmployeeCTCDetailController.GetById), new
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
        public async Task<ActionResult<ApiResponse<ClientEmployeeCTCDetailSM>>> Put(int id, [FromBody] ApiRequest<ClientEmployeeCTCDetailSM> apiRequest)
        {
            #region Check Request
            //int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            //apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
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

            var resp = await _clientEmployeeCTCDetailProcess.UpdateClientEmployeeCTCDetail(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        //[HttpPut("UpdateActiveCtcDetail")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeCTCDetailSM>>>> CtcdetailUpadte([FromBody] ApiRequest<IEnumerable<ClientEmployeeCTCDetailSM>> apiRequest)
        //{
        //    #region Check Request
        //    //int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
        //    //apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
        //    foreach (var item in apiRequest.ReqData)
        //    {
        //        apiRequest.ReqData.ToList().Add(item);
        //    }
        //    var innerReq = apiRequest?.ReqData;
        //    if (innerReq == null)
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
        //    }

        //    #endregion Check Request

        //    var resp = await _clientEmployeeCTCDetailProcess.UpdateActiveEmployeeCTCDetails((List<ClientEmployeeCTCDetailSM>)innerReq);
        //    if (resp != null)
        //    {
        //        return Ok(ModelConverter.FormNewSuccessResponse(resp));
        //    }
        //    else
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
        //    }
        //}

        [HttpPut("UpdateCtcDetail/{id}/{userId}/{status}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> UpdateActiveCtc(int id, int userId, bool status)
        {

            var resp = await _clientEmployeeCTCDetailProcess.UpdateActiveEmployeeCTC(id, userId, status);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Add/Update Endpoints

        #region Delete Endpoints

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientEmployeeCTCDetailProcess.DeleteClientEmployeeCTCDetail(id);
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

        [HttpDelete("my/EmployeeCTCDetail/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyEmployeeCTCDetail(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientEmployeeCTCDetailProcess.DeleteMyClientEmployeeCTCDetailById(id, currentCompanyId);
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

    }
}
