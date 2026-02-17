using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public class PermissionController : ApiControllerWithOdataRoot<PermissionSM>
    {
        #region Properties
        private readonly PermissionProcess _permissionProcess;

        #endregion Properties

        #region Constructor
        public PermissionController(PermissionProcess permissionProcess)
            : base(permissionProcess)
        {
            _permissionProcess = permissionProcess;
        }

        #endregion Constructor

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetAsOdata(ODataQueryOptions<PermissionSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);

            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints


        #region Get Endpoints

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetMyModulePermissionsBasedOnLicenseType()
        {
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            if (string.IsNullOrWhiteSpace(roleTypes))
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            var listSM = await _permissionProcess.GetActiveCompanyPermissionByUserId(companyCode, currentUserId, roleType);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("my/{moduleName}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetMyModulePermissionsBasedOnLicenseType(ModuleNameSM moduleName)
        {
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            if (string.IsNullOrWhiteSpace(roleTypes))
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            var listSM = await _permissionProcess.GetActiveCompanyPermissionByUserIdAndModuleName(companyCode, currentUserId, roleType, moduleName);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }
        #endregion


    }
}
