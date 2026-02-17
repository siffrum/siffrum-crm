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
    public class ClientEmployeeBankDetailController : ApiControllerWithOdataRoot<ClientEmployeeBankDetailSM>
    {
        private readonly ClientEmployeeBankDetailProcess _clientEmployeeBankDetailProcess;
        public ClientEmployeeBankDetailController(ClientEmployeeBankDetailProcess clientEmployeeBankDetailProcess)
            : base(clientEmployeeBankDetailProcess)
        {
            _clientEmployeeBankDetailProcess = clientEmployeeBankDetailProcess;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeBankDetailSM>>>> GetAsOdata(ODataQueryOptions<ClientEmployeeBankDetailSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region CRUD

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeBankDetailSM>>>> GetAll()
        {
            var listSM = await _clientEmployeeBankDetailProcess.GetAllClientEmployeeBankDetails();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeBankDetailSM>>> GetById(int id)
        {
            var singleSM = await _clientEmployeeBankDetailProcess.GetClientEmployeeBankDetailById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("Employee/{empId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeBankDetailSM>>>> GetByEmpId(int empId)
        {
            var listSM = await _clientEmployeeBankDetailProcess.GetClientEmployeeBankDetailByEmpId(empId);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #region --COUNT--

        [HttpGet("ClientEmployeeBankDetailCountResponse/{empId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientEmployeeBankDetailCountsResponse(int empId)
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _clientEmployeeBankDetailProcess.GetClientEmployeeBankDetailCounts(empId, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        #endregion --COUNT--

        #endregion Get Endpoints

        #region --MY ENDPOINTS-- 

        [HttpGet("my/BankDetailId/{empId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeBankDetailSM>>>> GetEmployeesBankDetailByEmployeeIdOfMyCompany(int empId)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeBankDetailProcess.GetClientUsersBankDetailByEmployeeIdOfMyCompany(currentCompanyId, empId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("my/AllEmployeesBankDetails")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeBankDetailSM>>>> GetAllEmployeesBankDetailsOfMyCompany()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeBankDetailProcess.GetEmployeesBankDetailsOfMyCompany(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/BankDetails")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeBankDetailSM>>>> GetMineClientEmployeeBankDetails()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetByEmpId(currentUserRecordId);
        }

        #endregion --MY ENDPOINTS-- 

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeBankDetailSM>>> Post([FromBody] ApiRequest<ClientEmployeeBankDetailSM> apiRequest)
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

            var subM = await _clientEmployeeBankDetailProcess.AddClientEmployeeBankDetail(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientEmployeeBankDetailController.GetById), new
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeBankDetailSM>>> Put(int id, [FromBody] ApiRequest<ClientEmployeeBankDetailSM> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
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

            var resp = await _clientEmployeeBankDetailProcess.UpdateClientEmployeeBankDetail(id, innerReq);
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientEmployeeBankDetailProcess.DeleteClientEmployeeBankDetail(id);
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

        [HttpDelete("my/EmployeeBankDetail/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyEmployeeBankDetail(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientEmployeeBankDetailProcess.DeleteMyClientUserBankDetailById(id, currentCompanyId);
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

        #endregion CRUD

    }
}
