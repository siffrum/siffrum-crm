using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public class ClientUserAddressController : ApiControllerWithOdataRoot<ClientUserAddressSM>
    {
        private readonly ClientUserAddressProcess _clientUserAddressProcess;
        public ClientUserAddressController(ClientUserAddressProcess clientUserAddressProcess)
            : base(clientUserAddressProcess)
        {
            _clientUserAddressProcess = clientUserAddressProcess;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,SuperAdmin,SystemAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserAddressSM>>>> GetAsOdata(ODataQueryOptions<ClientUserAddressSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserAddressSM>>>> GetAll()
        {
            var listSM = await _clientUserAddressProcess.GetAllClientUserAddresss();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserAddressSM>>> GetById(int id)
        {
            var singleSM = await _clientUserAddressProcess.GetClientUserAddressById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Get Endpoints

        #region --COUNT--

        [HttpGet]
        [Route("ClientUserAddressCountsResponse/{empId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetClientUserAddressCountsResponse(int empId)
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var countResp = await _clientUserAddressProcess.GetClientUserAddressCountsResponse(empId, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));
        }

        #endregion --COUNT--

        #region --My-EndPoints

        [HttpGet("my/ClientUserAddressByEmployeeID/{empId}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserAddressSM>>>> GetClientUserAddressByEmployeeIdOfMyCompany(int empId)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientUserAddressProcess.GetClientUsersAddressByEmployeeIdOfMyCompany(currentCompanyId, empId);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("my/ClientUserAddressById/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserAddressSM>>> GetMyClientUserAddressById(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var singleSM = await _clientUserAddressProcess.GetClientUserAddressByIdOfMyCompany(id, currentCompanyId);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpGet("my/AllEmployeesAddress")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientUserAddressSM>>>> GetAllEmployeesAddressOfMyCompany()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientUserAddressProcess.GetEmployeesAddressOfMyCompany(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("mine/Address")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserAddressSM>>> GetMineClientUserAddressDetails()
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstants.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            return await GetById(currentUserRecordId);
        }

        #endregion

        #region Add/Update Endpoints

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientUserAddressSM>>> Post([FromBody] ApiRequest<ClientUserAddressSM> apiRequest)
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

            var subM = await _clientUserAddressProcess.AddClientUserAddress(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientUserAddressController.GetById), new
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
        public async Task<ActionResult<ApiResponse<ClientUserAddressSM>>> Put(int id, [FromBody] ApiRequest<ClientUserAddressSM> apiRequest)
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

            var resp = await _clientUserAddressProcess.UpdateClientUserAddress(id, innerReq);
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
            var resp = await _clientUserAddressProcess.DeleteClientUserAddressById(id);
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

        [HttpDelete("my/EmployeeAddress/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyEmployeeAddress(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientUserAddressProcess.DeleteMyClientUserAddressById(id, currentCompanyId);
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

    }
}
