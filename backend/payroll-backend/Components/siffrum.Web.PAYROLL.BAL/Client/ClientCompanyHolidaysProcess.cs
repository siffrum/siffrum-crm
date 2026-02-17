using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class ClientCompanyHolidaysProcess : SiffrumPayrollBalOdataBase<ClientCompanyHolidaysSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientCompanyHolidaysProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientCompanyHolidaysSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientCompanyHolidays;
            IQueryable<ClientCompanyHolidaysSM> retSM = await MapEntityAsToQuerable<ClientCompanyHolidaysDM, ClientCompanyHolidaysSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All ClientCompanyHolidays details in database
        /// </summary>
        /// <returns>Service Model of List of ClientCompanyHolidays in database</returns>
        public async Task<List<ClientCompanyHolidaysSM>> GetAllClientCompanyHolidays()
        {
            var dm = await _apiDbContext.ClientCompanyHolidays.ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyHolidaysSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientCompanyHolidays Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientCompanyHolidays</param>
        /// <returns>Service Model of ClientCompanyHolidays in database of the id</returns>

        public async Task<ClientCompanyHolidaysSM> GetClientCompanyHolidaysById(int? id)
        {
            ClientCompanyHolidaysDM clientCompanyHolidaysDM = await _apiDbContext.ClientCompanyHolidays.FindAsync(id);

            if (clientCompanyHolidaysDM != null)
            {
                return _mapper.Map<ClientCompanyHolidaysSM>(clientCompanyHolidaysDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientCompanyHolidays
        /// </summary>
        /// <param name="clientCompanyHolidaysSM">ClientCompanyHolidays object</param>
        /// <returns> the added record</returns>

        public async Task<ClientCompanyHolidaysSM> AddClientCompanyHolidays(ClientCompanyHolidaysSM clientCompanyHolidaysSM)
        {
            var clientCompanyHolidaysDM = _mapper.Map<ClientCompanyHolidaysDM>(clientCompanyHolidaysSM);
            clientCompanyHolidaysDM.CreatedBy = _loginUserDetail.LoginId;
            clientCompanyHolidaysDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientCompanyHolidays.AddAsync(clientCompanyHolidaysDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientCompanyHolidaysSM>(clientCompanyHolidaysDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientCompanyHolidays of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientCompanyHolidaysSM">ClientCompanyHolidays object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientCompanyHolidaysSM> UpdateClientCompanyHolidays(int objIdToUpdate, ClientCompanyHolidaysSM clientCompanyHolidaysSM)
        {
            if (clientCompanyHolidaysSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientCompanyHolidays.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientCompanyHolidaysSM.Id = objIdToUpdate;

                    ClientCompanyHolidaysDM dbDM = await _apiDbContext.ClientCompanyHolidays.FindAsync(objIdToUpdate);
                    _mapper.Map(clientCompanyHolidaysSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientCompanyHolidaysSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientCompanyHolidays Id not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientCompanyHoliday by  Id
        /// </summary>
        /// <param name="id">primary key of ClientCompanyHolidays</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientCompanyHolidaysById(int id)
        {
            var isPresent = await _apiDbContext.ClientCompanyHolidays.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyHolidaysDM() { Id = id };
                _apiDbContext.ClientCompanyHolidays.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "ClientCompany Holidays Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My End-Points--

        /// <summary>
        /// Get All My ClientCompanyHolidays details in database
        /// </summary>
        /// <returns>Service Model of List of ClientCompanyHolidays in database</returns>
        public async Task<List<ClientCompanyHolidaysSM>> GetMyAllClientCompanyHolidays(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientCompanyHolidays.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyHolidaysSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Delete My Client Company Holiday in a Database.
        /// </summary>
        /// <param name="id">Primary Key of ClientCompanyHoliday</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns></returns>
        public async Task<DeleteResponseRoot> MyDeleteClientCompanyHolidaysById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientCompanyHolidays.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyHolidaysDM() { Id = id };
                _apiDbContext.ClientCompanyHolidays.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "ClientCompany Holidays Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My End-Points--

    }
}
