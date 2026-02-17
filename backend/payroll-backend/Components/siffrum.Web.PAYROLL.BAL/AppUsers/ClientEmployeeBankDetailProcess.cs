using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientEmployeeBankDetailProcess : SiffrumPayrollBalOdataBase<ClientEmployeeBankDetailSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientEmployeeBankDetailProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientEmployeeBankDetailSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientEmployeeBankDetails;
            IQueryable<ClientEmployeeBankDetailSM> retSM = await MapEntityAsToQuerable<ClientEmployeeBankDetailDM, ClientEmployeeBankDetailSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientEmployeeBankDetail details in database
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeBankDetail in database</returns>
        public async Task<List<ClientEmployeeBankDetailSM>> GetAllClientEmployeeBankDetails()
        {
            var dm = await _apiDbContext.ClientEmployeeBankDetails.ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeBankDetailSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeBankDetail Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeBankDetail</param>
        /// <returns>Service Model of ClientEmployeeBankDetail in database of the id</returns>

        public async Task<ClientEmployeeBankDetailSM> GetClientEmployeeBankDetailById(int id)
        {
            ClientEmployeeBankDetailDM clientEmployeeBankDetailDM = await _apiDbContext.ClientEmployeeBankDetails.FindAsync(id);

            if (clientEmployeeBankDetailDM != null)
            {
                return _mapper.Map<ClientEmployeeBankDetailSM>(clientEmployeeBankDetailDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get ClientEmployeeBankDetail Details by Employee-Id
        /// </summary>
        /// <param name="empId">Foreign Key of ClientEmployeeBankDetail</param>
        /// <returns>Service Model of ClientEmployeeBankDetail in database of the ClientUserId</returns>

        public async Task<List<ClientEmployeeBankDetailSM>> GetClientEmployeeBankDetailByEmpId(int empId)
        {
            var clientEmployeeBankDetailDM = await _apiDbContext.ClientEmployeeBankDetails.Where(x => x.ClientUserId == empId).ToListAsync();

            if (clientEmployeeBankDetailDM.Count > 0)
            {
                return _mapper.Map<List<ClientEmployeeBankDetailSM>>(clientEmployeeBankDetailDM);
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "Bankdetail for this User Not Found");
            }
        }

        /// <summary>
        /// Get  ClientEmployeeBankDetails Count by ClientEmployeeUserId
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>

        public async Task<int> GetClientEmployeeBankDetailCounts(int empId, int currentCompanyId)
        {
            int resp = _apiDbContext.ClientEmployeeBankDetails.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        #endregion --Get--

        #region --My-EndPoints--

        /// <summary>
        /// Get ClientEmployeeBankDetail Details by Employee-Id and Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="empId">Primary Key of ClientEmployeeUser</param>
        /// <returns>Service Model of ClientUsersBankDetailByEmployee in database of the ClientUserId</returns>

        public async Task<List<ClientEmployeeBankDetailSM>> GetClientUsersBankDetailByEmployeeIdOfMyCompany(int currentCompanyId, int empId)
        {
            var dm = await _apiDbContext.ClientEmployeeBankDetails.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeBankDetailSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeBankDetail Details by Company-Id
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns>Service Model of ClientUsersBankDetailByEmployee in database of the ClientCompanyDetailId</returns>

        public async Task<List<ClientEmployeeBankDetailSM>> GetEmployeesBankDetailsOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientEmployeeBankDetails.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeBankDetailSM>>(dm);
            return sm;
        }

        #endregion --My-EndPoints--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientEmployeeBankDetail
        /// </summary>
        /// <param name="clientEmployeeBankDetailSM">ClientEmployeeBankDetail object</param>
        /// <returns> the added record</returns>

        public async Task<ClientEmployeeBankDetailSM> AddClientEmployeeBankDetail(ClientEmployeeBankDetailSM clientEmployeeBankDetailSM)
        {
            var clientEmployeeBankDetailDM = _mapper.Map<ClientEmployeeBankDetailDM>(clientEmployeeBankDetailSM);
            clientEmployeeBankDetailDM.CreatedBy = _loginUserDetail.LoginId;
            clientEmployeeBankDetailDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientEmployeeBankDetails.AddAsync(clientEmployeeBankDetailDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientEmployeeBankDetailSM>(clientEmployeeBankDetailDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientEmployeeBankDetail of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientEmployeeBankDetailSM">ClientEmployeeBankDetail object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeBankDetailSM> UpdateClientEmployeeBankDetail(int objIdToUpdate, ClientEmployeeBankDetailSM clientEmployeeBankDetailSM)
        {
            if (clientEmployeeBankDetailSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientEmployeeBankDetails.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientEmployeeBankDetailSM.Id = objIdToUpdate;

                    ClientEmployeeBankDetailDM dbDM = await _apiDbContext.ClientEmployeeBankDetails.FindAsync(objIdToUpdate);
                    _mapper.Map(clientEmployeeBankDetailSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientEmployeeBankDetailSM>(dbDM);
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
        /// Delete ClientEmployeeBankDetail by  Id
        /// </summary>
        /// <param name="id">primary key of ClientEmployeeBankDetail</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientEmployeeBankDetail(int id)
        {
            var isPresent = await _apiDbContext.ClientEmployeeBankDetails.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeBankDetailDM() { Id = id };
                _apiDbContext.ClientEmployeeBankDetails.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Bank Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientUserBankDetail by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientUserBankDetailById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeBankDetails.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            //Linq to sql syntax
            //(from sub in _apiDbContext.ClientUsers  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeBankDetailDM() { Id = id };
                _apiDbContext.ClientEmployeeBankDetails.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Bank Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #endregion CRUD

    }
}
