using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.Net;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public partial class ClientUserController : ApiControllerWithOdataRoot<ClientUserSM>
    {
        #region Properties
        private readonly ClientUserProcess _clientUserProcess;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Properties

        #region Constructor
        public ClientUserController(ClientUserProcess process, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
            : base(process)
        {
            _clientUserProcess = process;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Constructor

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserSM>>>> GetAsOdata(ODataQueryOptions<ClientUserSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);

            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserSM>>>> GetAll()
        {
            var listSM = await _clientUserProcess.GetAllClientUsers();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> GetById(int id)
        {
            var singleSM = await _clientUserProcess.GetClientUserById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }


        [HttpGet("my/AllEmployees")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserSM>>>> GetAllEmployeesOfMyCompany()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientUserProcess.GetAllClientUsersOfMyCompany(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("my/EmployeeById/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> GetEmployeeByIdOfMyCompany(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var singleSM = await _clientUserProcess.GetClientUserByIdOfMyCompany(id, currentCompanyId);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("my/BadgeIdCardByEmployeeId/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<BadgeIdCardsSM>>> GetBadgeIdCardByIdOfMyCompany(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var singleSM = await _clientUserProcess.GetBadgeIdCardByIdOfMyCompany(id, currentCompanyId);
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> GetMineClientUserDetails()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetById(currentUserRecordId);
        }

        [HttpPost("mine/logo")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
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

            var resp = await _clientUserProcess.AddOrUpdateProfilePictureInDb(currentUserRecordId, _environment.WebRootPath, formFiles.First());
            if (!string.IsNullOrWhiteSpace(resp))
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_FileNotSaved, ApiErrorTypeSM.Fatal_Log));
            }
        }


        #endregion Get Endpoints

        #region ForgotPassword & ResetPassword EndPoints

        [HttpPost("ForgotPassword")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> ForgotPassword([FromBody] ApiRequest<ForgotPasswordSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            var httpContext = _httpContextAccessor.HttpContext;
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";
            var link = $"{baseUrl}/ResetPassword";
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _clientUserProcess.SendResetPasswordLink(innerReq, link);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("ResetPassword")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> ResetPassword([FromBody] ApiRequest<ResetPasswordRequestSM> apiRequest)
        {
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }
            var resp = await _clientUserProcess.UpdatePassword(innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("ValidatePassword")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> ValidatePassword(string authCode)
        {
            var resp = await _clientUserProcess.ValidatePassword(authCode);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion ForgotPassword & ResetPassword EndPoints

        #region --COUNT--

        [HttpGet]
        [Route("my/ClientEmployeeUserCountResponse")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientEmployeeUserCountsResponse()
        {
            var companyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _clientUserProcess.GetClientEmployeeUserCountResponse(companyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        [HttpGet]
        [Route("ClientCompanyUserCountResponse/{companyId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientCompanyUserCountsResponse(int companyId)
        {
            var countResp = await _clientUserProcess.GetClientCompanyUserCountsResponse(companyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        [HttpPost("EmployeeReportCountBasedOnStatus/{employeeStatusSM}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetEmployeeReportCountsBasedOnStatusResponse(EmployeeStatusSM employeeStatusSM)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _clientUserProcess.GetEmployeeReportCountBasedOnStatus(employeeStatusSM, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));

        }


        #endregion --COUNT--

        #region My-Add/Update Endpoints

        [HttpPost()]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> Post([FromBody] ApiRequest<ClientUserSM> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _clientUserProcess.AddClientUser(innerReq);
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> Put(int id, [FromBody] ApiRequest<ClientUserSM> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
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

            var updatedSM = await _clientUserProcess.UpdateClientUser(id, innerReq);
            if (updatedSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(updatedSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion My-Add/Update Endpoints

        #region Mine Update-Theme

        [HttpPut("mine/Theme/{themeId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> UpdateTheme(int themeId)
        {
            #region Check Request
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            if (themeId <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var response = await _clientUserProcess.UpdateClientUserTheme(themeId, currentUserId);
            if (response != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(response));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Mine Update-Theme

        #region My Update-Department

        [HttpPut("my/Department/{departmentId}/{userId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> UpdateDepartment(int departmentId, int userId)
        {
            #region Check Request
            int currentComnpanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (departmentId <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var response = await _clientUserProcess.UpdateClientUserDepartment(departmentId, userId, currentComnpanyId);
            if (response != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(response));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion My Update-Department

        #region Add/Update Endpoints

        [HttpPost("CompanyAdmin")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> AddCompanyAdmin([FromBody] ApiRequest<ClientUserSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _clientUserProcess.AddClientUser(innerReq);
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

        [HttpPut("CompanyAdmin/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> UpdateCompanyAdmin(int id, [FromBody] ApiRequest<ClientUserSM> apiRequest)
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

            var updatedSM = await _clientUserProcess.UpdateClientUser(id, innerReq);
            if (updatedSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(updatedSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Add/Update Endpoints

        #region --Add/Update Profile-Picture Endpoints--

        [HttpPost("mine/ProfilePicture")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> AddUpdateMineProfilePicture([FromBody] ApiRequest<string> apiRequest)
        {
            #region Check Request
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }



            #endregion Check Request

            var addedSM = await _clientUserProcess.AddUpdateMineProfilePicture(innerReq, currentUserId);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(GetById), new
                {
                    id = currentUserId
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("mine/ProfilePicture")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> GetMineClientUserProfilePicture()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            var imageResp = await _clientUserProcess.GetMineProfilePicture(currentUserRecordId, roleType);
            return Ok(ModelConverter.FormNewSuccessResponse(imageResp));
        }

        #endregion --Add/Update Mine-Profile-Picture Endpoints--

        #region --Add/Update My-Profile-Picture Endpoints--

        [HttpPost("my/ClientUserProfilePicture")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<string>>> AddUpdateMyClientUserProfilePicture([FromBody] ApiRequest<string> apiRequest, int empId)
        {
            #region Check Request
            int currentUserId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }
            #endregion Check Request

            var addedSM = await _clientUserProcess.AddUpdateMyClientProfilePicture(innerReq, currentUserId, empId);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(GetById), new
                {
                    id = currentUserId
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion

        #region --My & Mine Delete-Profile-Picture EndPoint--

        [HttpDelete("my/DeleteProfilePicture/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyProfilePicture(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientUserProcess.DeleteUserProfilePictureById(id, currentCompanyId);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --My & Mine Delete-Profile-Picture EndPoint--

        #region Delete Endpoints

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientUserProcess.DeleteClientUserById(id);
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteUserProfilePicture()
        {
            #region Check Request

            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyId <= 0)
            { return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims)); }

            #endregion Check Request

            var resp = await _clientUserProcess.DeleteProfilePictureById(currentCompanyId, _environment.WebRootPath);
            if (resp != null && resp.DeleteResult)
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            else
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
        }

        #endregion Delete Endpoints

        #region --My Delete Endpoint--

        [HttpDelete("my/ClientUser/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyEmployee(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientUserProcess.DeleteMyClientUserById(id, currentCompanyId);
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

        #region --Report--

        [HttpPost("ClientUsersReportCount")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetUserReportCountsResponse([FromBody] ApiRequest<BaseReportFilterSM> apiRequest)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            var countResp = await _clientUserProcess.GetUserReportCount(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));

        }

        [HttpPost("my/ClientUsersReport/{skip}/{top}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserSM>>>> GetAllClientUsersReport([FromBody] ApiRequest<BaseReportFilterSM> apiRequest, int skip, int top)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientUserProcess.GetAllClientUsersReport(innerReq, currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpPost("my/ClientUsersReportBasedOnEmployeeStatus")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserSM>>>> GetAllClientUsersReportBasedOnStatus(EmployeeStatusSM employeeStatusSM)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientUserProcess.GetAllClientUsersReportBasedOnStatus(employeeStatusSM, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion --Report--

        #region --Change Password--

        [HttpGet("ChangePassword")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<UpdateCredentialSM>>> ChangePassword()
        {
            var currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            var resp = await _clientUserProcess.ChangePassword(currentUserId);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }


        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> ChangePassword([FromBody] ApiRequest<UpdateCredentialSM> apiRequest)
        {
            var currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }
            var resp = await _clientUserProcess.ChangePassword(innerReq, currentUserId);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Change Password--

        #region --DashBoard--

        [HttpGet("DashBoardDetails")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserSM>>> GetDashBoardDetails()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientUserProcess.GetDashBoardDetails(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));

        }

        #endregion --DashBoard--

    }
}
