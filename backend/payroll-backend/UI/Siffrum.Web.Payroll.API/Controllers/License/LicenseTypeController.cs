using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.License;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.API.Controllers.License
{
    [Route("api/v1/[controller]")]
    public class LicenseTypeController : ApiControllerWithOdataRoot<LicenseTypeSM>
    {
        #region Properties
        private readonly LicenseTypeProcess _licenseTypeProcess;
        #endregion Properties

        #region Constructor
        public LicenseTypeController(LicenseTypeProcess licenseTypeProcess) : base(licenseTypeProcess)
        {
            _licenseTypeProcess = licenseTypeProcess;
        }
        #endregion Constructor

        #region Odata EndPoints
        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LicenseTypeSM>>>> GetAsOdata(ODataQueryOptions<LicenseTypeSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region GetById With Feature List Endpoint
        [HttpGet("extended/{id}")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<LicenseTypeSM>>> GetByIdExtended(int id)
        {
            var LicenseTypeSM = await _licenseTypeProcess.GetSingleFeatureGroupExtendedById(id);
            return Ok(ModelConverter.FormNewSuccessResponse(LicenseTypeSM));
        }
        #endregion GetAll With Feature List Endpoint

        #region GetAll Endpoint
        [HttpGet]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LicenseTypeSM>>>> GetAll()
        {
            var LicenseTypeListSM = await _licenseTypeProcess.GetAllLicenseTypes();
            return Ok(ModelConverter.FormNewSuccessResponse(LicenseTypeListSM));
        }

        [HttpGet("AllLicenseTypes")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CompanyLicenseDetailsSM>>>> GetAllLicenses()
        {
            var userLicenseDetailsListsSM = await _licenseTypeProcess.GetAllLicenses();
            return Ok(ModelConverter.FormNewSuccessResponse(userLicenseDetailsListsSM));
        }

        #endregion GetAll Endpoint

        #region Get Single Endpoint
        [HttpGet("{id}")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<LicenseTypeSM>>> GetById(int id)
        {
            var singleLicenseTypeSM = await _licenseTypeProcess.GetSingleFeatureGroupById(id);
            if (singleLicenseTypeSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleLicenseTypeSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Get Single Endpoint

        #region My (Get) Endpoint
        [HttpGet("my")]
        public async Task<ActionResult<ApiResponse<LicenseTypeSM>>> GetMyLicenses()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            if (currentCompanyId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetById(currentCompanyId);
        }
        #endregion My (Get) Endpoint

        #region --COUNT--

        [HttpGet]
        [Route("FeatureGroupCountResponse")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetFeatureGroupCountsResponse()
        {
            var countResp = await _licenseTypeProcess.GetAllLicenseTypeCountResponse();
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        #endregion --COUNT--

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<LicenseTypeSM>>> Post([FromBody] ApiRequest<LicenseTypeSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _licenseTypeProcess.AddLicensetype(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(LicenseTypeController.GetById), new
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<LicenseTypeSM>>> Put(int id, [FromBody] ApiRequest<LicenseTypeSM> apiRequest)
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

            var resp = await _licenseTypeProcess.UpdateLicenseType(id, innerReq);
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "SuperAdmin,ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _licenseTypeProcess.DeleteLicenseTypeById(id);
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

    }
}
