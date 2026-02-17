using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientEmployeeCTCDetailProcess : SiffrumPayrollBalOdataBase<ClientEmployeeCTCDetailSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly ClientUserProcess _clientUserProcess;

        #endregion --Properties--

        #region --Constructor--

        public ClientEmployeeCTCDetailProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, ClientUserProcess clientUserProcess)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _clientUserProcess = clientUserProcess;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientEmployeeCTCDetailSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientEmployeeCTCDetails;
            IQueryable<ClientEmployeeCTCDetailSM> retSM = await MapEntityAsToQuerable<ClientEmployeeCTCDetailDM, ClientEmployeeCTCDetailSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientEmployeeCTCDetail details in database
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeCTCDetail in database</returns>
        public async Task<List<ClientEmployeeCTCDetailSM>> GetAllClientEmployeeCTCDetails()
        {
            var dm = await _apiDbContext.ClientEmployeeCTCDetails.ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeCTCDetailSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeCTCDetail Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientUser</param>
        /// <returns>Service Model of ClientEmployeeCTCDetail in database of the id</returns>

        public async Task<ClientEmployeeCTCDetailSM> GetClientEmployeeCTCDetailById(int id)
        {
            ClientEmployeeCTCDetailDM clientEmployeeCTCDetailDM = await _apiDbContext.ClientEmployeeCTCDetails.FindAsync(id);
            var clientEmployeePayrollComponentDM = await _apiDbContext.ClientEmployeePayrollComponents.Where(x => x.ClientEmployeeCTCDetailId == id).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeePayrollComponentSM>>(clientEmployeeCTCDetailDM.ClientEmployeePayrollComponents);
            ClientEmployeeCTCDetailSM clientEmployeeCTCDetailSM = new ClientEmployeeCTCDetailSM();
            if (clientEmployeeCTCDetailDM != null)
            {
                clientEmployeeCTCDetailSM = _mapper.Map<ClientEmployeeCTCDetailSM>(clientEmployeeCTCDetailDM);
            }
            else
            {
                return null;
            }
            if (sm != null)
            {
                clientEmployeeCTCDetailSM.ClientEmployeePayrollComponents = sm;
            }
            return clientEmployeeCTCDetailSM;
        }

        /// <summary>
        /// Get ClientEmployeeCTCDetail Details by Employee-Id
        /// </summary>
        /// <param name="empId">Foreign Key of ClientEmployeeCTCDetail</param>
        /// <returns>Service Model of ClientEmployeeCTCDetail in database of the ClientUserId</returns>

        public async Task<List<ClientEmployeeCTCDetailSM>> GetClientEmployeeCtcDetailByEmpId(int id)
        {
            var dm = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => x.ClientUserId == id).ToListAsync();
            if (dm.Count > 0)
            {
                var sm = _mapper.Map<List<ClientEmployeeCTCDetailSM>>(dm);
                return sm;
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeCTCDetail not found: {id}", "CTC Detail for this User Not Found");
            }
        }

        /// <summary>
        /// Get ClientEmployeeCTCDetail Details by Employee-Id
        /// </summary>.
        /// <param name="id">Foreign Key of ClientEmployeeCTCDetail</param>
        /// <returns>Service Model of ClientEmployeeCTCDetail in database of the ClientUserId</returns>

        public async Task<ClientEmployeeCTCDetailSM> GetClientEmployeeActiveCtcDetailByEmpId(int id)
        {
            var clientEmployeeCTCDetailDM = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => x.ClientUserId == id && x.CurrentlyActive == true).FirstOrDefaultAsync();
            ClientEmployeeCTCDetailSM clientEmployeeCTCDetailSM = new ClientEmployeeCTCDetailSM();
            if (clientEmployeeCTCDetailDM != null)
            {
                clientEmployeeCTCDetailSM = _mapper.Map<ClientEmployeeCTCDetailSM>(clientEmployeeCTCDetailDM);
            }
            else
            {
                return null;
            }
            var clientEmployeePayrollComponentDM = await _apiDbContext.ClientEmployeePayrollComponents.Where(x => x.ClientEmployeeCTCDetailId == clientEmployeeCTCDetailDM.Id).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeePayrollComponentSM>>(clientEmployeeCTCDetailDM.ClientEmployeePayrollComponents);
            if (sm != null)
            {
                clientEmployeeCTCDetailSM.ClientEmployeePayrollComponents = sm;
            }
            return clientEmployeeCTCDetailSM;
        }

        #endregion --Get--

        #region --Count--

        /// <summary>
        /// Get  ClientEmployeeCtcDetails Count by ClientEmployeeUserId
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>
        public async Task<int> GetClientEmployeeCtcDetailCounts(int empId, int currentCompanyId)
        {
            int resp = _apiDbContext.ClientEmployeeCTCDetails.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        /// <summary>
        /// Get All ClientEmployeeCTC Report Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param> <returns>integer response based on CompanyId</returns></returns>
        public async Task<int> GetEmployeeCTCReportCount(EmployeeCTCReportRequestSM employeeCTCReportRequestSM, int currentCompanyId)
        {
            var today = employeeCTCReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            var reportCount = 0;
            employeeCTCReportRequestSM.SearchString = String.IsNullOrWhiteSpace(employeeCTCReportRequestSM.SearchString) ? null : employeeCTCReportRequestSM.SearchString.Trim();
            if (!String.IsNullOrWhiteSpace(employeeCTCReportRequestSM.SearchString))
            {
                reportCount = _apiDbContext.ClientEmployeeLeaves.Count(x => x.ClientCompanyDetailId == currentCompanyId &&
                (x.ClientUser.LoginId.Contains(employeeCTCReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(employeeCTCReportRequestSM.SearchString)) || (x.ClientUser.LastName.Contains(employeeCTCReportRequestSM.SearchString)));
                return reportCount;
            }
            else
            {
                switch (employeeCTCReportRequestSM.DateFilterType)
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
                        startDate = employeeCTCReportRequestSM.DateFrom;
                        endDate = employeeCTCReportRequestSM.DateTo;
                        break;
                    default:
                        break;
                }

            }
            reportCount = _apiDbContext.ClientEmployeeCTCDetails.Where(x => (((x.StartDateUtc > startDate && x.EndDateUtc < endDate) || (x.StartDateUtc > startDate && x.EndDateUtc < endDate && x.CurrentlyActive == employeeCTCReportRequestSM.CurrentlyActive))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
            return reportCount;
        }

        #endregion --Count--

        #region --My End-Points--

        /// <summary>
        /// Get ClientUserCtcDetails by Employee-Id and Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="empId">Primary Key of ClientEmployeeUser</param>
        /// <returns>Service Model of ClientEmployeeCTCDetail in database of the ClientUserId</returns>

        public async Task<List<ClientEmployeeCTCDetailSM>> GetClientUsersCtcDetailByEmployeeIdOfMyCompany(int currentCompanyId, int empId)
        {
            var dm = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            if (dm.Count > 0)
            {
                var sm = _mapper.Map<List<ClientEmployeeCTCDetailSM>>(dm);
                return sm;
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeCTCDetail not found: {empId}", "CTC Detail for this User Not Found");
            }
        }

        /// <summary>
        /// Get ClientUserCtcDetails by Company-Id
        /// </summary>
        /// <param name="currenCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of ClientEmployeeCTCDetail in database of the ClientCompanyDetailId</returns>

        public async Task<List<ClientEmployeeCTCDetailSM>> GetEmployeesCTCDetailsOfMyCompany(int currenCompanyId)
        {
            var dm = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => x.ClientUser.ClientCompanyDetailId == currenCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeCTCDetailSM>>(dm);
            return sm;
        }

        #endregion --My End-Points--

        #region --Reports--

        /// <summary>
        /// Gets all EmployeeCtcDetail report.
        /// </summary>
        /// <param name="employeeCTCReportRequestSM">EmployeeCTCReportRequestSM object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of List of ClientEmployeeCTCDetailExtendedUser in database.</returns>

        public async Task<List<ClientEmployeeCTCDetailExtendedUserSM>> GetTotalEmployeeCTCReport(EmployeeCTCReportRequestSM employeeCTCReportRequestSM, int currentCompanyId, int skip, int top)
        {
            List<ClientEmployeeCTCDetailDM> clientEmployeeCTCDetailDMs = new List<ClientEmployeeCTCDetailDM>();
            if (skip != -1 && top != -1)
            {
                var today = employeeCTCReportRequestSM.DateFrom;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                switch (employeeCTCReportRequestSM.DateFilterType)
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
                        startDate = employeeCTCReportRequestSM.DateFrom;
                        endDate = employeeCTCReportRequestSM.DateTo;
                        break;
                    default:
                        break;
                }
                employeeCTCReportRequestSM.SearchString = String.IsNullOrWhiteSpace(employeeCTCReportRequestSM.SearchString) ? null : employeeCTCReportRequestSM.SearchString.Trim();
                clientEmployeeCTCDetailDMs = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => (((x.StartDateUtc > startDate && x.EndDateUtc < endDate) || (x.StartDateUtc > startDate && x.EndDateUtc < endDate && x.CurrentlyActive == employeeCTCReportRequestSM.CurrentlyActive) || (x.ClientUser.LoginId.Contains(employeeCTCReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(employeeCTCReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(employeeCTCReportRequestSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
            }
            else
            {
                clientEmployeeCTCDetailDMs = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            }
            List<ClientEmployeeCTCDetailExtendedUserSM> clientEmployeeCTCDetailExtendedUsers = new List<ClientEmployeeCTCDetailExtendedUserSM>();
            foreach (var item in clientEmployeeCTCDetailDMs)
            {
                //var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == item.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                var userName = await _clientUserProcess.GetUserName(item.ClientUserId);
                var ctcDetails = await GetClientEmployeeActiveCtcDetailByEmpId(item.ClientUserId);
                if (ctcDetails != null)
                {

                    clientEmployeeCTCDetailExtendedUsers.Add(new ClientEmployeeCTCDetailExtendedUserSM()
                    {
                        Id = item.Id,
                        UserName = userName,
                        CurrentlyActive = ctcDetails.CurrentlyActive,
                        CurrencyCode = ctcDetails.CurrencyCode,
                        CtcAmount = ctcDetails.CtcAmount,
                        StartDateUTC = ctcDetails.StartDateUTC,
                        EndDateUTC = ctcDetails.EndDateUTC,
                        ClientEmployeePayrollComponents = ctcDetails.ClientEmployeePayrollComponents,
                        ClientUserId = item.ClientUserId,
                        CreatedBy = item.CreatedBy,
                        CreatedOnUTC = item.CreatedOnUTC,
                    });
                }
            }
            return clientEmployeeCTCDetailExtendedUsers;
        }


        /// <summary>
        /// Gets all EmployeeCtcDetail report of an employee.
        /// </summary>
        /// <param name="employeeCTCReportRequestSM">EmployeeCTCReportRequestSM Object.</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeCTCDetailExtendedUser in database.</returns>
        public async Task<List<ClientEmployeeCTCDetailExtendedUserSM>> GetEmployeeCtcReportByClientId(EmployeeCTCReportRequestSM employeeCTCReportRequestSM, int currentCompanyId, int skip, int top)
        {
            var today = employeeCTCReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            switch (employeeCTCReportRequestSM.DateFilterType)
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
                    startDate = employeeCTCReportRequestSM.DateFrom;
                    endDate = employeeCTCReportRequestSM.DateTo;
                    break;
                default:
                    break;
            }
            employeeCTCReportRequestSM.SearchString = String.IsNullOrWhiteSpace(employeeCTCReportRequestSM.SearchString) ? null : employeeCTCReportRequestSM.SearchString.Trim();
            var dm = await _apiDbContext.ClientEmployeeCTCDetails.Where(x => (((x.StartDateUtc > startDate && x.EndDateUtc < endDate) || (x.StartDateUtc > startDate && x.EndDateUtc < endDate && x.CurrentlyActive == employeeCTCReportRequestSM.CurrentlyActive) || (x.ClientUser.LoginId.Contains(employeeCTCReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(employeeCTCReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(employeeCTCReportRequestSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId && x.ClientUserId == employeeCTCReportRequestSM.ClientUserId)).Skip(skip).Take(top).ToListAsync();
            List<ClientEmployeeCTCDetailExtendedUserSM> clientEmployeeCTCDetailExtendedUsers = new List<ClientEmployeeCTCDetailExtendedUserSM>();
            foreach (var item in dm)
            {
                //var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == item.ClientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
                var userName = await _clientUserProcess.GetUserName(item.ClientUserId);
                var ctcDetails = await GetClientEmployeeCTCDetailById(item.Id);
                if (ctcDetails != null)
                {
                    clientEmployeeCTCDetailExtendedUsers.Add(new ClientEmployeeCTCDetailExtendedUserSM()
                    {
                        Id = item.Id,
                        UserName = userName,
                        CurrentlyActive = ctcDetails.CurrentlyActive,
                        CurrencyCode = ctcDetails.CurrencyCode,
                        CtcAmount = ctcDetails.CtcAmount,
                        StartDateUTC = ctcDetails.StartDateUTC,
                        EndDateUTC = ctcDetails.EndDateUTC,
                        ClientEmployeePayrollComponents = ctcDetails.ClientEmployeePayrollComponents,
                        ClientUserId = item.ClientUserId,
                        CreatedBy = item.CreatedBy,
                        CreatedOnUTC = item.CreatedOnUTC,
                    });

                }
            }
            return clientEmployeeCTCDetailExtendedUsers;
        }

        #endregion --Reports--


        #region --Add/Update--

        /// <summary>
        /// Add new ClientEmployeeCTCDetail
        /// </summary>
        /// <param name="clientEmployeeCTCDetailSM">ClientEmployeeCTCDetail object</param>
        /// <returns> the added record</returns>

        public async Task<ClientEmployeeCTCDetailSM?> AddClientEmployeeCTCDetail(ClientEmployeeCTCDetailSM clientEmployeeCTCDetailSM)
        {
            var clientEmployeeCTCDetailDM = _mapper.Map<ClientEmployeeCTCDetailDM>(clientEmployeeCTCDetailSM);
            //List<ClientEmployeeCTCDetailSM> clientEmployeeCTCDetail = new List<ClientEmployeeCTCDetailSM>();
            var sm = _mapper.Map<List<ClientEmployeePayrollComponentDM>>(clientEmployeeCTCDetailSM.ClientEmployeePayrollComponents);
            foreach (var item in sm)
            {
                item.CreatedBy = item.CreatedBy ?? _loginUserDetail.LoginId;
                if (item.CreatedOnUTC == null)
                {
                    item.CreatedOnUTC = DateTime.UtcNow;
                }
            }
            //ClientEmployeeCTCDetailDM clientEmployeeCTCDetailDMs = new ClientEmployeeCTCDetailDM();
            //if (clientEmployeeCTCDetailDM == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    return null;
            //}
            if (sm != null)
            {
                clientEmployeeCTCDetailDM.ClientEmployeePayrollComponents = sm.ToHashSet();
            }
            clientEmployeeCTCDetailDM.CurrentlyActive = true;
            clientEmployeeCTCDetailDM.CreatedBy = _loginUserDetail.LoginId;
            clientEmployeeCTCDetailDM.CreatedOnUTC = DateTime.UtcNow;
            var activeEmployeeCtc = await GetClientEmployeeActiveCtcDetailByEmpId(clientEmployeeCTCDetailSM.ClientUserId);
            if (activeEmployeeCtc != null)
            {
                var dbDM = _apiDbContext.ClientEmployeeCTCDetails.Find(activeEmployeeCtc.Id);
                if (dbDM != null)
                {
                    dbDM.EndDateUtc = clientEmployeeCTCDetailDM.StartDateUtc.AddDays(-1);
                    //if (clientEmployeeCTCDetailDM.StartDateUtc > DateTime.UtcNow)
                    dbDM.CurrentlyActive = false;
                }
            }
            await _apiDbContext.ClientEmployeeCTCDetails.AddAsync(clientEmployeeCTCDetailDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientEmployeeCTCDetailSM>(clientEmployeeCTCDetailDM);

            }
            return null;
        }

        /// <summary>
        /// Update ClientEmployeeCTCDetail of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientEmployeeCTCDetailSM">ClientEmployeeCTCDetail object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeCTCDetailSM> UpdateClientEmployeeCTCDetail(int objIdToUpdate, ClientEmployeeCTCDetailSM clientEmployeeCTCDetailSM)
        {
            if (clientEmployeeCTCDetailSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientEmployeeCTCDetails.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientEmployeeCTCDetailSM.Id = objIdToUpdate;

                    ClientEmployeeCTCDetailDM dbDM = await _apiDbContext.ClientEmployeeCTCDetails.FindAsync(objIdToUpdate);
                    _mapper.Map(clientEmployeeCTCDetailSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientEmployeeCTCDetailSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeCTCDetail not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        ///// <summary>
        ///// Update ClientEmployeeCTCDetail of added record
        ///// </summary>
        ///// <param name="clientEmployeeCTCDetailSM">ClientEmployeeCTCDetail object to update</param>
        ///// <returns>boolean for success in updating record</returns>
        ///// <exception cref="SiffrumPayrollException"></exception>

        //public async Task<List<ClientEmployeeCTCDetailSM>> UpdateActiveEmployeeCTCDetails(List<ClientEmployeeCTCDetailSM> clientEmployeeCTCDetailSM)
        //{
        //    ClientEmployeeCTCDetailDM dbDM = new ClientEmployeeCTCDetailDM();
        //    if (clientEmployeeCTCDetailSM != null)
        //    {
        //        foreach (ClientEmployeeCTCDetailSM item in clientEmployeeCTCDetailSM)
        //        {

        //            var isPresent = await _apiDbContext.ClientEmployeeCTCDetails.AnyAsync(x => x.Id == item.Id);
        //            if (isPresent)
        //            {
        //                dbDM = await _apiDbContext.ClientEmployeeCTCDetails.FindAsync(item.Id);
        //                _mapper.Map(item, dbDM);

        //                dbDM.LastModifiedBy = _loginUserDetail.LoginId;
        //                dbDM.LastModifiedOnUTC = DateTime.UtcNow;
        //            }
        //            else
        //            {
        //                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeCTCDetail not found: {item.Id}", "Data to update not found, add as new instead.");
        //            }
        //        }

        //        if (await _apiDbContext.SaveChangesAsync() > 0)
        //        {
        //            _mapper.Map<ClientEmployeeCTCDetailSM>(dbDM);
        //        }
        //    }
        //    return clientEmployeeCTCDetailSM;
        //}



        /// <summary>
        /// This function is used for updating ctc for employee.
        /// </summary>
        /// <param name="objIdToUpdate">Primary key of ClientEmployeeCtc</param>
        /// <param name="userId">Primary Key of ClientUser</param>
        /// <param name="status">Boolean parameter</param>
        /// <returns>the boolean response.</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<BoolResponseRoot> UpdateActiveEmployeeCTC(int objIdToUpdate, int userId, bool status)
        {
            ClientEmployeeCTCDetailDM clientEmployeeCTCDetailDM = new ClientEmployeeCTCDetailDM();
            if (objIdToUpdate > 0)
            {
                var ctcList = await GetClientEmployeeCtcDetailByEmpId(userId);
                foreach (var item in ctcList)
                {
                    if (item.Id == objIdToUpdate)
                    {
                        clientEmployeeCTCDetailDM = await _apiDbContext.ClientEmployeeCTCDetails.FindAsync(objIdToUpdate);
                        if (clientEmployeeCTCDetailDM != null)
                        {
                            clientEmployeeCTCDetailDM.CurrentlyActive = status;
                            clientEmployeeCTCDetailDM.LastModifiedBy = _loginUserDetail.LoginId;
                            clientEmployeeCTCDetailDM.LastModifiedOnUTC = DateTime.UtcNow;
                        }
                        else
                        {
                            throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeCtc not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                        }
                    }
                    else
                    {
                        clientEmployeeCTCDetailDM = await _apiDbContext.ClientEmployeeCTCDetails.FindAsync(item.Id);
                        if (clientEmployeeCTCDetailDM != null)
                        {
                            clientEmployeeCTCDetailDM.CurrentlyActive = false;
                            clientEmployeeCTCDetailDM.LastModifiedBy = _loginUserDetail.LoginId;
                            clientEmployeeCTCDetailDM.LastModifiedOnUTC = DateTime.UtcNow;
                        }
                    }
                }
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new BoolResponseRoot(true, "Employee CTC Changes Updated Successfully");
                }
            }
            return null;
        }


        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientEmployeeCTCDetail by  Id
        /// </summary>
        /// <param name="id">primary key of ClientEmployeeCTCDetail</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientEmployeeCTCDetail(int id)
        {
            var isPresent = await _apiDbContext.ClientEmployeeCTCDetails.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeCTCDetailDM() { Id = id };
                _apiDbContext.ClientEmployeeCTCDetails.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee CTC Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientUser by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientEmployeeCTCDetailById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeCTCDetails.AnyAsync(x => x.Id == id && x.ClientUser.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeCTCDetailDM() { Id = id };
                _apiDbContext.ClientEmployeeCTCDetails.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee CTC Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--


        #endregion CRUD

    }
}
