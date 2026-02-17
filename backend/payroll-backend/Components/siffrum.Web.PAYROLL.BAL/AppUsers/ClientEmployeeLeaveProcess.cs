using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientEmployeeLeaveProcess : SiffrumPayrollBalOdataBase<ClientEmployeeLeaveSM>
    {

        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly ClientUserProcess _clientUserProcess;

        #endregion --Properties--

        #region --Constructor--

        public ClientEmployeeLeaveProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, ClientUserProcess clientUserProcess)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _clientUserProcess = clientUserProcess;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientEmployeeLeaveSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientEmployeeLeaves;
            IQueryable<ClientEmployeeLeaveSM> retSM = await MapEntityAsToQuerable<ClientEmployeeLeaveDM, ClientEmployeeLeaveSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientEmployeeLeave details in database
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeLeave in database</returns>
        public async Task<List<ClientEmployeeLeaveSM>> GetAllClientEmployeeLeaves()
        {
            var dm = await _apiDbContext.ClientEmployeeLeaves.ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeLeaveSM>>(dm);
            return sm;
        }


        /// <summary>
        /// Get ClientEmployeeLeave Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeLeave</param>
        /// <returns>Service Model of ClientEmployeeLeave in database of the id</returns>

        public async Task<ClientEmployeeLeaveExtendedUserSM> GetClientEmployeeLeaveById(int id)
        {
            ClientEmployeeLeaveDM clientEmployeeLeaveDM = await _apiDbContext.ClientEmployeeLeaves.FindAsync(id);
            if (clientEmployeeLeaveDM != null)
            {
                var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == clientEmployeeLeaveDM.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                ClientEmployeeLeaveExtendedUserSM clientEmployeeLeaveExtendedUserSM = new ClientEmployeeLeaveExtendedUserSM()
                {
                    Id = clientEmployeeLeaveDM.Id,
                    ClientUserId = clientEmployeeLeaveDM.ClientUserId,
                    LeaveType = (LeaveTypeSM)clientEmployeeLeaveDM.LeaveType,
                    EmployeeComment = clientEmployeeLeaveDM.EmployeeComment,
                    ApprovedByUserName = clientEmployeeLeaveDM.ApprovedByUserName,
                    IsApproved = clientEmployeeLeaveDM.IsApproved,
                    ApprovalComment = clientEmployeeLeaveDM.ApprovalComment,
                    LeaveDateFromUTC = clientEmployeeLeaveDM.LeaveDateFromUTC,
                    LeaveDateToUTC = clientEmployeeLeaveDM.LeaveDateToUTC,
                    UserName = userName,
                };
                return clientEmployeeLeaveExtendedUserSM;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get ClientEmployeeLeave Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeLeave</param>
        /// <returns>Service Model of ClientEmployeeLeave in database of the id</returns>

        public async Task<List<ClientEmployeeLeaveSM>> GetEmployeeLeavesByUserId(int id)
        {
            var dm = await _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientUserId == id).ToListAsync();
            if (dm.Count > 0)
            {
                return _mapper.Map<List<ClientEmployeeLeaveSM>>(dm);
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {id}", "Leave for this User Not Found");
            }
        }

        /// <summary>
        /// Get ClientEmployeeLeave Details by Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of ClientEmployeeLeaveExtendedUser in database of the ClientCompanyDetailId</returns>

        public async Task<List<ClientEmployeeLeaveExtendedUserSM>> GetEmployeeLeaveByExpanded(int currentCompanyId, int skip, int top)
        {
            var employeeLeaveDB = await _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).Skip(skip).Take(top).ToListAsync();
            List<ClientEmployeeLeaveExtendedUserSM> clientEmployeeLeaveDetails = new List<ClientEmployeeLeaveExtendedUserSM>();

            if (employeeLeaveDB.Count > 0)
            {
                foreach (var item in employeeLeaveDB)
                {
                    var userName = await _clientUserProcess.GetUserName(item.ClientUserId);
                    clientEmployeeLeaveDetails.Add(new ClientEmployeeLeaveExtendedUserSM()
                    {
                        Id = item.Id,
                        UserName = userName,
                        ClientUserId = item.ClientUserId,
                        LeaveType = (LeaveTypeSM)item.LeaveType,
                        EmployeeComment = item.EmployeeComment,
                        IsApproved = item.IsApproved,
                        ApprovalComment = item.ApprovalComment,
                        LeaveDateFromUTC = item.LeaveDateFromUTC,
                        LeaveDateToUTC = item.LeaveDateToUTC,
                        CreatedBy = item.CreatedBy,
                        CreatedOnUTC = item.CreatedOnUTC,
                        ClientCompanyDetailId = currentCompanyId
                    });
                }
            }
            return clientEmployeeLeaveDetails;
        }


        #endregion --Get--

        #region --COUNT--

        /// <summary>
        /// Get ClientEmployeeLeave Count by EmployeeId in database.
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser</param>
        /// <param name="currenCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>
        public async Task<int> GetClientEmployeeLeaveCounts(int empId, int currenCompanyId)
        {
            int resp = _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currenCompanyId).Count();
            return resp;
        }

        /// <summary>
        /// Get All ClientEmployeeLeave Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param> <returns>integer response based on CompanyId</returns></returns>
        public async Task<int> GetAllClientEmployeeLeaveCounts(int currentCompanyId)
        {
            int resp = _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        /// <summary>
        /// Gets all leaves report of an employee.
        /// </summary>
        /// <param name="leaveReportRequestSM">LeaveReportRequest Object.</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeLeaves in database.</returns>
        public async Task<int> GetLeavesCountByClientId(int clientUserId, DateTime startDate, DateTime endDate)
        {
            var leaveCount = _apiDbContext.ClientEmployeeLeaves.Where(x => x.LeaveDateFromUTC > startDate && x.LeaveDateToUTC < endDate && x.ClientUserId == clientUserId).Count();

            return leaveCount;
        }

        /// <summary>
        /// Get All ClientEmployeeLeave Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param> <returns>integer response based on CompanyId</returns></returns>
        public async Task<int> GetLeaveReportCount(LeaveReportRequestSM leaveReportRequestSM, int currentCompanyId)
        {
            var today = leaveReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            var reportCount = 0;
            leaveReportRequestSM.SearchString = String.IsNullOrWhiteSpace(leaveReportRequestSM.SearchString) ? null : leaveReportRequestSM.SearchString.Trim();
            if (!String.IsNullOrWhiteSpace(leaveReportRequestSM.SearchString))
            {
                reportCount = _apiDbContext.ClientEmployeeLeaves.Count(x => x.ClientCompanyDetailId == currentCompanyId &&
                (x.ClientUser.LoginId.Contains(leaveReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(leaveReportRequestSM.SearchString)) || (x.ClientUser.LastName.Contains(leaveReportRequestSM.SearchString)));
                return reportCount;
            }
            switch (leaveReportRequestSM.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    reportCount = _apiDbContext.ClientEmployeeLeaves.Where(x => (((x.LeaveDateFromUTC > startDate && x.LeaveDateToUTC < endDate))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    reportCount = _apiDbContext.ClientEmployeeLeaves.Where(x => (((x.LeaveDateFromUTC > startDate && x.LeaveDateToUTC < endDate))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = leaveReportRequestSM.DateFrom;
                    endDate = leaveReportRequestSM.DateTo;
                    reportCount = _apiDbContext.ClientEmployeeLeaves.Where(x => (((x.LeaveDateFromUTC > startDate && x.LeaveDateToUTC < endDate))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                default:
                    break;
            }
            return reportCount;
        }

        #endregion --COUNT--

        #region --My-EndPoints--

        /// <summary>
        /// Get ClientEmployeeLeave Details by Employee-Id and Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="id">Primary Key of ClientEmployeeUser</param>
        /// <returns>Service Model of ClientUsersBankDetailByEmployee in database of the ClientUserId</returns>

        public async Task<List<ClientEmployeeLeaveSM>> GetClientUsersLeaveByEmployeeIdOfMyCompany(int id, int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientUserId == id && x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            if (dm.Count > 0)
            {
                return _mapper.Map<List<ClientEmployeeLeaveSM>>(dm);
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {id}", "Leave for this User Not Found");
            }
        }

        /// <summary>
        /// Get All ClientEmployeeLeave details in database of My Company
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeLeave in database</returns>
        public async Task<List<ClientEmployeeLeaveSM>> GetAllClientUsersLeaveOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeLeaveSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeLeave details in database by LeaveId of My Company
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeLeave</param>
        /// <param name="companyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of List of ClientEmployeeLeave in database</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeLeaveExtendedUserSM> GetClientEmployeeLeaveByIdOfMyCompany(int id, int companyId)
        {
            var clientEmployeeLeaveDM = await _apiDbContext.ClientEmployeeLeaves.Where(x => x.Id == id && x.ClientUser.ClientCompanyDetailId == companyId).FirstOrDefaultAsync();
            if (clientEmployeeLeaveDM != null)
            {
                var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == clientEmployeeLeaveDM.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                ClientEmployeeLeaveExtendedUserSM clientEmployeeLeaveExtendedUserSM = new ClientEmployeeLeaveExtendedUserSM()
                {
                    Id = clientEmployeeLeaveDM.Id,
                    ClientUserId = clientEmployeeLeaveDM.ClientUserId,
                    LeaveType = (LeaveTypeSM)clientEmployeeLeaveDM.LeaveType,
                    EmployeeComment = clientEmployeeLeaveDM.EmployeeComment,
                    ApprovedByUserName = clientEmployeeLeaveDM.ApprovedByUserName,
                    IsApproved = clientEmployeeLeaveDM.IsApproved,
                    ApprovalComment = clientEmployeeLeaveDM.ApprovalComment,
                    LeaveDateFromUTC = clientEmployeeLeaveDM.LeaveDateFromUTC,
                    LeaveDateToUTC = clientEmployeeLeaveDM.LeaveDateToUTC,
                    UserName = userName,
                };
                return clientEmployeeLeaveExtendedUserSM;
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeLeave not found: {id}", "Leave Not Found");
            }
        }

        #endregion --My-EndPoints--

        #region --Reports--

        /// <summary>
        /// Get all Leaves reports
        /// </summary>
        /// <param name="leaveReportRequestSM">LeaveReportRequest Object.</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeLeaves in database.</returns>
        public async Task<List<ClientEmployeeLeaveExtendedUserSM>> GetTotalLeavesReport(LeaveReportRequestSM leaveReportRequestSM, int currentCompanyId, int skip, int top)
        {
            List<ClientEmployeeLeaveDM> clientEmployeeLeaveDMs = new List<ClientEmployeeLeaveDM>();
            if (skip != -1 && top != -1)
            {
                var today = leaveReportRequestSM.DateFrom;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                switch (leaveReportRequestSM.DateFilterType)
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
                        startDate = leaveReportRequestSM.DateFrom;
                        endDate = leaveReportRequestSM.DateTo;
                        break;
                    default:
                        break;
                }
                leaveReportRequestSM.SearchString = String.IsNullOrWhiteSpace(leaveReportRequestSM.SearchString) ? null : leaveReportRequestSM.SearchString.Trim();
                clientEmployeeLeaveDMs = await _apiDbContext.ClientEmployeeLeaves.Where(x => (((x.LeaveDateFromUTC > startDate && x.LeaveDateToUTC < endDate) || (x.ClientUser.LoginId.Contains(leaveReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(leaveReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(leaveReportRequestSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
            }
            else
            {
                clientEmployeeLeaveDMs = await _apiDbContext.ClientEmployeeLeaves.Where(x => (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
            }
            List<ClientEmployeeLeaveExtendedUserSM> clientEmployeeLeaveDetails = new List<ClientEmployeeLeaveExtendedUserSM>();
            foreach (var item in clientEmployeeLeaveDMs)
            {
                var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == item.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                clientEmployeeLeaveDetails.Add(new ClientEmployeeLeaveExtendedUserSM()
                {
                    Id = item.Id,
                    UserName = userName,
                    ClientUserId = item.ClientUserId,
                    ClientCompanyDetailId = item.ClientCompanyDetailId,
                    LeaveType = (LeaveTypeSM)item.LeaveType,
                    EmployeeComment = item.EmployeeComment,
                    IsApproved = item.IsApproved,
                    ApprovalComment = item.ApprovalComment,
                    LeaveDateFromUTC = item.LeaveDateFromUTC,
                    LeaveDateToUTC = item.LeaveDateToUTC,
                    CreatedBy = item.CreatedBy,
                    CreatedOnUTC = item.CreatedOnUTC,
                });
            }
            return clientEmployeeLeaveDetails;
        }

        /// <summary>
        /// Gets all leaves report of an employee.
        /// </summary>
        /// <param name="leaveReportRequestSM">LeaveReportRequest Object.</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeLeaves in database.</returns>
        public async Task<List<ClientEmployeeLeaveExtendedUserSM>> GetLeavesReportByClientId(LeaveReportRequestSM leaveReportRequestSM, int currentCompanyId, int skip, int top)
        {
            List<ClientEmployeeLeaveDM> clientEmployeeLeaveDMs = new List<ClientEmployeeLeaveDM>();
            if (skip != -1 && top != -1)
            {
                var today = leaveReportRequestSM.DateFrom;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                switch (leaveReportRequestSM.DateFilterType)
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
                        startDate = leaveReportRequestSM.DateFrom;
                        endDate = leaveReportRequestSM.DateTo;
                        break;
                    default:
                        break;
                }
                leaveReportRequestSM.SearchString = String.IsNullOrWhiteSpace(leaveReportRequestSM.SearchString) ? null : leaveReportRequestSM.SearchString.Trim();
                clientEmployeeLeaveDMs = await _apiDbContext.ClientEmployeeLeaves.Where(x => (((x.LeaveDateFromUTC > startDate && x.LeaveDateToUTC < endDate) || (x.ClientUser.LoginId.Contains(leaveReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(leaveReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(leaveReportRequestSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId && x.ClientUserId == leaveReportRequestSM.ClientEmployeeUserId)).Skip(skip).Take(top).ToListAsync();
            }
            else
            {
                clientEmployeeLeaveDMs = await _apiDbContext.ClientEmployeeLeaves.Where(x => (x.ClientUser.ClientCompanyDetailId == currentCompanyId && x.ClientUserId == leaveReportRequestSM.ClientEmployeeUserId)).ToListAsync();
            }
            List<ClientEmployeeLeaveExtendedUserSM> clientEmployeeLeaveDetails = new List<ClientEmployeeLeaveExtendedUserSM>();
            foreach (var item in clientEmployeeLeaveDMs)
            {
                var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == item.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                clientEmployeeLeaveDetails.Add(new ClientEmployeeLeaveExtendedUserSM()
                {
                    Id = item.Id,
                    UserName = userName,
                    ClientUserId = item.ClientUserId,
                    ClientCompanyDetailId = item.ClientCompanyDetailId,
                    LeaveType = (LeaveTypeSM)item.LeaveType,
                    EmployeeComment = item.EmployeeComment,
                    IsApproved = item.IsApproved,
                    ApprovalComment = item.ApprovalComment,
                    LeaveDateFromUTC = item.LeaveDateFromUTC,
                    LeaveDateToUTC = item.LeaveDateToUTC,
                    CreatedBy = item.CreatedBy,
                    CreatedOnUTC = item.CreatedOnUTC,
                });
            }

            return clientEmployeeLeaveDetails;
        }

        #endregion --Reports--


        #region --Add/Update--

        /// <summary>
        /// Add new ClientEmployeeLeave
        /// </summary>
        /// <param name="clientEmployeeLeaveSM">ClientEmployeeLeave object</param>
        /// <returns> the added record</returns>

        public async Task<ClientEmployeeLeaveSM> AddClientEmployeeLeave(ClientEmployeeLeaveSM clientEmployeeLeaveSM)
        {
            var clientEmployeeLeaveDM = _mapper.Map<ClientEmployeeLeaveDM>(clientEmployeeLeaveSM);
            clientEmployeeLeaveDM.CreatedBy = _loginUserDetail.LoginId;
            clientEmployeeLeaveDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientEmployeeLeaves.AddAsync(clientEmployeeLeaveDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientEmployeeLeaveSM>(clientEmployeeLeaveDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientEmployeeLeave of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientEmployeeLeaveSm">ClientUser object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeLeaveSM> UpdateClientEmployeeLeave(int objIdToUpdate, ClientEmployeeLeaveSM clientEmployeeLeaveSM)
        {
            if (clientEmployeeLeaveSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientEmployeeLeaves.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientEmployeeLeaveSM.Id = objIdToUpdate;

                    ClientEmployeeLeaveDM dbDM = await _apiDbContext.ClientEmployeeLeaves.FindAsync(objIdToUpdate);
                    _mapper.Map(clientEmployeeLeaveSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientEmployeeLeaveSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeLeave not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientEmployeeLeave by  Id
        /// </summary>
        /// <param name="id">primary key of ClientEmployeeLeave</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientEmployeeLeaveById(int id)
        {
            var isPresent = await _apiDbContext.ClientEmployeeLeaves.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeLeaveDM() { Id = id };
                _apiDbContext.ClientEmployeeLeaves.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Leave Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientUserAddress by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientEmployeeLeaveById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeLeaves.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeLeaveDM() { Id = id };
                _apiDbContext.ClientEmployeeLeaves.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Leave Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #endregion CRUD

    }
}
