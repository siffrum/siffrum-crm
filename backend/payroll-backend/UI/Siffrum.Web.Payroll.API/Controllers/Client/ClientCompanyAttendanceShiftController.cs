using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.Client
{
    [Route("api/v1/[controller]")]
    public class ClientCompanyAttendanceShiftController : ApiControllerWithOdataRoot<ClientCompanyAttendanceShiftSM>
    {
        private readonly ClientCompanyAttendanceShiftProcess _clientCompanyAttendanceShiftProcess;
        public ClientCompanyAttendanceShiftController(ClientCompanyAttendanceShiftProcess clientCompanyAttendanceShiftProcess)
            : base(clientCompanyAttendanceShiftProcess)
        {
            _clientCompanyAttendanceShiftProcess = clientCompanyAttendanceShiftProcess;
        }
        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyAttendanceShiftSM>>>> GetAsOdata(ODataQueryOptions<ClientCompanyAttendanceShiftSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyAttendanceShiftSM>>>> GetAll()
        {
            var listSM = await _clientCompanyAttendanceShiftProcess.GetAllClientCompanyAttendanceShift();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyAttendanceShiftSM>>> GetById(int id)
        {
            var singleSM = await _clientCompanyAttendanceShiftProcess.GetClientCompanyAttendanceShiftById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #region My-EndPoints
        [HttpGet("my/CompanyAttendanceShift")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyAttendanceShiftSM>>>> GetMyClientCompanyAttendanceShiftById()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientCompanyAttendanceShiftProcess.GetMyClientCompanyAttendanceShiftById(currentCompanyId);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion My-EndPoints

        #endregion Get Endpoints

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyAttendanceShiftSM>>> Post([FromBody] ApiRequest<ClientCompanyAttendanceShiftSM> apiRequest)
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

            var subM = await _clientCompanyAttendanceShiftProcess.AddClientCompanyAttendanceShift(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyAddressController.GetById), new
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyAttendanceShiftSM>>> Put(int id, [FromBody] ApiRequest<ClientCompanyAttendanceShiftSM> apiRequest)
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

            var resp = await _clientCompanyAttendanceShiftProcess.UpdateClientCompanyAttendanceShift(id, innerReq);
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientCompanyAttendanceShiftProcess.DeleteClientCompanyAttendanceShiftById(id);
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

        #region --My-Delete EndPoints--

        [HttpDelete("my/CompanyAttendanceShift/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyCompanyAttendanceShift(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientCompanyAttendanceShiftProcess.DeleteMyClientCompanyAttendanceShiftById(id, currentCompanyId);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion --My-Delete EndPoints--

        #region --Mine Attendance-Shift--

        [HttpGet("mine/AttendanceShift")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientCompanyAttendanceShiftSM>>> GetMineClientCompanyAttendanceShift()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            var singleSM = await _clientCompanyAttendanceShiftProcess.GetMineClientCompanyAttendanceShiftById(currentUserRecordId);
            return ModelConverter.FormNewSuccessResponse(singleSM);
        }

        #endregion --Mine Attendance-Shift--


    }
}
