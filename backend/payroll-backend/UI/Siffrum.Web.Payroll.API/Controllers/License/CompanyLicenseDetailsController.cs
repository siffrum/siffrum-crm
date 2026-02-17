using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.License;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.API.Controllers.License
{
    [Route("api/v1/[controller]")]
    public class CompanyLicenseDetailsController : ApiControllerWithOdataRoot<CompanyLicenseDetailsSM>
    {
        #region Properties

        private readonly CompanyLicenseDetailsProcess _companyLicenseDetailsProcess;

        #endregion Properties

        #region Constructor
        public CompanyLicenseDetailsController(CompanyLicenseDetailsProcess companyLicenseDetailsProcess)
            : base(companyLicenseDetailsProcess)
        {
            _companyLicenseDetailsProcess = companyLicenseDetailsProcess;
        }


        #endregion Constructor

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CompanyLicenseDetailsSM>>>> GetAsOdata(ODataQueryOptions<CompanyLicenseDetailsSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region GetAll Endpoint

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CompanyLicenseDetailsSM>>>> GetAll()
        {
            var userLicenseDetailsListsSM = await _companyLicenseDetailsProcess.GetAllCompanyLicenseDetails();
            return Ok(ModelConverter.FormNewSuccessResponse(userLicenseDetailsListsSM));
        }


        #endregion GetAll Endpoint

        #region Get Single Endpoint

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> GetById(int id)
        {
            var singleCompanyLicenseDetailsSM = await _companyLicenseDetailsProcess.GetUserSubscriptionById(id);
            if (singleCompanyLicenseDetailsSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleCompanyLicenseDetailsSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Get Single Endpoint

        #region Add/Update EndPoint

        [HttpPost]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> Post([FromBody] ApiRequest<CompanyLicenseDetailsSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var userLicesneDetailsSM = await _companyLicenseDetailsProcess.AddUserSubscription(innerReq);
            if (userLicesneDetailsSM != null)
            {
                return CreatedAtAction(nameof(CompanyLicenseDetailsController.GetById), new
                {
                    id = userLicesneDetailsSM.Id
                }, ModelConverter.FormNewSuccessResponse(userLicesneDetailsSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> Put(int id, [FromBody] ApiRequest<CompanyLicenseDetailsSM> apiRequest)
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

            var resp = await _companyLicenseDetailsProcess.UpdateUserSubscription(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Add/Update EndPoint

        #region Delete Endpoint

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _companyLicenseDetailsProcess.DeleteUserSubscriptionById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Delete Endpoint

        //#region Mine-License

        //[HttpGet("mine/ActiveTrialLicense")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> GetMineActiveTrialLicense()
        //{
        //    int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
        //    if (currentUserRecordId <= 0)
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
        //    }
        //    CompanyLicenseDetailsSM? singleCompanyLicenseDetailsSM = await _companyLicenseDetailsProcess.GetActiveTrialCompanyLicenseDetailsByUserId(currentUserRecordId);
        //    return ModelConverter.FormNewSuccessResponse(singleCompanyLicenseDetailsSM);
        //}

        //[HttpGet("mine/Active")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> GetMineActiveLicense()
        //{
        //    int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
        //    if (currentUserRecordId <= 0)
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
        //    }
        //    CompanyLicenseDetailsSM? singleCompanyLicenseDetailsSM = await _companyLicenseDetailsProcess.GetActiveCompanyLicenseDetailsByUserId(currentUserRecordId);
        //    return ModelConverter.FormNewSuccessResponse(singleCompanyLicenseDetailsSM);
        //}

        //[HttpGet("mine/ModulePermissionsBasedOnLicenseType")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetMineModulePermissionsBasedOnLicenseType()
        //{
        //    string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
        //    RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
        //    int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
        //    var listSM = await _companyLicenseDetailsProcess.GetActiveCompanyLicenseDetailsPermissionByUserId(currentUserId, roleType);
        //    return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        //}

        //#endregion Mine-License

        #region My-License

        [HttpGet("my/Active")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> GetMyActiveLicense()
        {
            string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
            if (companyCode == null)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            CompanyLicenseDetailsSM? singleCompanyLicenseDetailsSM = await _companyLicenseDetailsProcess.GetActiveLicenseDetailsByCompanyCode(companyCode);
            return ModelConverter.FormNewSuccessResponse(singleCompanyLicenseDetailsSM);
        }

        //[HttpGet("my/ModulePermissionsBasedOnLicenseType")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<PermissionSM>>>> GetMyModulePermissionsBasedOnLicenseType()
        //{
        //    string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
        //    if (string.IsNullOrWhiteSpace(roleTypes))
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
        //    }
        //    string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
        //    if (string.IsNullOrWhiteSpace(companyCode))
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
        //    }
        //    RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
        //    int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
        //    if (currentUserId <= 0)
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
        //    }
        //    var listSM = await _companyLicenseDetailsProcess.GetActiveCompanyLicenseDetailsPermissionByUserId(companyCode, currentUserId, roleType);
        //    return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        //}

        [HttpPost("my/StartTrial")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> StartMyTrialLicense()
        {
            string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
            var userLicenseDetailsSM = await _companyLicenseDetailsProcess.AddTrialLicenseDetails(companyCode);
            if (userLicenseDetailsSM != null)
            {
                return CreatedAtAction(nameof(CompanyLicenseDetailsController.GetById), new
                {
                    id = userLicenseDetailsSM.Id
                }, ModelConverter.FormNewSuccessResponse(userLicenseDetailsSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("my/UpdateTrial")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> UpdateMyTrialLicense()
        {
            int userId = User.GetUserRecordIdFromCurrentUserClaims();

            var userLicenseDetailsSM = await _companyLicenseDetailsProcess.UpdateTrialLicenseStatus(userId);
            if (userLicenseDetailsSM != null)
            {
                return CreatedAtAction(nameof(CompanyLicenseDetailsController.GetById), new
                {
                    id = userLicenseDetailsSM.Id
                }, ModelConverter.FormNewSuccessResponse(userLicenseDetailsSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }


        #endregion My-License

        //#region Trial

        //[HttpPost("mine/AddTrial")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> AddMineTrialLicense()
        //{
        //    int userId = User.GetUserRecordIdFromCurrentUserClaims();

        //    var userLicenseDetailsSM = await _companyLicenseDetailsProcess.AddTrialLicenseDetails(userId);
        //    if (userLicenseDetailsSM != null)
        //    {
        //        return CreatedAtAction(nameof(CompanyLicenseDetailsController.GetById), new
        //        {
        //            id = userLicenseDetailsSM.Id
        //        }, ModelConverter.FormNewSuccessResponse(userLicenseDetailsSM));
        //    }
        //    else
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
        //    }
        //}

        //[HttpPut("mine/UpdateTrial")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SuperAdmin,SystemAdmin")]
        //public async Task<ActionResult<ApiResponse<CompanyLicenseDetailsSM>>> UpdateMineTrialLicense()
        //{
        //    int userId = User.GetUserRecordIdFromCurrentUserClaims();

        //    var userLicenseDetailsSM = await _companyLicenseDetailsProcess.UpdateTrialLicenseStatus(userId);
        //    if (userLicenseDetailsSM != null)
        //    {
        //        return CreatedAtAction(nameof(CompanyLicenseDetailsController.GetById), new
        //        {
        //            id = userLicenseDetailsSM.Id
        //        }, ModelConverter.FormNewSuccessResponse(userLicenseDetailsSM));
        //    }
        //    else
        //    {
        //        return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
        //    }
        //}

        //#endregion Trial

    }
}
