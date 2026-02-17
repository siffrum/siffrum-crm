using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class ClientCompanyAddressProcess : SiffrumPayrollBalOdataBase<ClientCompanyAddressSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientCompanyAddressProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientCompanyAddressSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientCompanyAddresss;
            IQueryable<ClientCompanyAddressSM> retSM = await MapEntityAsToQuerable<ClientCompanyAddressDM, ClientCompanyAddressSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientCompanyAddress details in database
        /// </summary>
        /// <returns>Service Model of List of ClientCompanyAddress in database</returns>
        public async Task<List<ClientCompanyAddressSM>> GetAllClientCompanyAddresss()
        {
            var dm = await _apiDbContext.ClientCompanyAddresss.ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyAddressSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientCompanyAddress Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<ClientCompanyAddressSM> GetClientCompanyAddressById(int id)
        {
            var clientCompanyAddressDM = await _apiDbContext.ClientCompanyAddresss.Where(x => x.ClientCompanyDetailId == id).FirstOrDefaultAsync();
            if (clientCompanyAddressDM != null)
            {
                return _mapper.Map<ClientCompanyAddressSM>(clientCompanyAddressDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --My-EndPoints--

        /// <summary>
        /// Get ClientCompanyAddress Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<ClientCompanyAddressSM> GetMyClientCompanyAddressById(int id, int currentCompanyId)
        {
            ClientCompanyAddressDM clientCompanyAddressDM = await _apiDbContext.ClientCompanyAddresss.Where(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId).FirstOrDefaultAsync();
            if (clientCompanyAddressDM != null)
            {
                return _mapper.Map<ClientCompanyAddressSM>(clientCompanyAddressDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --My-EndPoints

        #region --Add/Update--

        /// <summary>
        /// Add new ClientCompanyAddress
        /// </summary>
        /// <param name="clientCompanyAddressSM">ClientCompanyAddress object</param>
        /// <returns> the added record</returns>

        public async Task<ClientCompanyAddressSM> AddClientCompanyAddress(ClientCompanyAddressSM clientCompanyAddressSM)
        {
            var clientCompanyAddressDM = _mapper.Map<ClientCompanyAddressDM>(clientCompanyAddressSM);
            clientCompanyAddressDM.CreatedBy = _loginUserDetail.LoginId;
            clientCompanyAddressDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientCompanyAddresss.AddAsync(clientCompanyAddressDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientCompanyAddressSM>(clientCompanyAddressDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientCompanyAddress of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientCompanyAddressSM">ClientCompanyAddress object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientCompanyAddressSM> UpdateClientCompanyAddress(int objIdToUpdate, ClientCompanyAddressSM clientCompanyAddressSM)
        {
            if (clientCompanyAddressSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientCompanyAddresss.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientCompanyAddressSM.Id = objIdToUpdate;

                    ClientCompanyAddressDM dbDM = await _apiDbContext.ClientCompanyAddresss.FindAsync(objIdToUpdate);
                    _mapper.Map(clientCompanyAddressSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientCompanyAddressSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientCompanyAddress not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientCompanyAddress by  Id
        /// </summary>
        /// <param name="id">primary key of ClientCompanyAddress</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientCompanyAddressById(int id)
        {
            var isPresent = await _apiDbContext.ClientCompanyAddresss.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyAddressDM() { Id = id };
                _apiDbContext.ClientCompanyAddresss.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Company Address Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--


        #endregion
    }
}
