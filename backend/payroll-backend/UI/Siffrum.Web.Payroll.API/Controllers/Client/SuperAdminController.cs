using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.Client
{
    [Route("api/v1/[controller]")]
    public class SuperAdminController : ApiControllerWithOdataRoot<CompanyModulesSM>
    {
        private readonly SuperAdminProcess _superAdminProcess;
        private readonly PermissionProcess _permissionProcess;
        public SuperAdminController(SuperAdminProcess superAdminProcess, PermissionProcess permissionProcess)
            : base(superAdminProcess)
        {
            _superAdminProcess = superAdminProcess;
            _permissionProcess = permissionProcess;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CompanyModulesSM>>>> GetAsOdata(ODataQueryOptions<CompanyModulesSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region --Get--

        //[HttpGet("ModulesbyCompanyId/{Id}")]
        ////[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyModulesSM>>> GetById(int Id)
        //{
        //    var singleSM = await _superAdminProcess.GetCompanyModulesDetailsById(Id);
        //    return ModelConverter.FormNewSuccessResponse(singleSM);
        //}

        //[HttpGet("ModulesPermission")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetAll()
        //{
        //    var listSM = await _superAdminProcess.GetModules();
        //    return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        //}

        [HttpGet("CompanyModulesPermission/{companyId}/{roleType}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetPermissionByCompanyId(int companyId, RoleTypeSM roleType)
        {
            var listSM = await _permissionProcess.GetModulesPermissionByCompanyId(companyId, roleType);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("Modules")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetModules()
        {
            var listSM = await _superAdminProcess.GetGeneralModules();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        // This function is used when no module is available for company
        //[HttpGet("AllModules")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetAllModulesPermission()
        //{
        //    var listSM = await _superAdminProcess.GetAllModules();
        //    return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        //}

        [HttpGet("ModulePermissionsOfUser/{companyId}/{roleType}/{userId}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetModulePermissionOfUser(int companyId, RoleTypeSM roleType, int userId)
        {
            var listSM = await _permissionProcess.GetModulePermissionsByUserId(companyId, roleType, userId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/ModulePermissions")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetMineModulePermissions()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            var listSM = await _permissionProcess.GetModulePermissionsByUserId(currentCompanyId, roleType, currentUserId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/ModulePermissionsByModuleName/{moduleName}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetModulePermissionsByModuleName(ModuleNameSM moduleName)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            var listSM = await _permissionProcess.GetModulePermissionsByModuleName(currentCompanyId, roleType, moduleName, currentUserId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion--Get--

        #region --Get-Mine--

        //[HttpGet("mine/Modules")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyModulesSM>>> GetMineModules()
        //{

        //    int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
        //    if (currentCompanyId <= 0)
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
        //    }
        //    return await GetById(currentCompanyId);
        //}

        #endregion --Get-Mine--

        #region --Add/Update--

        //[HttpPost("Modules")]
        ////[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyModulesSM>>> Post([FromBody] ApiRequest<CompanyModulesSM> apiRequest)
        //{
        //    #region Check Request
        //    var innerReq = apiRequest?.ReqData;
        //    if (innerReq == null)
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
        //    }

        //    #endregion Check Request

        //    var addedSM = await _superAdminProcess.AddCompanyModules(innerReq);
        //    if (addedSM != null)
        //    {
        //        return CreatedAtAction(nameof(SuperAdminController.GetById), new
        //        {
        //            id = addedSM.Id
        //        }, ModelConverter.FormNewSuccessResponse(addedSM));
        //    }
        //    else
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
        //    }
        //}

        //[HttpPut("Modules/{Id}")]
        ////[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyModulesSM>>> Put(int Id, [FromBody] ApiRequest<CompanyModulesSM> apiRequest)
        //{
        //    #region Check Request
        //    var innerReq = apiRequest?.ReqData;
        //    if (innerReq == null)
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
        //    }

        //    if (Id <= 0)
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
        //    }

        //    #endregion Check Request

        //    var resp = await _superAdminProcess.UpdateCompanyModulesDetail(Id, innerReq);
        //    if (resp != null)
        //    {
        //        return Ok(ModelConverter.FormNewSuccessResponse(resp));
        //    }
        //    else
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
        //    }
        //}

        #region ADD/UPDATE MODULE PERMISSIONS

        [HttpPost("AddModulePermissions")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> AddModulePermissions([FromBody] ApiRequest<IEnumerable<PermissionSM>> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _superAdminProcess.AddModulesPermission((List<PermissionSM>)innerReq);
            return Ok(ModelConverter.FormNewSuccessResponse(subM));
        }

        [HttpPut("UpdateModulePemissions")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> UpdateModulePemissions([FromBody] ApiRequest<IEnumerable<PermissionSM>> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _superAdminProcess.UpdateModulesPermission((List<PermissionSM>)innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("UpdateModulePermissionsForUser")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<BoolResponseRoot>>> UpdateModulePermissionsForUser([FromBody] ApiRequest<IEnumerable<PermissionSM>> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _superAdminProcess.UpdateModulesForSingleUser((List<PermissionSM>)innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion ADD/UPDATE MODULE PERMISSIONS

        [HttpPut("CompanyAdminUserActivationSetting/{userId}/{loginStatus}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> PutAdminUserActivationSetting(int userId, LoginStatusSM loginStatus)
        {
            #region Check Request

            if (userId <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _superAdminProcess.UpdateAdminUserActivationSetting(userId, loginStatus);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("CompanyEnableOrDisable/{companyId}/{isEnabled}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> CompanyEnableOrDisable(int companyId, bool isEnabled)
        {
            #region Check Request

            if (companyId <= 0)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdInvalid, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var resp = await _superAdminProcess.UpdateCompanyStatusSetting(companyId, isEnabled);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Add/Update--

        #region --Delete--

        [HttpDelete("{id}")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _superAdminProcess.DeleteCompanyModuleById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Delete--

    }
}
