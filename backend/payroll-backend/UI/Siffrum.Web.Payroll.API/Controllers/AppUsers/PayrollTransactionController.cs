using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public class PayrollTransactionController : ApiControllerWithOdataRoot<PayrollTransactionSM>
    {
        private readonly PayrollTransactionProcess _payrollTransactionProcess;
        public PayrollTransactionController(PayrollTransactionProcess payrollTransactionProcess)
            : base(payrollTransactionProcess)
        {
            _payrollTransactionProcess = payrollTransactionProcess;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PayrollTransactionSM>>>> GetAsOdata(ODataQueryOptions<PayrollTransactionSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PayrollTransactionSM>>>> GetAll()
        {
            var listSM = await _payrollTransactionProcess.GetAllPaymentGenerations();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<PayrollTransactionSM>>> GetById(int id)
        {
            var singleSM = await _payrollTransactionProcess.GetPayrollTransactionsById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotFound, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Get Endpoints

        #region --My-EndPoints--
        [HttpPost("my/AllPayrollTransactionsBasedonDateTime")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeLeaveSM>>>> GetAllPayrollTransactionsOfMyCompany(DateTime dateTime, int skip, int top)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _payrollTransactionProcess.GetAllPayrollTransactionsOfMyCompany(currentCompanyId, dateTime, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }
        #endregion --My-EndPoints--

        #region --Count--

        [HttpGet("PayrollTransactionCountResponse")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetPayrollTransactionCountsResponse(DateTime dateTime)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _payrollTransactionProcess.GetPayrollTransactionCounts(currentCompanyId, dateTime);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));

        }

        [HttpPost("PayrollTransactionReportCount")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetPayrollTransactionReportCountResponse([FromBody] ApiRequest<PayrollTransactionReportSM> payrollTransactionReportSM)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = payrollTransactionReportSM?.ReqData;
            var countResp = await _payrollTransactionProcess.GetPayrollReportCount(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));

        }

        #endregion --Count--

        #region Add/Update Endpoints

        [HttpPost("GeneratePayroll")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<PayrollTransactionSM>>> Post([FromBody] ApiRequest<PayrollTransactionSM> apiRequest)
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

            var subM = await _payrollTransactionProcess.AddGeneratePayroll(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(PayrollTransactionController.GetById), new
                {
                    id = subM.Id
                }, ModelConverter.FormNewSuccessResponse(subM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("GenerateAllPayrolls")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PayrollTransactionSM>>>> GenerateAllPayroll([FromBody] ApiRequest<IEnumerable<PayrollTransactionSM>> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            foreach (var item in apiRequest.ReqData)
            {
                item.ClientCompanyDetailId = currentCompanyId;
                apiRequest.ReqData.ToList().Add(item);
            }
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _payrollTransactionProcess.AddGenerateAllPayroll((List<PayrollTransactionSM>)innerReq);
            return Ok(ModelConverter.FormNewSuccessResponse(subM));
        }

        [HttpPost("AllPayrollTransactionsBasedonDateTime")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<IEnumerable<GeneratePayrollTransactionSM>>> GetAllPayrollTransactions(DateTime dateTime)
        {
            var listSM = await _payrollTransactionProcess.GetTotalPayrollTransactions(dateTime);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpPost("my/AllPayrollTransactionReport")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<IEnumerable<GeneratePayrollTransactionSM>>> GetAllPayrollTransactionReport([FromBody] ApiRequest<PayrollTransactionReportSM> apiRequest, int skip, int top)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _payrollTransactionProcess.GetTotalPayrollTransactionReport(innerReq, currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpPost("my/PayrollTransactionReportByUserId")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeLeaveExtendedUserSM>>>> GetPayrollTransactionReportByUserId([FromBody] ApiRequest<PayrollTransactionReportSM> apiRequest)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _payrollTransactionProcess.GetTotalPayrollTransactionReportByUserId(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion Add/Update Endpoints

        #region Delete Endpoints

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _payrollTransactionProcess.DeletePayrollTransaction(id);
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

        #region --My Delete Endpoint--

        [HttpDelete("my/LetterDocument/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyPayrollTransaction(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _payrollTransactionProcess.DeleteMyPayrollTransactionById(id, currentCompanyId);
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

        #region --Generate PaySlips--

        [HttpGet("my/PaySlips/{userId}/{dateTime}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> GetGeneratePaySlips(int userId, DateTime dateTime)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var payslip = await _payrollTransactionProcess.GeneratePaySlips(userId, currentCompanyId, dateTime);
            if (payslip != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(payslip));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Generate PaySlips--

    }
}
