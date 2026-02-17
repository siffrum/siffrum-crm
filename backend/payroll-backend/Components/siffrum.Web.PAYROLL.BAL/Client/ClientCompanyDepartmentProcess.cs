using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class ClientCompanyDepartmentProcess : SiffrumPayrollBalOdataBase<ClientCompanyDepartmentSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientCompanyDepartmentProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientCompanyDepartmentSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientCompanyDepartments;
            IQueryable<ClientCompanyDepartmentSM> retSM = await MapEntityAsToQuerable<ClientCompanyDepartmentDM, ClientCompanyDepartmentSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All Client Company Departments details in database
        /// </summary>
        /// <returns>Service Model of List of ClientTheme in database</returns>
        public async Task<List<ClientCompanyDepartmentSM>> GetAllClientCompanyDepartment()
        {
            var dm = await _apiDbContext.ClientCompanyDepartments.ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyDepartmentSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get Client Company Department Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientTheme</param>
        /// <returns>Service Model of ClientCompanyDepartment in database of the id</returns>

        public async Task<ClientCompanyDepartmentSM> GetClientCompanyDepartmentById(int id)
        {
            ClientCompanyDepartmentDM clientCompanyDepartmentDM = await _apiDbContext.ClientCompanyDepartments.FindAsync(id);

            if (clientCompanyDepartmentDM != null)
            {
                return _mapper.Map<ClientCompanyDepartmentSM>(clientCompanyDepartmentDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientCompanyDepartment
        /// </summary>
        /// <param name="clientCompanyDepartmentSM">ClientCompanyDepartment object</param>
        /// <returns> the added record</returns>

        public async Task<ClientCompanyDepartmentSM> AddClientCompanyDepartment(ClientCompanyDepartmentSM clientCompanyDepartmentSM)
        {
            var clientCompanyDepartmentDM = _mapper.Map<ClientCompanyDepartmentDM>(clientCompanyDepartmentSM);
            clientCompanyDepartmentDM.CreatedBy = _loginUserDetail.LoginId;
            clientCompanyDepartmentDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientCompanyDepartments.AddAsync(clientCompanyDepartmentDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientCompanyDepartmentSM>(clientCompanyDepartmentDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientCompanyDepartment of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientCompanyDepartmentSM">ClientCompanyDepartment object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientCompanyDepartmentSM> UpdateClientCompanyDepartment(int objIdToUpdate, ClientCompanyDepartmentSM clientCompanyDepartmentSM)
        {
            if (clientCompanyDepartmentSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientCompanyDepartments.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientCompanyDepartmentSM.Id = objIdToUpdate;

                    ClientCompanyDepartmentDM dbDM = await _apiDbContext.ClientCompanyDepartments.FindAsync(objIdToUpdate);
                    _mapper.Map(clientCompanyDepartmentSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientCompanyDepartmentSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientCompanyDepartment Id not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientCompanydepartment by  Id
        /// </summary>
        /// <param name="id">primary key of ClientCompanyDepartment</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientCompanyDepartmentById(int id)
        {
            var isPresent = await _apiDbContext.ClientCompanyDepartments.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyDepartmentDM() { Id = id };
                _apiDbContext.ClientCompanyDepartments.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Client Company Department Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }



        #endregion --Delete--

        #region --My and Mine Get-Method--
        /// <summary>
        /// Get All My Client Company Departments details in database
        /// </summary>
        /// <returns>Service Model of List of ClientTheme in database</returns>
        public async Task<List<ClientCompanyDepartmentSM>> GetAllMyClientCompanyDepartment(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientCompanyDepartments.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyDepartmentSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get All Mine Client Company Departments details in database
        /// </summary>
        /// <returns>Service Model of List of ClientTheme in database</returns>
        public async Task<ClientCompanyDepartmentSM> GetMineClientCompanyDepartment(int currentCompanyId)
        {
            var result = await (
                                 from clientUser in _apiDbContext.ClientUsers
                                 join companyDepartment in _apiDbContext.ClientCompanyDepartments
                                     on clientUser.ClientCompanyDepartmentId equals companyDepartment.Id
                                 where clientUser.Id == currentCompanyId
                                 select new
                                 {
                                     CompanyDepartment = companyDepartment
                                 }
                                ).FirstOrDefaultAsync();
            var clientCompanyDepartmentDM = result?.CompanyDepartment;
            if (clientCompanyDepartmentDM != null)
            {
                return _mapper.Map<ClientCompanyDepartmentSM>(clientCompanyDepartmentDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --My and Mine Get-Method--

        #region --Report--
        public async Task<List<ClientCompanyDepartmentReportSM>> GetClientCompanyDepartmentReport(int currentCompanyId, int skip, int top)
        {
            List<ClientCompanyDepartmentDM> clientCompanyDepartmentDMs = new List<ClientCompanyDepartmentDM>();
            if (skip != -1 && top != -1)
            {
                clientCompanyDepartmentDMs = await _apiDbContext.ClientCompanyDepartments.Where(x => x.ClientCompanyDetailId == currentCompanyId).Skip(skip).Take(top).ToListAsync();
            }
            else
            {
                clientCompanyDepartmentDMs = await _apiDbContext.ClientCompanyDepartments.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            }
            List<ClientCompanyDepartmentReportSM> clientCompanyDepartmentReports = new List<ClientCompanyDepartmentReportSM>();
            foreach (var item in clientCompanyDepartmentDMs)
            {
                int employeeCount = _apiDbContext.ClientUsers.Count(x => x.ClientCompanyDepartmentId == item.Id);
                clientCompanyDepartmentReports.Add(new ClientCompanyDepartmentReportSM()
                {
                    DepartmentName = item.DepartmentName,
                    EmployeeCount = employeeCount,
                });
            }
            return clientCompanyDepartmentReports;
        }
        #endregion --Report--

    }
}
