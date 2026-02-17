using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientUserAddressProcess : SiffrumPayrollBalOdataBase<ClientUserAddressSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientUserAddressProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientUserAddressSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientUserAddresss;
            IQueryable<ClientUserAddressSM> retSM = await MapEntityAsToQuerable<ClientUserAddressDM, ClientUserAddressSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientUserAddress details in database
        /// </summary>
        /// <returns>Service Model of List of ClientUserAddress in database</returns>
        public async Task<List<ClientUserAddressSM>> GetAllClientUserAddresss()
        {
            var dm = await _apiDbContext.ClientUserAddresss.ToListAsync();
            var sm = _mapper.Map<List<ClientUserAddressSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientUserAddress Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<ClientUserAddressSM> GetClientUserAddressById(int id)
        {
            ClientUserAddressDM clientUserAddressDM = await _apiDbContext.ClientUserAddresss.FindAsync(id);
            if (clientUserAddressDM != null)
            {
                return _mapper.Map<ClientUserAddressSM>(clientUserAddressDM);
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUserAddress not found: {id}", "Address for this User Not Found");
            }
        }

        /// <summary>
        /// Get ClientUserAddress Details by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<ClientUserAddressSM> GetClientUserAddressByUserId(int userId)
        {
            ClientUserAddressDM clientUserAddressDM = await _apiDbContext.ClientUserAddresss.Where(x => x.ClientUserId == userId && x.ClientUserAddressType == ClientUserAddressTypeDM.PermanentAddress || x.ClientUserAddressType == ClientUserAddressTypeDM.PermanentAddress).FirstOrDefaultAsync();
            if (clientUserAddressDM != null)
            {
                return _mapper.Map<ClientUserAddressSM>(clientUserAddressDM);
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUserId not found: {userId}", "Address for this User Not Found");
            }
        }

        /// <summary>
        /// Get ClientUserAddress Details by Employee-Id
        /// </summary>
        /// <param name="empId">Foreign Key of ClientUserAddress</param>
        /// <returns></returns>

        public async Task<List<ClientUserAddressSM>> GetClientAddressByEmpId(int empId, int companyId)
        {
            var clientUserAddressDM = await _apiDbContext.ClientUserAddresss.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == companyId).ToListAsync();

            if (clientUserAddressDM.Count > 0)
            {
                return _mapper.Map<List<ClientUserAddressSM>>(clientUserAddressDM);
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "Address for this User Not Found");
            }
        }

        #endregion --Get--

        #region --Count--

        /// <summary>
        /// Get ClientUserAddress Count by EmployeeId in database.
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns>integer response based on employeeId</returns>

        public async Task<int> GetClientUserAddressCountsResponse(int empId, int currentCompanyId)
        {
            int resp = _apiDbContext.ClientUserAddresss.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        #endregion --Count--

        #region --My End-Points--

        /// <summary>
        /// Get ClientUserAddress Details by Employee-Id and Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="empId">Primary Key of ClientEmployeeUser</param>
        /// <returns>Service Model of ClientUserAddress in database of the ClientUserId</returns>

        public async Task<List<ClientUserAddressSM>> GetClientUsersAddressByEmployeeIdOfMyCompany(int currentCompanyId, int empId)
        {
            var dm = await _apiDbContext.ClientUserAddresss.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientUserAddressSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientUserAddress Details by Id
        /// </summary>
        /// <param name="id"> Primary Key of ClientUserAddress</param>
        /// <param name="companyId">Primary Key of ClientCompanyDetailId</param>
        /// <returns>Service Model of ClientUserAddress in database of the Id</returns>

        public async Task<ClientUserAddressSM> GetClientUserAddressByIdOfMyCompany(int id, int companyId)
        {
            ClientUserAddressDM clientUserAddressDM = await _apiDbContext.ClientUserAddresss.Where(x => x.Id == id && x.ClientUser.ClientCompanyDetailId == companyId).FirstOrDefaultAsync();
            if (clientUserAddressDM != null)
            {
                return _mapper.Map<ClientUserAddressSM>(clientUserAddressDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get ClientUserAddress Details by Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetailId</param>
        /// <returns>Service Model of ClientUserAddress in database of the CompanyId</returns>

        public async Task<List<ClientUserAddressSM>> GetEmployeesAddressOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientUserAddresss.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientUserAddressSM>>(dm);
            return sm;
        }

        #endregion --My End-Points

        #region --Add/Update--

        /// <summary>
        /// Add new ClientUserAddress
        /// </summary>
        /// <param name="clientUserAddressSM">ClientUserAddress object</param>
        /// <returns> the added record</returns>

        public async Task<ClientUserAddressSM> AddClientUserAddress(ClientUserAddressSM clientUserAddressSM)
        {
            var clientUserAddressDM = _mapper.Map<ClientUserAddressDM>(clientUserAddressSM);
            clientUserAddressDM.CreatedBy = _loginUserDetail.LoginId;
            clientUserAddressDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientUserAddresss.AddAsync(clientUserAddressDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientUserAddressSM>(clientUserAddressDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientUserAddress of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientUserAddressSM">ClientUserAddress object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientUserAddressSM> UpdateClientUserAddress(int objIdToUpdate, ClientUserAddressSM clientUserAddressSM)
        {
            if (clientUserAddressSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientUserAddresss.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientUserAddressSM.Id = objIdToUpdate;

                    ClientUserAddressDM dbDM = await _apiDbContext.ClientUserAddresss.FindAsync(objIdToUpdate);
                    _mapper.Map(clientUserAddressSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientUserAddressSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUserAddress not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientUserAddress by  Id
        /// </summary>
        /// <param name="id">primary key of ClientUserAddress</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientUserAddressById(int id)
        {
            var isPresent = await _apiDbContext.ClientUserAddresss.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientUserAddressDM() { Id = id };
                _apiDbContext.ClientUserAddresss.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "ClientUser Address Deleted Successfully");
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
        public async Task<DeleteResponseRoot> DeleteMyClientUserAddressById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientUserAddresss.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientUserAddressDM() { Id = id };
                _apiDbContext.ClientUserAddresss.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "ClientUser Address Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--


        #endregion --Crud--

    }
}
