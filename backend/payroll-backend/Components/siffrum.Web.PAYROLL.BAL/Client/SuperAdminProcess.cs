using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class SuperAdminProcess : SiffrumPayrollBalOdataBase<CompanyModulesSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public SuperAdminProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<CompanyModulesSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.CompanyModules;
            IQueryable<CompanyModulesSM> retSM = await MapEntityAsToQuerable<CompanyModulesDM, CompanyModulesSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All Company Modules details in database
        /// </summary>
        /// <returns>Service Model of List of CompanyModulesin database</returns>
        public async Task<List<CompanyModulesSM>> GetGeneralModules()
        {
            var dm = await _apiDbContext.CompanyModules.ToListAsync();
            var sm = _mapper.Map<List<CompanyModulesSM>>(dm);
            return sm;
        }





        #endregion --Get--

        #region ADD/UPDATE MODULE PERMISSION

        /// <summary>
        /// Add Permissions to Modules
        /// </summary>
        /// <param name="permissions">Permission object</param>
        /// <returns> the added record</returns>
        public async Task<BoolResponseRoot> AddModulesPermission(List<PermissionSM> permissions)
        {
            foreach (PermissionSM item in permissions)
            {
                var dbItem = _mapper.Map<PermissionDM>(item);
                dbItem.CreatedBy = _loginUserDetail.LoginId;
                dbItem.CreatedOnUTC = DateTime.UtcNow;
                await _apiDbContext.Permissions.AddAsync(dbItem);
            }
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return new BoolResponseRoot(true, "Permissions Saved Successfully");
            }

            return new BoolResponseRoot(false, "error in saving Permissions");

        }

        /// <summary>
        /// Add Default Permissions for Company Modules.
        /// </summary>
        /// <param name="dummyDM">ClientCompanyDetail Object</param>
        /// <returns>the boolean success in adding the record.</returns>
        public async Task<BoolResponseRoot> AddDefaultPermissionsForCompany(ClientCompanyDetailDM dummyDM)
        {
            var companyModules = await GetGeneralModules();
            for (int i = (int)RoleTypeDM.ClientAdmin; i <= (int)RoleTypeDM.ClientEmployee; i++)
            {
                foreach (var item in companyModules)
                {
                    _apiDbContext.Permissions.Add(new PermissionDM() { RoleType = (RoleTypeDM)i, ClientCompanyDetailId = dummyDM.Id, CompanyModulesId = item.Id, CreatedBy = _loginUserDetail.LoginId, CreatedOnUTC = DateTime.UtcNow });
                }

            }
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return new BoolResponseRoot(true, "Permissions Saved Successfully");
            }
            return new BoolResponseRoot(false, "error in saving Permissions");
        }

        /// <summary>
        /// Update Permisiions of added record Modules.
        /// </summary>
        /// <param name="permissions">Permission object to update</param>
        /// <returns>the Service Model on updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<BoolResponseRoot> UpdateModulesPermission(List<PermissionSM> permissions)
        {
            if (permissions != null)
            {
                foreach (PermissionSM item in permissions)
                {

                    var dbPer = _apiDbContext.Permissions.FirstOrDefault(x => x.CompanyModulesId == item.CompanyModulesId && x.ClientCompanyDetailId == item.ClientCompanyDetailId && (RoleTypeDM)x.RoleType == (RoleTypeDM)item.RoleType);
                    if (dbPer != null)
                    {
                        int _id = dbPer.Id;
                        //var dbDM = await _apiDbContext.Permissions.FindAsync(dbPer.Id);
                        _mapper.Map(item, dbPer);
                        dbPer.Id = _id;
                        dbPer.LastModifiedBy = _loginUserDetail.LoginId;
                        dbPer.LastModifiedOnUTC = DateTime.UtcNow;
                    }
                }

                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new BoolResponseRoot(true, "Permissions Updated Successfully");
                }
            }
            return null;
        }

        /// <summary>
        /// Updates Permissions to Modules for both Employee and Admin.
        /// </summary>
        /// <param name="permissions">Permission Object</param>
        /// <returns>boolean for success in updating record</returns>

        public async Task<BoolResponseRoot> UpdateModulesForSingleUser(List<PermissionSM> permissions)
        {
            PermissionDM dbDM = new PermissionDM();
            if (permissions != null)
            {
                foreach (PermissionSM item in permissions)
                {

                    var isPresent = await _apiDbContext.Permissions.AnyAsync(x => x.Id == item.Id && x.ClientCompanyDetailId == item.ClientCompanyDetailId);
                    if (isPresent)
                    {
                        dbDM = await _apiDbContext.Permissions.FindAsync(item.Id);
                        _mapper.Map(item, dbDM);

                        dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                        dbDM.LastModifiedOnUTC = DateTime.UtcNow;
                    }
                }

                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new BoolResponseRoot(true, "Permissions Updated Successfully");
                }
            }
            return null;
        }

        #endregion ADD/UPDATE MODULE PERMISSION

        #region --Update-LoginUserSetting--

        /// <summary>
        /// Update ClientUser of RoleType Admin of added record
        /// </summary>
        /// <param name="objIdToUpdate"></param>
        /// <param name="loginStatus"></param>
        /// <returns>the Service Model on updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<DeleteResponseRoot> UpdateAdminUserActivationSetting(int objIdToUpdate, LoginStatusSM loginStatus)
        {
            if (objIdToUpdate > 0)
            {
                ClientUserDM objDM = await _apiDbContext.ClientUsers.Where(x => x.Id == objIdToUpdate).FirstOrDefaultAsync();
                if (objDM != null)
                {
                    if (loginStatus == LoginStatusSM.Disabled)
                    {
                        objDM.LoginStatus = LoginStatusDM.Disabled;
                    }
                    else if (loginStatus == LoginStatusSM.Enabled)
                    {
                        objDM.LoginStatus = LoginStatusDM.Enabled;

                    }
                    else
                    {
                        objDM.LoginStatus = LoginStatusDM.PasswordResetRequired;
                    }
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new DeleteResponseRoot(true, "User-Setting Changed Successfully");
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        /// <summary>
        /// Update ClientUser of RoleType Admin of added record
        /// </summary>
        /// <param name="objIdToUpdate">integer object</param>
        /// <param name="isEnabled">boolean object</param>
        /// <returns>the Service Model on updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<DeleteResponseRoot> UpdateCompanyStatusSetting(int objIdToUpdate, bool isEnabled)
        {
            if (objIdToUpdate > 0)
            {
                var objDM = await _apiDbContext.ClientCompanyDetails.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    objDM.IsEnabled = isEnabled;
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new DeleteResponseRoot(true, "Company Status Setting Changed Successfully");
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Update-LoginUserSetting--

        #region --Delete--

        /// <summary>
        /// Delete CompanyModule by  Id
        /// </summary>
        /// <param name="id">primary key of CompanyModule</param>
        /// <returns>boolean for success in removing record</returns>
        public async Task<DeleteResponseRoot> DeleteCompanyModuleById(int id)
        {
            var isPresent = await _apiDbContext.CompanyModules.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var dmToDelete = new CompanyModulesDM() { Id = id };
                _apiDbContext.CompanyModules.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "CompanyModules Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

    }
}
