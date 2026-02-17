using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.Net;

namespace Siffrum.Web.Payroll.API.Controllers.Client
{
    [Route("api/v1/[controller]")]
    public partial class ClientCompanyDetailController : ApiControllerWithOdataRoot<ClientCompanyDetailSM>
    {
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        private readonly IWebHostEnvironment _environment;

        public ClientCompanyDetailController(ClientCompanyDetailProcess clientCompanyDetailProcess, IWebHostEnvironment environment)
            : base(clientCompanyDetailProcess)
        {
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            _environment = environment;
        }
        //}

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyDetailSM>>>> GetAsOdata(ODataQueryOptions<ClientCompanyDetailSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientCompanyDetailSM>>>> GetAll()
        {
            var listSM = await _clientCompanyDetailProcess.GetAllClientCompanyDetails();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> GetById(int id)
        {
            var singleSM = await _clientCompanyDetailProcess.GetClientCompanyDetailById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> GetMyClientCompanyDetail()
        {
            int currentCompanyRecordId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetById(currentCompanyRecordId);
        }

        #endregion Get Endpoints

        #region --Mine-EndPoints--

        [HttpGet("mine/CompanyDetail")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> GetMineClienCompanyDetails()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetById(currentUserRecordId);
        }

        [HttpGet("mine/Logo")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<string>>> GetMineClientCompanyLogo()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            var imageResp = await _clientCompanyDetailProcess.GetMineCompanyLogo(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(imageResp));
        }

        #endregion --Mine-EndPoints

        #region --COUNT--

        [HttpGet]
        [Route("ClientCompanyDetailCountResponse")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientCompanyDetailCountsResponse()
        {
            var countResp = await _clientCompanyDetailProcess.GetClientCompanyDetailCountsResponse();
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        #endregion --COUNT--

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> Post([FromBody] ApiRequest<ClientCompanyDetailSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _clientCompanyDetailProcess.AddClientCompanyDetail(innerReq);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyDetailController.GetById), new
                {
                    id = addedSM.Id
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> Put(int id, [FromBody] ApiRequest<ClientCompanyDetailSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            if (id <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _clientCompanyDetailProcess.UpdateClientCompanyDetail(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("my/logo")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> AddOrUpdateCompanyIcon()// do not read body in case of MultiPartForm
        {
            #region Check Request

            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }

            (var apiRequest, var formFiles)
                = await base.TryReadApiRequestAsMultipart<object>(ensureAtLeastOneFile: true);
            //if req object needed
            //var innerReq = apiRequest?.ReqData;
            //if (innerReq == null)
            //{
            //    return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            //}

            #endregion Check Request

            var resp = await _clientCompanyDetailProcess.AddOrUpdateCompanyDetailLogoInDb(currentCompanyId, _environment.WebRootPath, formFiles.First());
            if (!string.IsNullOrWhiteSpace(resp))
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_FileNotSaved, ApiErrorTypeSM.Fatal_Log));
            }
        }

        [HttpPost("AddCompanyDetailsForRegistration")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> AddClientCompanyDetails([FromBody] ApiRequest<ClientCompanyDetailSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _clientCompanyDetailProcess.AddClientCompanyDetailForRegistration(innerReq);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyDetailController.GetById), new
                {
                    id = addedSM.Id
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Add/Update Endpoints

        #region --Add/Update Company-Logo Endpoints--

        [HttpPost("mine/CompanyLogo")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> AddUpdateMineCompanyLogo([FromBody] ApiRequest<string> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }
            #endregion Check Request

            var addedSM = await _clientCompanyDetailProcess.AddUpdateCompanyLogo(innerReq, currentCompanyId);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyDetailController.GetById), new
                {
                    id = currentCompanyId
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Add/Update Company-Logo Endpoints--

        #region --Delete Company-Logo EndPoint--

        [HttpDelete("mine/DeleteCompanyLogo")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMineCompanyLogo()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientCompanyDetailProcess.DeleteCompanyLogoById(currentCompanyId);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Delete Company-Logo EndPoint--

        #region Delete Endpoints

        [HttpDelete("{id}")]

        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientCompanyDetailProcess.DeleteClientCompanyDetailById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpDelete("my/logo")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteCompanyIcon()
        {
            #region Check Request

            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyId <= 0)
            { return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims)); }

            #endregion Check Request

            var resp = await _clientCompanyDetailProcess.DeleteClientCompanyDetailLogoById(currentCompanyId, _environment.WebRootPath);
            if (resp != null && resp.DeleteResult)
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            else
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
        }

        #endregion Delete Endpoints
    }
}
