using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.License;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.API.Controllers.License
{
    [Route("api/v1/[controller]")]
    public class CompanyInvoiceController : ApiControllerWithOdataRoot<CompanyInvoiceSM>
    {
        #region Properties

        private readonly CompanyInvoiceProcess _companyInvoiceProcess;

        #endregion Properties

        #region Constructor
        public CompanyInvoiceController(CompanyInvoiceProcess companyInvoiceProcess) : base(companyInvoiceProcess)
        {
            _companyInvoiceProcess = companyInvoiceProcess;
        }
        #endregion Constructor

        #region Get All
        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CompanyInvoiceSM>>>> GetAll()
        {
            var listSM = await _companyInvoiceProcess.GetAllCompanyInvoices();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }
        #endregion Get All

        #region Get Single

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyInvoiceSM>>> GetById(int id)
        {
            var singleSM = await _companyInvoiceProcess.GetUserInvoiceById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Get Single

        #region Add

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyInvoiceSM>>> Post([FromBody] ApiRequest<CompanyInvoiceSM> apiRequest)
        {
            #region Check Request

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var addedSM = await _companyInvoiceProcess.AddUserInvoice(innerReq);
            if (addedSM != null)
            {
                return CreatedAtAction(nameof(CompanyInvoiceController.GetById), new
                {
                    id = addedSM.Id
                }, ModelConverter.FormNewSuccessResponse(addedSM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Add

        #region Put
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CompanyInvoiceSM>>> Put(int id, [FromBody] ApiRequest<CompanyInvoiceSM> apiRequest)
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

            var resp = await _companyInvoiceProcess.UpdateUserInvoice(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Put

        #region Delete
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _companyInvoiceProcess.DeleteUserInvoiceById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Delete

    }
}
