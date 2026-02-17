using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Constants;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientEmployeeAttendanceProcess : SiffrumPayrollBalOdataBase<ClientEmployeeAttendanceSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly ClientEmployeeLeaveProcess _clientEmployeeLeaveProcess;
        private readonly ClientUserProcess _clientUserProcess;

        #endregion --Properties--

        #region --Constructor--

        public ClientEmployeeAttendanceProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, ClientEmployeeLeaveProcess clientEmployeeLeaveProcess, ClientUserProcess clientUserProcess)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _clientEmployeeLeaveProcess = clientEmployeeLeaveProcess;
            _clientUserProcess = clientUserProcess;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientEmployeeAttendanceSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientEmployeeAttendances;
            IQueryable<ClientEmployeeAttendanceSM> retSM = await MapEntityAsToQuerable<ClientEmployeeAttendanceDM, ClientEmployeeAttendanceSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All ClientEmployeeAttendance in a database.
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeAttendance in database</returns>
        public async Task<List<ClientEmployeeAttendanceSM>> GetAllClientEmployeeAttendance()
        {
            var dm = await _apiDbContext.ClientEmployeeAttendances.ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeAttendanceSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeAttendance Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeAttendance</param>
        /// <returns>Service Model of ClientEmployeeAttendance in database of the id</returns>

        public async Task<ClientEmployeeAttendanceSM> GetClientEmployeeAttendanceById(int id)
        {
            ClientEmployeeAttendanceDM clientEmployeeAttendanceDM = await _apiDbContext.ClientEmployeeAttendances.FindAsync(id);

            if (clientEmployeeAttendanceDM != null)
            {
                return _mapper.Map<ClientEmployeeAttendanceSM>(clientEmployeeAttendanceDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get ClientEmployeeAttendance Details by Id
        /// </summary>
        /// <param name="employeeId">Primary Key of ClientUser</param>
        /// <param name="companyId">Primary Key of ClientCompany</param>
        /// <returns>Service Model of List of CompanyModules in database</returns>

        //public async Task<List<ClientEmployeeAttendanceSM>> GetClientEmployeeAttendanceByEmpoyeeId(int companyId, int employeeId)
        //{
        //    var dm = await _apiDbContext.ClientEmployeeAttendances.Where(x => x.ClientCompanyDetailId == companyId && x.ClientUserId == employeeId).ToListAsync();
        //    var sm = _mapper.Map<List<ClientEmployeeAttendanceSM>>(dm);
        //    return sm;
        //}

        /// <summary>
        /// Get ClientEmployeeAttendance Detail Based on ClientUserId in a database.
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser Object</param>
        /// <returns>Service Model of List of ClientEmployeeAttendance in database</returns>
        //public async Task<List<ClientEmployeeAttendanceSM>> GetClientEmployeeAttendanceDetailByEmpId(int empId)
        //{
        //    var clientEmployeeAttendanceDM = await _apiDbContext.ClientEmployeeAttendances.Where(x => x.ClientUserId == empId).ToListAsync();

        //    if (clientEmployeeAttendanceDM.Count > 0)
        //    {
        //        return _mapper.Map<List<ClientEmployeeAttendanceSM>>(clientEmployeeAttendanceDM);
        //    }
        //    else
        //    {
        //        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "ClientEmployeeAttendance Detail for this User Not Found");
        //    }
        //}

        #endregion --Get--

        #region --Count--

        /// <summary>
        /// Get All AttendanceReport Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param> <returns>integer response based on CompanyId</returns></returns>
        public async Task<int> GetAttendanceReportCount(EmployeeAttendanceReportRequestSM employeeAttendanceReportRequestSM, int currentCompanyId)
        {
            var today = employeeAttendanceReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            var reportCount = 0;
            employeeAttendanceReportRequestSM.SearchString = String.IsNullOrWhiteSpace(employeeAttendanceReportRequestSM.SearchString) ? null : employeeAttendanceReportRequestSM.SearchString.Trim();
            if (!String.IsNullOrWhiteSpace(employeeAttendanceReportRequestSM.SearchString))
            {
                reportCount = _apiDbContext.ClientEmployeeAttendances.Count(x => x.ClientCompanyDetailId == currentCompanyId &&
                (x.ClientUser.LoginId.Contains(employeeAttendanceReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(employeeAttendanceReportRequestSM.SearchString)) || (x.ClientUser.LastName.Contains(employeeAttendanceReportRequestSM.SearchString)));
                return reportCount;
            }
            switch (employeeAttendanceReportRequestSM.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = employeeAttendanceReportRequestSM.DateFrom;
                    endDate = employeeAttendanceReportRequestSM.DateTo;
                    break;
                default:
                    break;
            }
            reportCount = _apiDbContext.ClientEmployeeAttendances.Where(x => (((x.AttendanceDate > startDate && x.AttendanceDate < endDate))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
            return reportCount;
        }


        #endregion --Count--

        #region --Get My/Mine--

        /// <summary>
        /// Get ClientEmployeeAttendance Detail Based on ClientUserId in a database.
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser Object</param>
        /// <param name="dateTime">DateTime Object</param>
        /// <returns>Service Model of List of ClientEmployeeAttendance in database</return>
        public async Task<List<ClientEmployeeAttendanceSM>> GetMineClientEmployeeAttendanceDetailByEmpId(int empId, DateTime dateTime)
        {
            var today = dateTime;
            List<ClientEmployeeAttendanceSM> resp = new List<ClientEmployeeAttendanceSM>();
            DateTime startDate = new DateTime().Date;
            DateTime endDate = new DateTime();
            startDate = new DateTime(today.Year, today.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
            List<ClientEmployeeAttendanceSM> employeeAttendance = await GetMineClientEmployeeAttendanceDetailByEmpIdandDate(empId, startDate, endDate);
            while (startDate <= endDate)
            {
                ClientEmployeeAttendanceSM? attendanceSM = employeeAttendance.FirstOrDefault(x => x.AttendanceDate.Date == startDate.Date);
                if (attendanceSM == null)
                    attendanceSM = new ClientEmployeeAttendanceSM();
                attendanceSM.AttendanceDate = startDate;
                resp.Add(attendanceSM);
                startDate = startDate.AddDays(1);
            }
            return resp;
        }

        private async Task<List<ClientEmployeeAttendanceSM>> GetMineClientEmployeeAttendanceDetailByEmpIdandDate(int empId, DateTime startDate, DateTime endDate)
        {
            var employeeAttendanceDM = await _apiDbContext.ClientEmployeeAttendances.Where(x => (x.AttendanceDate >= startDate && x.AttendanceDate <= endDate) && (x.ClientUserId == empId)).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeAttendanceSM>>(employeeAttendanceDM);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeAttendance Detail Based on ClientUserId in a database.
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser Object</param>
        /// <param name="dateTime">DateTime Object</param>
        /// <returns>Service Model of List of ClientEmployeeAttendance in database</return>
        public async Task<List<ClientEmployeeAttendanceSM>> GetMyClientEmployeeAttendanceDetailByEmpId(int empId, int currentCompanyId, DateTime dateTime)
        {
            var today = dateTime;
            List<ClientEmployeeAttendanceSM> resp = new List<ClientEmployeeAttendanceSM>();
            DateTime startDate = new DateTime().Date;
            DateTime endDate = new DateTime();
            startDate = new DateTime(today.Year, today.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
            List<ClientEmployeeAttendanceSM> employeeAttendance = await GetMyClientEmployeeAttendanceDetailByEmpIdandDate(empId, currentCompanyId, startDate, endDate);
            while (startDate <= endDate)
            {
                ClientEmployeeAttendanceSM? attendanceSM = employeeAttendance.FirstOrDefault(x => x.AttendanceDate.Date == startDate.Date);
                if (attendanceSM == null)
                    attendanceSM = new ClientEmployeeAttendanceSM();
                attendanceSM.AttendanceDate = startDate;
                resp.Add(attendanceSM);
                startDate = startDate.AddDays(1);
            }
            return resp;
        }

        private async Task<List<ClientEmployeeAttendanceSM>> GetMyClientEmployeeAttendanceDetailByEmpIdandDate(int empId, int currentCompanyId, DateTime startDate, DateTime endDate)
        {
            var employeeAttendanceDM = await _apiDbContext.ClientEmployeeAttendances.Where(x => (x.AttendanceDate >= startDate && x.AttendanceDate <= endDate) && (x.ClientUserId == empId && x.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeAttendanceSM>>(employeeAttendanceDM);
            return sm;
        }

        #endregion --Get My/Mine--

        #region --Reports--

        /// <summary>
        /// Get all Employees-Attendance reports
        /// </summary>
        /// <param name="attendanceReportRequestSM">AttendanceReportRequest Object.</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeAttendance in database.</returns>
        public async Task<List<ClientEmployeeAttendanceExtendedUserSM>> GetTotalEmployeesAttendanceReport(EmployeeAttendanceReportRequestSM attendanceReportRequestSM, int currentCompanyId, int skip, int top)
        {
            var today = attendanceReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            switch (attendanceReportRequestSM.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = attendanceReportRequestSM.DateFrom;
                    endDate = attendanceReportRequestSM.DateTo;
                    break;
                default:
                    break;
            }
            attendanceReportRequestSM.SearchString = String.IsNullOrWhiteSpace(attendanceReportRequestSM.SearchString) ? null : attendanceReportRequestSM.SearchString.Trim();
            //var dm = _apiDbContext.ClientEmployeeAttendances.Where(x => (((x.AttendanceDate > startDate && x.AttendanceDate < endDate) || (x.ClientUser.LoginId.Contains(attendanceReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(attendanceReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(attendanceReportRequestSM.SearchString))))) && (x.ClientCompanyDetailId == currentCompanyId)).Select(y => y.ClientUserId).Distinct();
            var dm = await _apiDbContext.ClientEmployeeAttendances.Where(x => (((x.AttendanceDate > startDate && x.AttendanceDate < endDate) || (x.ClientUser.LoginId.Contains(attendanceReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(attendanceReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(attendanceReportRequestSM.SearchString))))) && (x.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
            List<ClientEmployeeAttendanceExtendedUserSM> clientEmployeeLeaveDetails = new List<ClientEmployeeAttendanceExtendedUserSM>();

            foreach (var items in dm)
            {
                //var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == items.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                var userName = await _clientUserProcess.GetUserName(items.ClientUserId);
                var presentCount = _apiDbContext.ClientEmployeeAttendances.Where(x => x.AttendanceDate > startDate && x.AttendanceDate < endDate && x.ClientUserId == items.ClientUserId && x.AttendanceStatus == AttendanceStatusDM.P).Count();
                var absentCount = _apiDbContext.ClientEmployeeAttendances.Where(x => x.AttendanceDate > startDate && x.AttendanceDate < endDate && x.ClientUserId == items.ClientUserId && x.AttendanceStatus == AttendanceStatusDM.A).Count();
                var leaveCount = await _clientEmployeeLeaveProcess.GetLeavesCountByClientId(items.ClientUserId, startDate, endDate);
                clientEmployeeLeaveDetails.Add(new ClientEmployeeAttendanceExtendedUserSM()
                {
                    UserName = userName,
                    PresentCount = presentCount,
                    AbsentCount = absentCount,
                    LeaveCount = leaveCount
                });

            }
            return clientEmployeeLeaveDetails;
        }

        /// <summary>
        /// Gets all leaves report of an employee.
        /// </summary>
        /// <param name="employeeAttendanceReportRequestSM">LeaveReportRequest Object.</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeLeaves in database.</returns>
        public async Task<List<ClientEmployeeAttendanceExtendedUserSM>> GetEmployeesAttendanceReportByClientId(EmployeeAttendanceReportRequestSM employeeAttendanceReportRequestSM, int currentCompanyId, int skip, int top)
        {
            var today = employeeAttendanceReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            switch (employeeAttendanceReportRequestSM.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = employeeAttendanceReportRequestSM.DateFrom;
                    endDate = employeeAttendanceReportRequestSM.DateTo;
                    break;
                default:
                    break;
            }
            employeeAttendanceReportRequestSM.SearchString = String.IsNullOrWhiteSpace(employeeAttendanceReportRequestSM.SearchString) ? null : employeeAttendanceReportRequestSM.SearchString.Trim();
            var dm = _apiDbContext.ClientEmployeeAttendances.Where(x => (((x.AttendanceDate > startDate && x.AttendanceDate < endDate) || (x.ClientUser.LoginId.Contains(employeeAttendanceReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(employeeAttendanceReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(employeeAttendanceReportRequestSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId && x.ClientUserId == employeeAttendanceReportRequestSM.ClientEmployeeUserId)).Skip(skip).Take(top).Select(y => y.ClientUserId).Distinct();
            List<ClientEmployeeAttendanceExtendedUserSM> clientEmployeeAttendanceExtendedUsers = new List<ClientEmployeeAttendanceExtendedUserSM>();
            foreach (var item in dm)
            {
                //var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == item).Select(x => x.LoginId).FirstOrDefaultAsync();
                var userName = await _clientUserProcess.GetUserName(item);
                var presentCount = _apiDbContext.ClientEmployeeAttendances.Where(x => x.AttendanceDate > startDate && x.AttendanceDate < endDate && x.ClientUserId == item && x.AttendanceStatus == AttendanceStatusDM.P).Count();
                var absentCount = _apiDbContext.ClientEmployeeAttendances.Where(x => x.AttendanceDate > startDate && x.AttendanceDate < endDate && x.ClientUserId == item && x.AttendanceStatus == AttendanceStatusDM.A).Count();
                var leaveCount = await _clientEmployeeLeaveProcess.GetLeavesCountByClientId(item, startDate, endDate);
                clientEmployeeAttendanceExtendedUsers.Add(new ClientEmployeeAttendanceExtendedUserSM()
                {
                    UserName = userName,
                    PresentCount = presentCount,
                    AbsentCount = absentCount,
                    LeaveCount = leaveCount
                });
            }

            return clientEmployeeAttendanceExtendedUsers;
        }

        #endregion --Reports--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientEmployeeAttendance
        /// </summary>
        /// <param name="clientEmployeeAttendanceSM">ClientEmployeeAttendance object</param>
        /// <returns> the added record</returns>

        public async Task<ClientEmployeeAttendanceSM> AddClientEmployeeAttendanceOnCheckIn(ClientEmployeeAttendanceSM clientEmployeeAttendanceSM)
        {
            var clientEmployeeAttendanceDM = _mapper.Map<ClientEmployeeAttendanceDM>(clientEmployeeAttendanceSM);
            clientEmployeeAttendanceDM.CreatedBy = _loginUserDetail.LoginId;
            clientEmployeeAttendanceDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientEmployeeAttendances.AddAsync(clientEmployeeAttendanceDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientEmployeeAttendanceSM>(clientEmployeeAttendanceDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientEmployeeAttendance of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientEmployeeAttendanceSM">ClientEmployeeAttendance object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeAttendanceSM> UpdateClientEmployeeAttendance(int objIdToUpdate, ClientEmployeeAttendanceSM clientEmployeeAttendanceSM)
        {
            if (clientEmployeeAttendanceSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientEmployeeAttendances.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientEmployeeAttendanceSM.Id = objIdToUpdate;

                    ClientEmployeeAttendanceDM dbDM = await _apiDbContext.ClientEmployeeAttendances.FindAsync(objIdToUpdate);
                    _mapper.Map(clientEmployeeAttendanceSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientEmployeeAttendanceSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeBankDetail not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientEmployeeAttendance by  Id
        /// </summary>
        /// <param name="id">primary key of ClientEmployeeBankDetail</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientEmployeeAttendance(int id)
        {
            var isPresent = await _apiDbContext.ClientEmployeeAttendances.AnyAsync(x => x.Id == id);
            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeAttendanceDM() { Id = id };
                _apiDbContext.ClientEmployeeAttendances.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Attendance Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientEmployeeAttendances by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientEmployeeAttendanceById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeAttendances.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeAttendanceDM() { Id = id };
                _apiDbContext.ClientEmployeeAttendances.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Attendance Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #region --Excel File--

        public async Task<ClientExcelFileResponseSM> GetAttendanceHeadingRowFromExcelFile(ClientExcelFileRequestSM excelFileRequestSM)
        {
            string fullPath = "";
            try
            {
                byte[] bytes = Convert.FromBase64String(excelFileRequestSM.UploadFile);
                fullPath = Path.Combine(Path.GetFullPath(Path.GetTempPath()), Guid.NewGuid().ToString()) + ".tmp";
                System.IO.File.WriteAllBytes(fullPath, bytes);
                //string fileContent;
                //using (StreamReader reader = new StreamReader(fullPath))
                //{
                //    fileContent = reader.ReadToEnd();
                //}     
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Reading and Saving File{ex}", "Error in Reading and Saving File");
            }

            var attendentsdata = HeadingsFromExcel(fullPath, excelFileRequestSM.HeaderRow);
            var headingData = new ClientExcelFileResponseSM();
            if (headingData.ExcelFieldMapping == null)
            {
                headingData.ExcelFieldMapping = attendentsdata;
            }

            headingData.FileName = fullPath;
            headingData.HeaderRow = excelFileRequestSM.HeaderRow;
            return headingData;
        }

        public async Task<ClientExcelFileSummarySM> AddAttendanceDataFromExcel(ClientExcelFileResponseSM excelFileResponseSM, int currentCompanyId)
        {
            if (!File.Exists(excelFileResponseSM.FileName))
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Please Upload a File.", "Please Upload a File.");
            }
            DataTable attendentsdata = DataFromExcelSheet(excelFileResponseSM.FileName, excelFileResponseSM.HeaderRow).Tables[0];
            ClientExcelFileSummarySM clientExcelFileSummarySM = new ClientExcelFileSummarySM();
            clientExcelFileSummarySM.AttendanceSummary = new List<ClientEmployeeAttendanceExtendedUserSM>();
            var count = 0;
            if (attendentsdata.Rows.Count > 0)
            {
                for (int i = 0; i < attendentsdata.Rows.Count; i++)
                {
                    var attendances = new ClientEmployeeAttendanceSM();
                    try
                    {

                        if (i >= excelFileResponseSM.HeaderRow)
                        {

                            var item = attendentsdata.Rows[i];
                            var empCodeCol = excelFileResponseSM.ExcelFieldMapping.GetValueOrDefault(AttendanceItemFields.EmployeeCode);
                            var CheckOutCol = excelFileResponseSM.ExcelFieldMapping.GetValueOrDefault(AttendanceItemFields.CheckOut);
                            var AttendanceStatusCol = excelFileResponseSM.ExcelFieldMapping.GetValueOrDefault(AttendanceItemFields.AttendanceStatus);
                            var AttendanceDateCol = excelFileResponseSM.ExcelFieldMapping.GetValueOrDefault(AttendanceItemFields.AttendanceDate);
                            var CheckInCol = excelFileResponseSM.ExcelFieldMapping.GetValueOrDefault(AttendanceItemFields.CheckIn);
                            var EmployeeNameCol = excelFileResponseSM.ExcelFieldMapping.GetValueOrDefault(AttendanceItemFields.EmployeeName);
                            var EmployeeCode = !string.IsNullOrEmpty(item[empCodeCol].ToString()) ? item[empCodeCol].ToString() : "";
                            var clientUserId = await _apiDbContext.ClientUsers.Where(x => x.EmployeeCode == EmployeeCode && x.ClientCompanyDetailId == currentCompanyId).Select(y => y.Id).FirstOrDefaultAsync();
                            var status = !string.IsNullOrEmpty(item[AttendanceStatusCol].ToString()) ? item[AttendanceStatusCol].ToString() : "";
                            if (status == "WO-I" || status == "WO-II" || status == "WO")
                            {
                                status = "WO";
                            }
                            if (Enum.TryParse<AttendanceStatusSM>(status, out AttendanceStatusSM workStatus))
                            {
                                attendances.AttendanceStatus = workStatus;
                            }
                            if (DateTime.TryParse(item[AttendanceDateCol].ToString(), out DateTime activates))
                            {
                                attendances.AttendanceDate = activates;
                            }
                            attendances.CheckIn = !string.IsNullOrEmpty(item[CheckInCol].ToString()) ? item[CheckInCol].ToString() : "";
                            attendances.CheckOut = !string.IsNullOrEmpty(item[CheckInCol].ToString()) ? item[CheckOutCol].ToString() : "";
                            attendances.ClientCompanyDetailId = currentCompanyId;

                            if (i == excelFileResponseSM.HeaderRow)
                            {
                                if (DateTime.TryParse(item[AttendanceDateCol].ToString(), out DateTime activatess))
                                {
                                    clientExcelFileSummarySM.FromDate = activatess.ToString("dd-MM-yyyy");
                                }

                            }

                            if (clientUserId == 0)
                            {
                                count++;
                                clientExcelFileSummarySM.AttendanceSummary.Add(new ClientEmployeeAttendanceExtendedUserSM()
                                {
                                    ErrorMessageInUpload = $"EmployeeCode : {EmployeeCode} not found.",
                                    CheckIn = attendances.CheckIn,
                                    CheckOut = attendances.CheckOut,
                                    AttendanceDate = activates,
                                    AttendanceStatus = workStatus,
                                    ClientCompanyDetailId = attendances.ClientCompanyDetailId,
                                    UserName = item[EmployeeNameCol].ToString(),
                                    EmployeeCode = EmployeeCode,
                                });
                                if (i == attendentsdata.Rows.Count - 1)
                                {
                                    if (DateTime.TryParse(item[AttendanceDateCol].ToString(), out DateTime activatess))
                                    {
                                        clientExcelFileSummarySM.ToDate = activatess.ToString("dd-MM-yyyy");
                                    }

                                }
                                continue;
                            }
                            else
                            {
                                attendances.ClientUserId = clientUserId;
                            }
                            if (String.IsNullOrEmpty(item[EmployeeNameCol].ToString()))
                            {
                                clientExcelFileSummarySM.AttendanceSummary.Add(new ClientEmployeeAttendanceExtendedUserSM()
                                {
                                    ErrorMessageInUpload = $"Employee Name not found.",
                                    CheckIn = attendances.CheckIn,
                                    CheckOut = attendances.CheckOut,
                                    AttendanceDate = activates,
                                    AttendanceStatus = workStatus,
                                    ClientCompanyDetailId = attendances.ClientCompanyDetailId,
                                    UserName = item[EmployeeNameCol].ToString(),
                                    ClientUserId = attendances.ClientUserId,
                                    EmployeeCode = EmployeeCode
                                });
                            }

                            if (!DateTime.TryParse(item[AttendanceDateCol].ToString(), out DateTime activate))
                            {
                                clientExcelFileSummarySM.AttendanceSummary.Add(new ClientEmployeeAttendanceExtendedUserSM()
                                {
                                    ErrorMessageInUpload = $"Attendance date is not in correct format.",
                                    CheckIn = attendances.CheckIn,
                                    CheckOut = attendances.CheckOut,
                                    ClientUserId = attendances.ClientUserId,
                                    ClientCompanyDetailId = attendances.ClientCompanyDetailId,
                                    AttendanceStatus = workStatus,
                                    UserName = item[EmployeeNameCol].ToString(),
                                    AttendanceDate = activates,
                                    EmployeeCode = EmployeeCode
                                });
                            }
                            if (!Enum.TryParse<AttendanceStatusSM>(status, out AttendanceStatusSM workStatuses))
                            {
                                clientExcelFileSummarySM.AttendanceSummary.Add(new ClientEmployeeAttendanceExtendedUserSM()
                                {
                                    ErrorMessageInUpload = $"WorkStatus not in a correct Formate.",
                                    CheckIn = attendances.CheckIn,
                                    CheckOut = attendances.CheckOut,
                                    AttendanceDate = attendances.AttendanceDate,
                                    ClientUserId = clientUserId,
                                    ClientCompanyDetailId = attendances.ClientCompanyDetailId,
                                    UserName = item[EmployeeNameCol].ToString(),
                                    AttendanceStatus = workStatus,
                                    EmployeeCode = EmployeeCode
                                });
                            }
                            if (i == attendentsdata.Rows.Count - 1)
                            {
                                if (DateTime.TryParse(item[AttendanceDateCol].ToString(), out DateTime activatess))
                                {
                                    clientExcelFileSummarySM.ToDate = activatess.ToString("dd-MM-yyyy");

                                }

                            }
                            var dbItem = new ClientEmployeeAttendanceDM();
                            var clientEmployeeAttendanceDM = _mapper.Map<ClientEmployeeAttendanceDM>(attendances);
                            clientEmployeeAttendanceDM.CreatedBy = _loginUserDetail.LoginId;
                            clientEmployeeAttendanceDM.CreatedOnUTC = DateTime.UtcNow;
                            await _apiDbContext.ClientEmployeeAttendances.AddAsync(clientEmployeeAttendanceDM);
                        }


                    }

                    catch (Exception ex)
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Excel File Reading.{ex}", "Error in Excel File Reading");
                    }
                }
                clientExcelFileSummarySM.NumberOfRecordsNotAdded = count;
                clientExcelFileSummarySM.TotalRecordsCount = attendentsdata.Rows.Count - excelFileResponseSM.HeaderRow;
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    clientExcelFileSummarySM.EmployeeRecordsAddedCount = clientExcelFileSummarySM.TotalRecordsCount - count;
                    return clientExcelFileSummarySM;
                }
                return clientExcelFileSummarySM;

            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Excel file empty or not in correct format,Excel does not contain any data.", "Excel file empty or not in correct format,Excel does not contain any data.");
            }
        }


        #endregion --Excel File--

        #region Add Data Of SummaryFile

        public async Task<ClientExcelFileSummarySM> AddAttendanceDataFromSummary(List<ClientEmployeeAttendanceExtendedUserSM> clientEmployeeAttendanceExtendedUserSMs, int currentCompanyId)
        {
            ClientExcelFileSummarySM clientExcelFileSummarySM = new ClientExcelFileSummarySM();
            clientExcelFileSummarySM.AttendanceSummary = new List<ClientEmployeeAttendanceExtendedUserSM>();
            try
            {

                foreach (ClientEmployeeAttendanceExtendedUserSM item in clientEmployeeAttendanceExtendedUserSMs)
                {
                    if (item.EmployeeCode == null)
                    {
                        continue;
                    }
                    var clientUserId = await _apiDbContext.ClientUsers.Where(x => x.EmployeeCode == item.EmployeeCode && x.ClientCompanyDetailId == currentCompanyId).Select(y => y.Id).FirstOrDefaultAsync();


                    if (clientUserId == 0)
                    {
                        clientExcelFileSummarySM.AttendanceSummary.Add(new ClientEmployeeAttendanceExtendedUserSM()
                        {
                            ErrorMessageInUpload = $"Employee Code : {item.EmployeeCode} not Found.",
                            CheckIn = item.CheckIn,
                            CheckOut = item.CheckOut,
                            AttendanceDate = item.AttendanceDate,
                            ClientUserId = clientUserId,
                            ClientCompanyDetailId = currentCompanyId,
                            UserName = item.UserName,
                            AttendanceStatus = item.AttendanceStatus,
                            EmployeeCode = item.EmployeeCode
                        });
                    }
                    else
                    {
                        var attendances = new ClientEmployeeAttendanceSM
                        {
                            AttendanceDate = item.AttendanceDate,
                            AttendanceStatus = item.AttendanceStatus,
                            CheckIn = item.CheckIn,
                            CheckOut = item.CheckOut,
                            ClientUserId = clientUserId,
                            ClientCompanyDetailId = currentCompanyId,
                        };

                        var dbItem = _mapper.Map<ClientEmployeeAttendanceDM>(attendances);
                        dbItem.CreatedBy = _loginUserDetail.LoginId;
                        dbItem.CreatedOnUTC = DateTime.UtcNow;
                        await _apiDbContext.ClientEmployeeAttendances.AddAsync(dbItem);
                    }
                }
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return clientExcelFileSummarySM;
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Saving Data in Attendance Table{ex}", "Error in Saving Data in Attendance Table");
            }

            return clientExcelFileSummarySM;

        }

        #endregion Add Data Of SummaryFile

        #region --Private-Section--

        private DataSet DataFromExcelSheet(string tempFilePath, int HeaderColumn)
        {
            string isHeader = "no"; string Sheetname = null;
            try
            {
                string connectionString = "";
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1;'";
                connectionString = String.Format(connectionString, tempFilePath, isHeader);
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connectionString);
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbDataAdapter dataAdapter = new System.Data.OleDb.OleDbDataAdapter();
                cmd.Connection = conn;
                DataSet ds = new DataSet();
                var columns = new Dictionary<int, string>();
                var cols = new DataColumn();
                try
                {
                    conn.Open();
                    DataTable dtSchema;
                    dtSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);


                    if (dtSchema.Rows.Count > 0)
                    {
                        var tmpobj = dtSchema.Select("TABLE_NAME like '%" + Sheetname + "%'");
                        foreach (var item in tmpobj)
                        {
                            //int count = 0;
                            DataTable dt = new DataTable();
                            dt.TableName = item["TABLE_NAME"].ToString().Trim('\'').Trim('$');
                            cmd.CommandText = "SELECT * From [" + item["TABLE_NAME"].ToString() + "]";
                            dataAdapter.SelectCommand = cmd;
                            dataAdapter.Fill(dt);
                            ds.Tables.Add(dt);
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Reading Excel File{ex}", "Error in Reading Excel File");
                }
                System.IO.File.Delete(tempFilePath);
                return ds;
            }
            catch (Exception ex)
            {

                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Reading Excel File{ex}", "Error in Reading Excel File");
            }
        }

        private Dictionary<string, int> HeadingsFromExcel(string tempFilePath, int HeaderColumn)
        {
            string isHeader = "no"; string Sheetname = null;
            try
            {
                string connectionString = "";
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1;'";
                connectionString = String.Format(connectionString, tempFilePath, isHeader);
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connectionString);
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbDataAdapter dataAdapter = new System.Data.OleDb.OleDbDataAdapter();
                cmd.Connection = conn;
                DataSet ds = new DataSet();
                var columns = new Dictionary<string, int>();
                var cols = new DataColumn();
                try
                {
                    conn.Open();
                    DataTable dtSchema;
                    dtSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);


                    if (dtSchema.Rows.Count > 0)
                    {
                        var tmpobj = dtSchema.Select("TABLE_NAME like '%" + Sheetname + "%'");
                        foreach (var item in tmpobj)
                        {
                            int count = 0;
                            cmd.CommandText = "SELECT * From [" + item["TABLE_NAME"].ToString() + "]";
                            using (OleDbDataReader dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    if (count == (HeaderColumn - 1))
                                    {
                                        for (int i = 0; i < dataReader.FieldCount; i++)
                                        {
                                            string val = dataReader.GetString(i);
                                            columns.Add(val, i);
                                        }
                                        break;
                                    }
                                    count++;
                                }
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Reading Excel File{ex}", "Error in Reading Excel File");
                }
                return columns;
            }
            catch (Exception ex)
            {

                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Reading Excel File{ex}", "Error in Reading Excel File");
            }
        }

        #endregion --Private-Section--

    }
}
