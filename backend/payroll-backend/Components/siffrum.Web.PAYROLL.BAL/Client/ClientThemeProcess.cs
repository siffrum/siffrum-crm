using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class ClientThemeProcess : SiffrumPayrollBalOdataBase<ClientThemeSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientThemeProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientThemeSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientThemes;
            IQueryable<ClientThemeSM> retSM = await MapEntityAsToQuerable<ClientThemeDM, ClientThemeSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All ClientTheme details in database
        /// </summary>
        /// <returns>Service Model of List of ClientTheme in database</returns>
        public async Task<List<ClientThemeSM>> GetAllClientThemes()
        {
            var dm = await _apiDbContext.ClientThemes.ToListAsync();
            List<ClientThemeSM> clientThemeSM = new List<ClientThemeSM>();
            foreach (var item in dm)
            {
                clientThemeSM.Add(new ClientThemeSM()
                {
                    Name = item.Name,
                    Id = item.Id,
                });
            }
            return clientThemeSM;
        }

        /// <summary>
        /// Get ClientTheme Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientTheme</param>
        /// <returns>Service Model of ClientTheme in database of the id</returns>

        public async Task<ClientThemeSM> GetClientThemeById(int? id)
        {
            ClientThemeDM clientThemeDM = await _apiDbContext.ClientThemes.FindAsync(id);

            if (clientThemeDM != null)
            {
                return _mapper.Map<ClientThemeSM>(clientThemeDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientTheme
        /// </summary>
        /// <param name="clientThemeSM">ClientTheme object</param>
        /// <returns> the added record</returns>

        public async Task<ClientThemeSM> AddClientTheme(ClientThemeSM clientThemeSM)
        {
            var clientThemeDM = _mapper.Map<ClientThemeDM>(clientThemeSM);
            clientThemeDM.CreatedBy = _loginUserDetail.LoginId;
            clientThemeDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientThemes.AddAsync(clientThemeDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientThemeSM>(clientThemeDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientTheme of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientThemeSM">ClientTheme object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientThemeSM> UpdateClientTheme(int objIdToUpdate, ClientThemeSM clientThemeSM)
        {
            if (clientThemeSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientThemes.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientThemeSM.Id = objIdToUpdate;

                    ClientThemeDM dbDM = await _apiDbContext.ClientThemes.FindAsync(objIdToUpdate);
                    _mapper.Map(clientThemeSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientThemeSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientTheme Id not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientTheme by  Id
        /// </summary>
        /// <param name="id">primary key of ClientTheme</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientThemeById(int id)
        {
            var isPresent = await _apiDbContext.ClientThemes.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var dmToDelete = new ClientThemeDM() { Id = id };
                _apiDbContext.ClientThemes.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Client Theme Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --Client Theme Get-Methods--

        /// <summary>
        /// Get Default ClientTheme Details
        /// </summary>
        /// <returns>Service Model of ClientTheme in database of the Default Theme</returns>

        public async Task<ClientThemeSM> GetDefaultClientTheme()
        {
            ClientThemeDM? clientThemeDM = await _apiDbContext.ClientThemes.Where(x => x.IsSelected == true).FirstOrDefaultAsync();

            if (clientThemeDM != null)
            {
                return _mapper.Map<ClientThemeSM>(clientThemeDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get ClientUser Detail Based on ClientUserId in a database.
        /// </summary>
        /// <param name="id">Primary Key of ClientUser Object</param>
        /// <returns>Service Model of List of ClientUser in database</returns>
        public async Task<ClientThemeSM> GetMineClientTheme(int id)
        {
            ClientUserDM? clientUserDM = await _apiDbContext.ClientUsers.Where(x => x.Id == id && x.UserSettingId != null).FirstOrDefaultAsync();

            if (clientUserDM != null)
            {
                var themeResponsSM = await GetClientThemeById(clientUserDM.UserSettingId);
                return themeResponsSM;
            }
            else
            {
                var defaultTheme = await GetDefaultClientTheme();
                return defaultTheme;
            }

        }

        #endregion --Themes Get-Method--

    }
}
