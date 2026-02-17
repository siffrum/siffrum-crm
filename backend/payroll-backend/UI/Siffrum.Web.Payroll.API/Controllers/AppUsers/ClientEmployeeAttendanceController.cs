using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.API.Controllers.AppUsers
{
    [Route("api/v1/[controller]")]
    public class ClientEmployeeAttendanceController : ApiControllerWithOdataRoot<ClientEmployeeAttendanceSM>
    {
        private readonly ClientEmployeeAttendanceProcess _clientEmployeeAttendanceProcess;
        public ClientEmployeeAttendanceController(ClientEmployeeAttendanceProcess clientEmployeeAttendanceProcess)
            : base(clientEmployeeAttendanceProcess)
        {
            _clientEmployeeAttendanceProcess = clientEmployeeAttendanceProcess;
        }

        #region Odata EndPoints

        [HttpGet]
        [Route("odata")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceSM>>>> GetAsOdata(ODataQueryOptions<ClientEmployeeAttendanceSM> oDataOptions)
        {
            //TODO: validate inputs here probably 
            var retList = await base.GetAsEntitiesOdata(oDataOptions);
            return Ok(ModelConverter.FormNewSuccessResponse(retList));
        }

        #endregion Odata EndPoints

        #region --Get-EndPoints--

        [HttpGet]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceSM>>>> GetAll()
        {
            var listSM = await _clientEmployeeAttendanceProcess.GetAllClientEmployeeAttendance();
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAttendanceSM>>> GetById(int id)
        {
            var singleSM = await _clientEmployeeAttendanceProcess.GetClientEmployeeAttendanceById(id);
            if (singleSM != null)
            {
                return ModelConverter.FormNewSuccessResponse(singleSM);
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        //[HttpGet("Employee/{empId}")]
        //[Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceSM>>>> GetByEmpId(int empId)
        //{
        //    var listSM = await _clientEmployeeAttendanceProcess.GetClientEmployeeAttendanceDetailByEmpId(empId);
        //    if (listSM != null)
        //    {
        //        return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        //    }
        //    else
        //    {
        //        return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
        //    }
        //}

        #endregion --Get-EndPoints--

        #region --Count--

        [HttpPost("AttendanceReportCount")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IntResponseRoot>>> GetAttendanceReportCountsResponse([FromBody] ApiRequest<EmployeeAttendanceReportRequestSM> apiRequest)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            var countResp = await _clientEmployeeAttendanceProcess.GetAttendanceReportCount(innerReq, currentCompanyId);
            return Ok(ModelConverter.FormNewSuccessResponse(new IntResponseRoot(countResp)));

        }

        #endregion --Count--

        #region --My/Mine-Get-EndPoints--

        //[HttpGet("my/EmployeeAttendanceDetail/{empId}")]
        //[Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceSM>>>> GetEmployeesAttendanceByEmployeeIdOfMyCompany(int empId)
        //{
        //    int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
        //    var listSM = await _clientEmployeeAttendanceProcess.GetClientEmployeeAttendanceByEmpoyeeId(currentCompanyId, empId);
        //    return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        //}

        [HttpPost("mine/EmployeeAttendanceDetail/{dateTime}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceSM>>>> GetMineEmployeeAttendanceDetails(DateTime dateTime)
        {
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAttendanceProcess.GetMineClientEmployeeAttendanceDetailByEmpId(currentUserRecordId, dateTime);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("my/EmployeeAttendanceDetail/{empId}/{dateTime}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceSM>>>> GetMyEmployeeAttendanceDetails(int empId, DateTime dateTime)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAttendanceProcess.GetMyClientEmployeeAttendanceDetailByEmpId(empId, currentCompanyId, dateTime);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --My/Mine-Get-EndPoints--

        #region --Reports--

        [HttpPost("my/EmployeeAttendanceReport{skip}/{top}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeAttendanceExtendedUserSM>>>> GetAllEmployeeAttendanceReport([FromBody] ApiRequest<EmployeeAttendanceReportRequestSM> apiRequest, int skip, int top)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAttendanceProcess.GetTotalEmployeesAttendanceReport(innerReq, currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        [HttpPost("my/EmployeeAttendanceReportByClientUserId/{skip}/{top}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientEmployeeLeaveExtendedUserSM>>>> GetEmployeeAttendanceReportByClientUserId([FromBody] ApiRequest<EmployeeAttendanceReportRequestSM> apiRequest, int skip, int top)
        {
            #region Check Request
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var listSM = await _clientEmployeeAttendanceProcess.GetEmployeesAttendanceReportByClientId(innerReq, currentCompanyId, skip, top);
            return Ok(ModelConverter.FormNewSuccessResponse(listSM));
        }

        #endregion --Reports--

        #region --Add/Update--

        [HttpPost("CheckIn")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAttendanceSM>>> Post([FromBody] ApiRequest<ClientEmployeeAttendanceSM> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            int currentUserId = User.GetUserRecordIdFromCurrentUserClaims();
            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            apiRequest.ReqData.ClientUserId = currentUserId;
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _clientEmployeeAttendanceProcess.AddClientEmployeeAttendanceOnCheckIn(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientEmployeeAttendanceController.GetById), new
                {
                    id = subM.Id
                }, ModelConverter.FormNewSuccessResponse(subM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("CheckOut/{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAttendanceSM>>> Put(int id, [FromBody] ApiRequest<ClientEmployeeAttendanceSM> apiRequest)
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

            var resp = await _clientEmployeeAttendanceProcess.UpdateClientEmployeeAttendance(id, innerReq);
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

        #region --Mine-Add/Update--

        [HttpPost("mine/CheckIn")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAttendanceSM>>> MineCheckInAttendance([FromBody] ApiRequest<ClientEmployeeAttendanceSM> apiRequest)
        {
            #region Check Request
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();

            apiRequest.ReqData.ClientCompanyDetailId = currentCompanyId;
            apiRequest.ReqData.ClientUserId = currentUserRecordId;

            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _clientEmployeeAttendanceProcess.AddClientEmployeeAttendanceOnCheckIn(innerReq);
            if (subM != null)
            {
                return CreatedAtAction(nameof(ClientEmployeeAttendanceController.GetById), new
                {
                    id = subM.Id
                }, ModelConverter.FormNewSuccessResponse(subM));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPut("mine/CheckOut/{id}")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientEmployeeAttendanceSM>>> MineCheckOutAttendance(int id, [FromBody] ApiRequest<ClientEmployeeAttendanceSM> apiRequest)
        {
            #region Check Request
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();

            apiRequest.ReqData.ClientUserId = currentUserRecordId;
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

            var resp = await _clientEmployeeAttendanceProcess.UpdateClientEmployeeAttendance(id, innerReq);
            if (resp != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        #endregion --Mine-Add/Update--

        #region Delete Endpoints

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> Delete(int id)
        {
            var resp = await _clientEmployeeAttendanceProcess.DeleteClientEmployeeAttendance(id);
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

        [HttpDelete("my/EmployeeAttendance/{id}")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<DeleteResponseRoot>>> DeleteMyEmployeeAttendance(int id)
        {
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var resp = await _clientEmployeeAttendanceProcess.DeleteMyClientEmployeeAttendanceById(id, currentCompanyId);
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

        #region Reading Excel File

        //[HttpPost("GetExcel")]
        //public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetExcelFile([FromBody] ApiRequest<string> apiRequest)
        //{
        //    var innerRequest = apiRequest?.ReqData;
        //    if (String.IsNullOrEmpty(innerRequest))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        //byte[] xlsFileBytes = System.IO.File.ReadAllBytes(folderPath.SearchString);
        //        byte[] excelBytes = Convert.FromBase64String(innerRequest);
        //        using (MemoryStream stream = new MemoryStream(excelBytes))
        //        {
        //            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
        //            {
        //                WorkbookPart workbookPart = doc.WorkbookPart;
        //                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
        //                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

        //                List<object> data = new List<object>();
        //                bool isFirstRow = true;

        //                foreach (Row r in sheetData.Elements<Row>())
        //                {
        //                    //ArrayList rowData = new ArrayList();
        //                    List<object> rowData = new List<object>();

        //                    foreach (Cell c in r.Elements<Cell>())
        //                    {
        //                        if (c.DataType != null && c.DataType == CellValues.SharedString)
        //                        {
        //                            var stringId = Convert.ToInt32(c.InnerText);
        //                            string cellValue = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;

        //                            if (isFirstRow)
        //                            {
        //                                // Treat cell value as column heading
        //                                rowData.Add(cellValue);
        //                            }
        //                            else
        //                            {
        //                                // Treat cell value as data value
        //                                rowData.Add(cellValue);
        //                            }
        //                        }
        //                    }

        //                    if (!isFirstRow)
        //                    {
        //                        data.Add(rowData);
        //                    }

        //                    isFirstRow = false;
        //                }
        //                var targetObject = data.FirstOrDefault();

        //                return Ok(ModelConverter.FormNewSuccessResponse(targetObject));
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// this function is used for getting the all heading rows of a table.
        /// </summary>
        /// <returns>the list of strings of heading row from a excel file</returns>

        [HttpPost("AttendanceHeadingRow")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientExcelFileResponseSM>>> GetAttendanceHeadingRow([FromBody] ApiRequest<ClientExcelFileRequestSM> excelFileRequest)
        {
            var innerReq = excelFileRequest?.ReqData;
            var listSM = await _clientEmployeeAttendanceProcess.GetAttendanceHeadingRowFromExcelFile(innerReq);
            if (listSM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }

        [HttpPost("AddEmployeeAttendanceDataFromExcel")]
        [Authorize(AuthenticationSchemes = RenoBearerTokenAuthHandlerRoot.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientExcelFileSummarySM>>> AddEmployeeAttendanceDataFromExcel([FromBody] ApiRequest<ClientExcelFileResponseSM> clientExcelFileResponseSM)
        {
            var currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = clientExcelFileResponseSM?.ReqData;
            var listSm = await _clientEmployeeAttendanceProcess.AddAttendanceDataFromExcel(innerReq, currentCompanyId);
            if (listSm != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(listSm));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }

        }

        [HttpPost("AddAttendanceDataFromSummary")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin,ClientEmployee")]
        public async Task<ActionResult<ApiResponse<ClientExcelFileSummarySM>>> AddAttendanceDataFromSummary([FromBody] ApiRequest<IEnumerable<ClientEmployeeAttendanceExtendedUserSM>> apiRequest)
        {
            #region Check Request
            int currentCompanyId = User.GetCompanyRecordIdFromCurrentUserClaims();
            var innerReq = apiRequest?.ReqData;
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }

            #endregion Check Request

            var subM = await _clientEmployeeAttendanceProcess.AddAttendanceDataFromSummary((List<ClientEmployeeAttendanceExtendedUserSM>)innerReq, currentCompanyId);
            if (subM != null)
            {
                return Ok(ModelConverter.FormNewSuccessResponse(subM));
            }
            else
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }


        #endregion Reading Excel File

    }
}
