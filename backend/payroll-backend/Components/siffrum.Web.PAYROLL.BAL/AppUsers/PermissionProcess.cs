using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.BAL.License;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class PermissionProcess : SiffrumPayrollBalOdataBase<PermissionSM>
    {
        #region --Properties--

        private readonly IPasswordEncryptHelper _passwordEncryptHelper;
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        private readonly APIConfiguration _apiConfiguration;
        private readonly CompanyLicenseDetailsProcess _companyLicenseDetailsProcess;

        #endregion --Properties--

        #region --Constructor--

        public PermissionProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext,
            IPasswordEncryptHelper passwordEncryptHelper, ClientCompanyDetailProcess clientCompanyDetailProcess,
            APIConfiguration apiConfiguration, CompanyLicenseDetailsProcess companyLicenseDetailsProcess)
            : base(mapper, apiDbContext)
        {
            _passwordEncryptHelper = passwordEncryptHelper;
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            _apiConfiguration = apiConfiguration;
            _companyLicenseDetailsProcess = companyLicenseDetailsProcess;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<PermissionSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.Permissions;
            IQueryable<PermissionSM> retSM = await MapEntityAsToQuerable<PermissionDM, PermissionSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD 

        #region --Get--
        /// <summary>
        /// Get specific user permission if present, if not present will return company permissions for specified user role
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="userID"></param>
        /// <param name="roleTypeSM"></param>
        /// <returns></returns>
        public async Task<List<PermissionSM>> GetActiveCompanyPermissionByUserId(string companyCode, int userID, RoleTypeSM roleTypeSM)
        
        {
            var company = await _clientCompanyDetailProcess.GetClientCompanyByCompanyCode(companyCode);
            //var activeLicense = await _companyLicenseDetailsProcess.GetActiveLicenseByCompanyId(company.Id);
            CompanyLicenseDetailsSM? activeLicense = null;
            if (_apiConfiguration.IsTestLicenseUsed == true)
            {
                activeLicense = await _companyLicenseDetailsProcess.GetTestPremiumLicense();
            }
            else
            {
                activeLicense = await _companyLicenseDetailsProcess.GetActiveLicenseByCompanyId(company.Id);
            }

            if (activeLicense != null)
            {
                var permissions = await GetModulePermissionsByUserId(company.Id, roleTypeSM, userID);
                return permissions;
            }
            return null;
        }

        /// <summary>
        /// Get specific user permission if present, if not present will return company permissions for specified user role
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="userID"></param>
        /// <param name="roleTypeSM"></param>
        /// <returns></returns>
        public async Task<PermissionSM> GetActiveCompanyPermissionByUserIdAndModuleName(string companyCode, int userID, RoleTypeSM roleTypeSM, ModuleNameSM moduleName)
        {
            var company = await _clientCompanyDetailProcess.GetClientCompanyByCompanyCode(companyCode);
            CompanyLicenseDetailsSM? activeLicense = null;
            if(_apiConfiguration.IsTestLicenseUsed == true)
            {
                activeLicense = await _companyLicenseDetailsProcess.GetTestPremiumLicense();
            }
            else
            {
                activeLicense = await _companyLicenseDetailsProcess.GetActiveLicenseByCompanyId(company.Id);
            }
            if (activeLicense != null)
            {
                var permissions = await GetModulePermissionsByModuleName(company.Id, roleTypeSM, moduleName, userID);
                return permissions;
            }
            return null;
        }

        /// <summary>
        /// Get specific user permission if present, if not present will return company permissions for specified user role
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="userID"></param>
        /// <param name="roleTypeSM"></param>
        /// <returns>The list of permissions of a user.</returns>
        public async Task<List<PermissionSM>> GetModulePermissionsByUserId(int companyId, RoleTypeSM roleType, int clientUserId)
        {
            var userId = await _apiDbContext.Permissions.Where(x => x.ClientCompanyDetailId == companyId && x.RoleType == (RoleTypeDM)roleType && x.ClientUserId == clientUserId).Select(x => x.ClientUserId).FirstOrDefaultAsync();
            List<PermissionSM> permissionSM = new List<PermissionSM>();
            if (userId == null)
            {
                permissionSM = await GetModulesPermissionByCompanyId(companyId, roleType);
            }
            else
            {
                permissionSM = (from p in _apiDbContext.Permissions
                                join companyModules in _apiDbContext.CompanyModules on p.CompanyModulesId equals companyModules.Id
                                where p.ClientCompanyDetailId == companyId && p.RoleType == (RoleTypeDM)roleType && p.ClientUserId == userId
                                select new PermissionSM
                                {
                                    Id = p.Id,
                                    Add = p.Add,
                                    Delete = p.Delete,
                                    Edit = p.Edit,
                                    View = p.View,
                                    IsEnabledForClient = p.IsEnabledForClient,
                                    ModuleName = (ModuleNameSM)companyModules.ModuleName,
                                    ClientCompanyDetailId = (int)p.ClientCompanyDetailId,
                                    CompanyModulesId = (int)p.CompanyModulesId,
                                    ClientUserId = p.ClientUserId,
                                    RoleType = (RoleTypeSM)p.RoleType,
                                    IsStandAlone = (bool)companyModules.StandAlone,
                                }).ToList();
            }
            return permissionSM;
        }


        /// <summary>
        /// This function is used for getting Company Permissions.
        /// </summary>
        /// <param name="companyId">Primary Key of CompanyDetail.</param>
        /// <param name="roleType">Enum of RoleType</param>
        /// <returns></returns>
        public async Task<List<PermissionSM>> GetModulesPermissionByCompanyId(int companyId, RoleTypeSM roleType)
        {
            var permissionList = (from p in _apiDbContext.Permissions
                                  join companyModules in _apiDbContext.CompanyModules on p.CompanyModulesId equals companyModules.Id
                                  where p.ClientCompanyDetailId == companyId && p.RoleType == (RoleTypeDM)roleType && p.ClientUserId == null
                                  select new PermissionSM
                                  {
                                      Id = p.Id,
                                      Add = p.Add,
                                      Delete = p.Delete,
                                      Edit = p.Edit,
                                      View = p.View,
                                      IsEnabledForClient = p.IsEnabledForClient,
                                      ModuleName = (ModuleNameSM)companyModules.ModuleName,
                                      ClientCompanyDetailId = (int)p.ClientCompanyDetailId,
                                      CompanyModulesId = (int)p.CompanyModulesId,
                                      ClientUserId = p.ClientUserId,
                                      RoleType = (RoleTypeSM)p.RoleType
                                  }).ToList();
            return permissionList;
        }

        /// <summary>
        /// This function is used for getting the permission object.
        /// </summary>
        /// <param name="companyId">Primary Key of CompanyDetail.</param>
        /// <param name="roleType">Enum of RoleType</param>
        /// <param name="moduleName">String object</param>
        /// <param name="currentUserId">Primary Key of ClientUser</param>
        /// <returns></returns>
        public async Task<PermissionSM> GetModulePermissionsByModuleName(int companyId, RoleTypeSM roleType, ModuleNameSM moduleName, int currentUserId)
        {
            var userId = await _apiDbContext.Permissions.Where(x => x.ClientCompanyDetailId == companyId && x.RoleType == (RoleTypeDM)roleType && x.ClientUserId == currentUserId).Select(x => x.ClientUserId).FirstOrDefaultAsync();
            PermissionSM? permissionSM = new PermissionSM();
            if (userId == null || userId <= 0)
            {
                permissionSM = (from p in _apiDbContext.Permissions
                                join companyModules in _apiDbContext.CompanyModules on p.CompanyModulesId equals companyModules.Id
                                where p.ClientCompanyDetailId == companyId && p.RoleType == (RoleTypeDM)roleType && p.ClientUserId == null
                                && companyModules.ModuleName == (ModuleNameDM)moduleName
                                select new PermissionSM
                                {
                                    Id = p.Id,
                                    Add = p.Add,
                                    Delete = p.Delete,
                                    Edit = p.Edit,
                                    View = p.View,
                                    IsEnabledForClient = p.IsEnabledForClient,
                                    ModuleName = moduleName,
                                    ClientCompanyDetailId = (int)p.ClientCompanyDetailId,
                                    CompanyModulesId = (int)p.CompanyModulesId,
                                    ClientUserId = p.ClientUserId,
                                    RoleType = (RoleTypeSM)p.RoleType,
                                    IsStandAlone = (bool)companyModules.StandAlone,
                                }).FirstOrDefault();

            }
            else
            {
                permissionSM = (from p in _apiDbContext.Permissions
                                join companyModules in _apiDbContext.CompanyModules on p.CompanyModulesId equals companyModules.Id
                                where p.ClientCompanyDetailId == companyId && p.RoleType == (RoleTypeDM)roleType
                                && p.ClientUserId == currentUserId && companyModules.ModuleName == (ModuleNameDM)moduleName
                                select new PermissionSM
                                {
                                    Id = p.Id,
                                    Add = p.Add,
                                    Delete = p.Delete,
                                    Edit = p.Edit,
                                    View = p.View,
                                    IsEnabledForClient = p.IsEnabledForClient,
                                    ModuleName = moduleName,
                                    ClientCompanyDetailId = (int)p.ClientCompanyDetailId,
                                    CompanyModulesId = (int)p.CompanyModulesId,
                                    ClientUserId = p.ClientUserId,
                                    RoleType = (RoleTypeSM)p.RoleType,
                                    IsStandAlone = (bool)companyModules.StandAlone,
                                }).FirstOrDefault();
            }
            return permissionSM;


        }


        #endregion --Get--



        #region --Add Update--



        #endregion --Add Update--



        #region --Delete--

        #endregion

        #endregion CRUD


    }
}
