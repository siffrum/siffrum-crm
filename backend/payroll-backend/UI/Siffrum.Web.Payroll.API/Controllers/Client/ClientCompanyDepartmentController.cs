using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.Client
{
    [Route("api/v1/[controller]")]
    public class ClientCompanyDepartmentController : ApiControllerWithOdataRoot<ClientCompanyDepartmentSM>
    {
        #region Properties

        private readonly ClientCompanyDepartmentProcess _clientCompanyDepartmentProcess;

        #endregion Properties

        #region Constructor
        public ClientCompanyDepartmentController(ClientCompanyDepartmentProcess clientCompanyDepartmentProcess)
            : base(clientCompanyDepartmentProcess)
        {
            _clientCompanyDepartmentProcess = clientCompanyDepartmentProcess;
        }

        #endregion Constructor

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyDepartmentSM>>>> GetAsOdata(ODataQueryOptions<ClientCompanyDepartmentSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyDepartmentSM>>>> GetAll()
        {
            var listSM = await _clientCompanyDepartmentProcess.GetAllClientCompanyDepartment();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDepartmentSM>>> GetById(int id)
        {
            var singleSM = await _clientCompanyDepartmentProcess.GetClientCompanyDepartmentById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Get Endpoints

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDepartmentSM>>> Post([FromBody] ApiRequest<ClientCompanyDepartmentSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _clientCompanyDepartmentProcess.AddClientCompanyDepartment(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyDepartmentController.GetById), new
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
        public async Task<ActionResult<ApiResponse<ClientCompanyDepartmentSM>>> Put(int id, [FromBody] ApiRequest<ClientCompanyDepartmentSM> apiRequest)
        {
            #region Check Request
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

            var resp = await _clientCompanyDepartmentProcess.UpdateClientCompanyDepartment(id, innerReq);
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
            var resp = await _clientCompanyDepartmentProcess.DeleteClientCompanyDepartmentById(id);
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

        #region --My and Mine Get-Method--
        [HttpGet("my/CompanyDepartments")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyDepartmentReportSM>>>> GetMyClientCompanyDepartments()
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientCompanyDepartmentProcess.GetAllMyClientCompanyDepartment(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/Department")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDepartmentReportSM>>> GetMineClientCompanyDepartments()
        {
            var currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            var singleSM = await _clientCompanyDepartmentProcess.GetMineClientCompanyDepartment(currentUserId);
            return Ok(ModelConverter.FormNewSuccessResponse(singleSM));
        }

        #endregion --My and Mine Get-Method--

        #region --My and Mine Add/Update Methods--

        [HttpPost("my")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDepartmentSM>>> MyCompanyDeparment([FromBody] ApiRequest<ClientCompanyDepartmentSM> apiRequest)
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

            var subM = await _clientCompanyDepartmentProcess.AddClientCompanyDepartment(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyDepartmentController.GetById), new
                {
                    id = subM.Id
                }, ModelConverter.FormNewSuccessResponse(subM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("my/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDepartmentSM>>> MyCompanyDepartment(int id, [FromBody] ApiRequest<ClientCompanyDepartmentSM> apiRequest)
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

            var resp = await _clientCompanyDepartmentProcess.UpdateClientCompanyDepartment(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --My and Mine Add/Update Methods--

        #region --Report--

        [HttpGet("my/ClientCompanyDepartmentReport/{skip}/{top}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyDepartmentReportSM>>>> GetClientCompanyDepartmentReport(int skip, int top)
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientCompanyDepartmentProcess.GetClientCompanyDepartmentReport(currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion --Report--

    }
}
