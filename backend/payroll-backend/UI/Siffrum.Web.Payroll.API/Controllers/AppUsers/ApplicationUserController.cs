using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.Net;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public partial class ApplicationUserController : ApiControllerWithOdataRoot<ApplicationUserSM>
    {
        private readonly ApplicationUserProcess _applicationUserProcess;
        private readonly IWebHostEnvironment _environment;

        public ApplicationUserController(ApplicationUserProcess process, IWebHostEnvironment environment)
            : base(process)
        {
            _applicationUserProcess = process;
            _environment = environment;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ApplicationUserSM>>>> GetAsOdata(ODataQueryOptions<ApplicationUserSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ApplicationUserSM>>>> GetAll()
        {
            var listSM = await _applicationUserProcess.GetAllApplicationUsers();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ApplicationUserSM>>> GetById(int id)
        {
            var singleSM = await _applicationUserProcess.GetApplicationUserById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("mine")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin,ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ApplicationUserSM>>> GetMineApplicationUserDetails()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetById(currentUserRecordId);
        }

        #endregion Get Endpoints

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ApplicationUserSM>>> Post([FromBody] ApiRequest<ApplicationUserSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _applicationUserProcess.AddApplicationUser(innerReq);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(GetById), new
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
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ApplicationUserSM>>> Put(int id, [FromBody] ApiRequest<ApplicationUserSM> apiRequest)
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

            var updatedSM = await _applicationUserProcess.UpdateApplicationUser(id, innerReq);
            if (updatedSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(updatedSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("mine/logo")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientCompanyDetailSM>>> AddOrUpdateUserProfilePicture()// do not read body in case of MultiPartForm
        {
            #region Check Request

            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }

            (var apiRequest, var formFiles)
                = await TryReadApiRequestAsMultipart<object>(ensureAtLeastOneFile: true);

            #endregion Check Request

            var resp = await _applicationUserProcess.AddOrUpdateProfilePictureInDb(currentUserRecordId, _environment.WebRootPath, formFiles.First());
            if (!string.IsNullOrWhiteSpace(resp))
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_FileNotSaved, ApiErrorTypeSM.Fatal_Log));
            }
        }


        #endregion Add/Update Endpoints

        #region Delete Endpoints

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _applicationUserProcess.DeleteApplicationUserById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpDelete("mine/logo")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteUserProfilePicture()
        {
            #region Check Request

            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyId <= 0)
            { return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims)); }

            #endregion Check Request

            var resp = await _applicationUserProcess.DeleteProfilePictureById(currentCompanyId, _environment.WebRootPath);
            if (resp != null && resp.DeleteResult)
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            else
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
        }

        #endregion Delete Endpoints
    }
}
