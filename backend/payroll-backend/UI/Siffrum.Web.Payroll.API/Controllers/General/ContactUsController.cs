using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.BAL.General;
using Siffrum.Web.Payroll.ServiceModels.v1.General;

namespace Siffrum.Web.Payroll.API.Controllers.General
{
    [Route("api/v1/[controller]")]
    public class ContactUsController : ApiControllerWithOdataRoot<ContactUsSM>
    {
        #region Properties
        private readonly ContactUsProcess _contactUsProcess;
        #endregion Properties

        #region Constructor
        public ContactUsController(ContactUsProcess contactUsProcess) : base(contactUsProcess)
        {
            _contactUsProcess = contactUsProcess;
        }
        #endregion Constructor

        #region Odata EndPoints
        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ContactUsSM>>>> GetAsOdata(ODataQueryOptions<ContactUsSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Count

        [HttpGet]
        [Route("ContactUsCountResponse")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "CompanyAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetContactUsCountsResponse()
        {
            var countResp = await _contactUsProcess.GetAllContactUsCountResponse();
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        #endregion Count

        #region GetAll Endpoint
        [HttpGet]
        //[Authorize(AuthenticationSchemes =RenoBearerTokenAuthHandlerRoot.DefaultSchema , Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ContactUsSM>>>> GetAll()
        {
            var contactUsSM = await _contactUsProcess.GetAllContactUs();
            return Ok(ModelConverter.FormNewSuccessResponse(contactUsSM));
        }
        #endregion GetAll Endpoint

        #region Get Single Endpoint
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<ContactUsSM>>> GetById(int id)
        {
            var singleFeatureSM = await _contactUsProcess.GetSingleContactUsById(id);
            if (singleFeatureSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleFeatureSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Get Single Endpoint

        #region Add Endpoint
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ContactUsSM>>> Post([FromBody] ApiRequest<ContactUsSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var contactUsSM = await _contactUsProcess.AddContactUs(innerReq);
            if (contactUsSM != null)
            {
                return CreatedAtAction(nameof(ContactUsController.GetById), new
                {
                    id = contactUsSM.Id
                }, ModelConverter.FormNewSuccessResponse(contactUsSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Add Endpoint

        #region Delete Endpoint
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _contactUsProcess.DeleteContactUsById(id);
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
    }
}
