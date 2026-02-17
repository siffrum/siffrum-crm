using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientGenericPayrollComponentProcess : SiffrumPayrollBalOdataBase<ClientGenericPayrollComponentSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientGenericPayrollComponentProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientGenericPayrollComponentSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientGenericPayrollComponents;
            IQueryable<ClientGenericPayrollComponentSM> retSM = await MapEntityAsToQuerable<ClientGenericPayrollComponentDM, ClientGenericPayrollComponentSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientGenericPayrollComponent details in database
        /// </summary>
        /// <returns>Service Model of List of ClientGenericPayrollComponent in database</returns>
        public async Task<List<ClientGenericPayrollComponentSM>> GetAllClientGenericPayrollComponents()
        {
            var dm = await _apiDbContext.ClientGenericPayrollComponents.ToListAsync();
            var sm = _mapper.Map<List<ClientGenericPayrollComponentSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientGenericPayrollComponent Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientGenericPayrollComponent</param>
        /// <returns>Service Model of ClientGenericPayrollComponent in database of the id</returns>

        public async Task<ClientGenericPayrollComponentSM> GetClientGenericPayrollComponentById(int id)
        {
            ClientGenericPayrollComponentDM clientGenericPayrollComponentDM = await _apiDbContext.ClientGenericPayrollComponents.FindAsync(id);

            if (clientGenericPayrollComponentDM != null)
            {
                return _mapper.Map<ClientGenericPayrollComponentSM>(clientGenericPayrollComponentDM);
            }
            else
            {
                return null;
            }
        }


        #endregion --Get--

        #region --COUNT--

        /// <summary>
        /// Get ClientGenericPayrollComponent Count by EmployeeId in database.
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns>integer response based on employeeId</returns>
        public async Task<int> GetClientGenericPayrollComponentCounts(int currentCompanyId)
        {
            int resp = _apiDbContext.ClientGenericPayrollComponents.Where(x => x.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        #endregion --COUNT--

        #region --My End-Points--

        /// <summary>
        /// Get ClientGenericPayrollComponent Details by Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of ClientGenericPayrollComponent in database of the ClientUserId</returns>
        public async Task<List<ClientGenericPayrollComponentSM>> GetClientGenericPayrollComponentOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientGenericPayrollComponents.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientGenericPayrollComponentSM>>(dm);
            return sm;
        }

        #endregion --My End-Points--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientGenericPayrollComponent
        /// </summary>
        /// <param name="clientGenericPayrollComponentSM">ClientGenericPayrollComponent object</param>
        /// <returns> the added record</returns>

        public async Task<ClientGenericPayrollComponentSM> AddClientGenericPayrollComponent(ClientGenericPayrollComponentSM clientGenericPayrollComponentSM)
        {
            var clientGenericPayrollComponentDM = _mapper.Map<ClientGenericPayrollComponentDM>(clientGenericPayrollComponentSM);
            clientGenericPayrollComponentDM.CreatedBy = _loginUserDetail.LoginId;
            clientGenericPayrollComponentDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientGenericPayrollComponents.AddAsync(clientGenericPayrollComponentDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientGenericPayrollComponentSM>(clientGenericPayrollComponentDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientGenericPayrollComponent of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientGenericPayrollComponentSM">ClientGenericPayrollComponent object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientGenericPayrollComponentSM> UpdateClientGenericPayrollComponent(int objIdToUpdate, ClientGenericPayrollComponentSM clientGenericPayrollComponentSM)
        {
            if (clientGenericPayrollComponentSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientGenericPayrollComponents.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientGenericPayrollComponentSM.Id = objIdToUpdate;

                    ClientGenericPayrollComponentDM dbDM = await _apiDbContext.ClientGenericPayrollComponents.FindAsync(objIdToUpdate);
                    _mapper.Map(clientGenericPayrollComponentSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientGenericPayrollComponentSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientGenericPayrollComponent not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientGenericPayrollComponent by  Id
        /// </summary>
        /// <param name="id">primary key of ClientGenericPayrollComponent</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientGenericPayrollComponent(int id)
        {
            var isPresent = await _apiDbContext.ClientGenericPayrollComponents.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientGenericPayrollComponentDM() { Id = id };
                _apiDbContext.ClientGenericPayrollComponents.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Generic Payroll Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientGenericPayrollComponent by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientGenericPayrollComponentById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeDocuments.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientGenericPayrollComponentDM() { Id = id };
                _apiDbContext.ClientGenericPayrollComponents.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Generic Payroll Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #endregion CRUD


        #region My Add/Update EndPoints

        /// <summary>
        /// Add new ClientGenericPayrollComponent
        /// </summary>
        /// <param name="clientGenericPayrollComponentSM">ClientGenericPayrollComponent object</param>
        /// <returns> the added record</returns>

        public async Task<ClientGenericPayrollComponentSM> AddGenericPayrollComponent(ClientGenericPayrollComponentSM clientGenericPayrollComponentSM)
        {
            var genericListItem = await GetClientGenericPayrollComponentOfMyCompany(clientGenericPayrollComponentSM.ClientCompanyDetailId);
            ClientGenericPayrollComponentDM clientGenericPayrollComponentDM = new ClientGenericPayrollComponentDM();
            float percentage = 0;
            foreach (var item in genericListItem)
            {
                percentage = percentage + item.Percentage;
            }
            percentage = percentage + clientGenericPayrollComponentSM.Percentage;
            if (percentage > 100)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Cannot add Component as percentage is greater than 100: {percentage}", $"Cannot add Component as percentage is greater than 100");
            }
            else
            {
                clientGenericPayrollComponentDM = _mapper.Map<ClientGenericPayrollComponentDM>(clientGenericPayrollComponentSM);
                clientGenericPayrollComponentDM.CreatedBy = _loginUserDetail.LoginId;
                clientGenericPayrollComponentDM.CreatedOnUTC = DateTime.UtcNow;
            }

            await _apiDbContext.ClientGenericPayrollComponents.AddAsync(clientGenericPayrollComponentDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientGenericPayrollComponentSM>(clientGenericPayrollComponentDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientGenericPayrollComponent
        /// </summary>
        /// <param name="clientGenericPayrollComponentSM">ClientGenericPayrollComponent object</param>
        /// <returns> the added record</returns>

        public async Task<ClientGenericPayrollComponentSM> UpdateGenericPayrollComponent(int objIdToUpdate, ClientGenericPayrollComponentSM clientGenericPayrollComponentSM)
        {
            float percentage = 0;
            var genericListItem = await GetClientGenericPayrollComponentOfMyCompany(clientGenericPayrollComponentSM.ClientCompanyDetailId);
            ClientGenericPayrollComponentDM clientGenericPayrollComponentDM = new ClientGenericPayrollComponentDM();
            if (clientGenericPayrollComponentSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientGenericPayrollComponents.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientGenericPayrollComponentSM.Id = objIdToUpdate;

                    ClientGenericPayrollComponentDM dbDM = await _apiDbContext.ClientGenericPayrollComponents.FindAsync(objIdToUpdate);
                    foreach (var item in genericListItem)
                    {
                        if (item.Id != dbDM.Id)
                        {
                            percentage = percentage + item.Percentage;
                        }
                    }
                    percentage = percentage + clientGenericPayrollComponentSM.Percentage;
                    if (percentage > 100)
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Cannot add Component as percentage is greater than 100: {percentage}", $"Cannot add Component as percentage is greater than 100");
                    }
                    _mapper.Map(clientGenericPayrollComponentSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientGenericPayrollComponentSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientGenericPayrollComponent not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion My Add/Update EndPoints

    }
}
