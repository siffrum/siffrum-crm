using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.Client
{
    [Route("api/v1/[controller]")]
    public class SqlReportController : ApiControllerWithOdataRoot<SQLReportMasterSM>
    {
        private readonly SqlReportProcess _sqlReportProcess;
        public SqlReportController(SqlReportProcess sqlReportProcess)
            : base(sqlReportProcess)
        {
            _sqlReportProcess = sqlReportProcess;
        }
        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SQLReportMasterSM>>>> GetAsOdata(ODataQueryOptions<SQLReportMasterSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region Get Endpoints

        [HttpGet]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SQLReportMasterSM>>>> GetAllSqlReports()
        {
            var listSM = await _sqlReportProcess.GetAllSqlReport();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportMasterSM>>> GetById(int id)
        {
            var singleSM = await _sqlReportProcess.GetSqlReportsById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        /*[HttpGet("SelectReportsById/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportDataModelSM>>> GetSelectReportById(int id, int pageNo)
        {
            try
            {
                int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();

                var singleSM = await _sqlReportProcess.GetSelectSqlReportsById(id, currentCompanyId, pageNo);
                if (singleSM != null)
                {
                    return ModelConverter.FormNewSuccessResponse(singleSM);
                }
                else
                {
                    return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"{ex.InnerException}", $"{ex.Message}");
            }
        }*/

        [HttpGet("SelectReportsById/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportResponseModel>>> GetSelectReportByIdAsync(int id, int pageNo)
        {
            try
            {
                int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();

                var singleSM = await _sqlReportProcess.GetSelectSqlReportsByIdAsync(id, currentCompanyId, pageNo);
                if (singleSM != null)
                {
                    return ModelConverter.FormNewSuccessResponse(singleSM);
                }
                else
                {
                    return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"{ex.InnerException}", $"{ex.Message}");
            }
        }

        /*[HttpGet("SqlReports")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportDataModelSM>>> GetSqlReports(string query)
        {
            int? currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            if (roleType == RoleTypeSM.SuperAdmin)
            {
                currentCompanyId = null;
            }
            int pageNo = 1;
            var singleSM = await _sqlReportProcess.SqlReport(query, pageNo, currentCompanyId);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }*/

        [HttpGet("SqlReports")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportResponseModel>>> GetSqlReportsAsync(string query, int skip, int top)
        {
            int? currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            string roleTypes = User.GetUserRoleTypeFromCurrentUserClaims();
            RoleTypeSM roleType = (RoleTypeSM)Enum.Parse(typeof(RoleTypeSM), roleTypes);
            if (roleType == RoleTypeSM.SuperAdmin)
            {
                currentCompanyId = null;
            }
            var singleSM = await _sqlReportProcess.SqlReportAsync(query, skip,currentCompanyId, top);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Get EndPoints

        #region My EndPoints

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SQLReportMasterSM>>>> GetAllMySqlReports()
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _sqlReportProcess.GetMySqlReport(currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion My EndPoints

        #region Add/Update Methods

        [HttpPost]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportMasterSM>>> AddSqlReport([FromBody] ApiRequest<SQLReportMasterSM> apiRequest)
        {
            #region Check Request
            //int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            //apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _sqlReportProcess.AddSqlReport(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientCompanyAddressController.GetById), new
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
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<SQLReportMasterSM>>> UpdateSqlReport(int id, [FromBody] ApiRequest<SQLReportMasterSM> apiRequest)
        {
            #region Check Request
            //int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            //apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
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

            var resp = await _sqlReportProcess.UpdateSqlReport(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Add/Update Methods

        #region Delete Method

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteSqlReport(int id)
        {
            var resp = await _sqlReportProcess.DeleteSqlReportById(id);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion Delete Method

        #region My Delete Methods

        [HttpDelete("my/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _sqlReportProcess.DeleteMySqlReportById(id, currentCompanyId);
            if (resp != null && resp.DeleteResult)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(resp?.DeleteMessage, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion My Delete Methods


    }
}
