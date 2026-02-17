using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.License;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.BAL.License
{
    public class LicenseTypeProcess : SiffrumPayrollBalOdataBase<LicenseTypeSM>
    {
        #region Properties
        private readonly ILoginUserDetail _loginUserDetail;
        #endregion Properties

        #region Constructor
        public LicenseTypeProcess(IMapper mapper, ApiDbContext apiDbContext, ILoginUserDetail loginUserDetail) : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion Constructor

        #region Odata
        /// <summary>
        /// This method gets any FeatureGroup(s) by filtering/sorting the data
        /// </summary>
        /// <returns>LicenseType(s)</returns>
        public override async Task<IQueryable<LicenseTypeSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.LicenseTypes;
            IQueryable<LicenseTypeSM> retSM = await base.MapEntityAsToQuerable<LicenseTypeDM, LicenseTypeSM>(_mapper, entitySet);
            return retSM;
        }
        #endregion Odata

        #region --Count--

        /// <summary>
        /// Get FeatureGroup Count in database.
        /// </summary>
        /// <returns>integer response</returns>

        public async Task<int> GetAllLicenseTypeCountResponse()
        {
            int resp = _apiDbContext.LicenseTypes.Count();
            return resp;
        }

        #endregion --Count--

        #region Get All

        /// <summary>
        /// This methods gets all FeatureGroups (List of LicenseType)
        /// </summary>
        /// <returns>All FeatureGroups</returns>
        public async Task<List<LicenseTypeSM>> GetAllLicenseTypes()
        {
            List<LicenseTypeSM> allFeatureGroup = new List<LicenseTypeSM>();
            var _licenseTypeDb = await _apiDbContext.LicenseTypes.ToListAsync();
            foreach (var license in _licenseTypeDb)
            {
                List<int> permissionIds = new List<int>();
                List<PermissionSM> permissionList = new List<PermissionSM>();
                var license_Permissions = await _apiDbContext.LicenseTypes_Permissions.Where(x => x.LicenseTypeId == license.Id).ToListAsync();
                foreach (var licType in license_Permissions)
                {
                    permissionIds.Add((int)licType.PermissionId);
                    //var permissionDM = _apiDbContext.Permissions.FirstOrDefault(x => x.Id == licType.PermissionId);
                    var permissionDM = (from p in _apiDbContext.Permissions
                                        join companyModules in _apiDbContext.CompanyModules on p.CompanyModulesId equals companyModules.Id
                                        where p.Id == licType.PermissionId
                                        select new PermissionSM
                                        {
                                            Id = p.Id,
                                            Add = p.Add,
                                            Delete = p.Delete,
                                            Edit = p.Edit,
                                            View = p.View,
                                            IsEnabledForClient = p.IsEnabledForClient,
                                            ModuleName = (ModuleNameSM)companyModules.ModuleName,
                                            CompanyModulesId = (int)p.CompanyModulesId,
                                            RoleType = (RoleTypeSM)p.RoleType,
                                            IsStandAlone = (bool)companyModules.StandAlone,
                                        }).FirstOrDefault();
                    permissionList.Add(permissionDM);
                }
                var _licenseTypeSM = _mapper.Map<LicenseTypeSM>(license);
                _licenseTypeSM.PermissionIds = permissionIds;
                _licenseTypeSM.Permissions = permissionList;
                allFeatureGroup.Add(_licenseTypeSM);
            }

            return allFeatureGroup;
        }

        /// <summary>
        /// Retrieves a list of all license types.
        /// </summary>
        /// <returns>A list of LicenseTypeSM or null if no subscriptions are found.</returns>
        public async Task<List<LicenseTypeSM>?> GetAllLicenses()
        {
            try
            {
                var licenseTypeDb = await _apiDbContext.LicenseTypes.ToListAsync();
                if (licenseTypeDb == null)
                    return null;
                return _mapper.Map<List<LicenseTypeSM>>(licenseTypeDb);
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get products, please try again", ex.InnerException);
            }
        }

        #endregion Get All

        #region Get Single
        /// <summary>
        /// This method gets a single FeatureGroup on id
        /// </summary>
        /// <param name="id">FeatureGroup Id</param>
        /// <returns>Single FeatureGroup</returns>
        public async Task<LicenseTypeSM?> GetSingleFeatureGroupById(int id)
        {
            LicenseTypeDM? _licenserTypeDb = await _apiDbContext.LicenseTypes.FindAsync(id);

            if (_licenserTypeDb != null)
            {
                List<int> permissionIds = new List<int>();
                var featuresLicenseTypes = await _apiDbContext.LicenseTypes_Permissions.Where(x => x.LicenseTypeId == _licenserTypeDb.Id).ToListAsync();
                foreach (var feature in featuresLicenseTypes)
                {
                    permissionIds.Add((int)feature.PermissionId);
                }
                var _licenseTypeSM = _mapper.Map<LicenseTypeSM>(_licenserTypeDb);
                _licenseTypeSM.PermissionIds = permissionIds;
                return _licenseTypeSM;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method gets a single FeatureGroup on id
        /// </summary>
        /// <param name="id">FeatureGroup Id</param>
        /// <returns>Single FeatureGroup</returns>
        public async Task<LicenseTypeSM?> GetSingleFeatureGroupByStripePriceId(string stripeId)
        {
            LicenseTypeDM? _licenseTypeDb = await _apiDbContext.LicenseTypes.FirstOrDefaultAsync(x => x.StripePriceId == stripeId);

            if (_licenseTypeDb != null)
            {
                List<int> permissionIds = new List<int>();
                var LicenseTypes_Permissions = await _apiDbContext.LicenseTypes_Permissions.Where(x => x.LicenseTypeId == _licenseTypeDb.Id).ToListAsync();
                foreach (var feature in LicenseTypes_Permissions)
                {
                    permissionIds.Add((int)feature.PermissionId);
                }
                var _licenseTypeSM = _mapper.Map<LicenseTypeSM>(_licenseTypeDb);
                _licenseTypeSM.PermissionIds = permissionIds;
                return _licenseTypeSM;
            }
            else
            {
                return null;
            }
        }
        #endregion Get Single

        #region Get Single Extended
        /// <summary>
        /// This method gets a single FeatureGroup on id
        /// </summary>
        /// <param name="id">FeatureGroup Id</param>
        /// <returns>Single FeatureGroup</returns>
        public async Task<LicenseTypeSM?> GetSingleFeatureGroupExtendedById(int id)
        {
            LicenseTypeDM? _licenseTypeDb = await _apiDbContext.LicenseTypes.FindAsync(id);
            if (_licenseTypeDb != null)
            {
                List<int> permissionIds = new List<int>();
                List<PermissionSM> permissionList = new List<PermissionSM>();
                var LicenseTypes_Permissions = await _apiDbContext.LicenseTypes_Permissions.Where(x => x.LicenseTypeId == _licenseTypeDb.Id).ToListAsync();
                foreach (var permission in LicenseTypes_Permissions)
                {
                    permissionIds.Add((int)permission.PermissionId);
                    var permissionDM = _apiDbContext.Permissions.FirstOrDefault(x => x.Id == permission.PermissionId);
                    permissionList.Add(_mapper.Map<PermissionSM>(permissionDM));
                }
                var _licenseTypeSM = _mapper.Map<LicenseTypeSM>(_licenseTypeDb);
                _licenseTypeSM.PermissionIds = permissionIds;
                _licenseTypeSM.Permissions = permissionList;
                return _licenseTypeSM;
            }
            else
            {
                return null;
            }
        }
        #endregion Get Single Extended

        #region --Add/Update--

        /// <summary>
        /// Add new ClientTheme
        /// </summary>
        /// <param name="licenseTypeSM">ClientTheme object</param>
        /// <returns> the added record</returns>

        public async Task<LicenseTypeSM> AddLicensetype(LicenseTypeSM licenseTypeSM)
        {
            var licenseTypeDM = _mapper.Map<LicenseTypeDM>(licenseTypeSM);
            licenseTypeDM.CreatedBy = _loginUserDetail.LoginId;
            licenseTypeDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.LicenseTypes.AddAsync(licenseTypeDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<LicenseTypeSM>(licenseTypeDM);
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
        /// <param name="licenseTypeSM">ClientTheme object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<LicenseTypeSM> UpdateLicenseType(int objIdToUpdate, LicenseTypeSM licenseTypeSM)
        {
            if (licenseTypeSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.LicenseTypes.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    licenseTypeSM.Id = objIdToUpdate;

                    LicenseTypeDM dbDM = await _apiDbContext.LicenseTypes.FindAsync(objIdToUpdate);
                    _mapper.Map(licenseTypeSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<LicenseTypeSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"LicenseTypeSM Id not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
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

        public async Task<DeleteResponseRoot> DeleteLicenseTypeById(int id)
        {
            var isPresent = await _apiDbContext.ClientThemes.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var dmToDelete = new LicenseTypeDM() { Id = id };
                _apiDbContext.LicenseTypes.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "LicenseType Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

    }
}
