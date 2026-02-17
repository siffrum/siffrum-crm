using Microsoft.AspNetCore.Http;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public partial class ApplicationUserProcess : LoginUserProcess<ApplicationUserSM>
    {
        public ApplicationUserProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, loginUserDetail, apiDbContext)
        {
        }

        #region Odata
        public override async Task<IQueryable<ApplicationUserSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ApplicationUsers;
            IQueryable<ApplicationUserSM> retSM = await MapEntityAsToQuerable<ApplicationUserDM, ApplicationUserSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD 

        #region Get
        public async Task<List<ApplicationUserSM>> GetAllApplicationUsers()
        {
            var dm = await _apiDbContext.ApplicationUsers.ToListAsync();
            var sm = _mapper.Map<List<ApplicationUserSM>>(dm);
            return sm;
        }
        public async Task<ApplicationUserSM> GetApplicationUserById(int id)
        {
            ApplicationUserDM applicationUserDM = await _apiDbContext.ApplicationUsers.FindAsync(id);

            if (applicationUserDM != null)
            {
                return _mapper.Map<ApplicationUserSM>(applicationUserDM);
            }
            else
            {
                return null;
            }
        }

        #endregion Get

        #region Add Update
        public async Task<ApplicationUserSM> AddApplicationUser(ApplicationUserSM applicationUserSM)
        {
            var objDM = _mapper.Map<ApplicationUserDM>(applicationUserSM);
            objDM.CreatedBy = _loginUserDetail.LoginId;
            objDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ApplicationUsers.AddAsync(objDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ApplicationUserSM>(objDM);
            }
            else
            {
                return null;
            }
        }
        public async Task<ApplicationUserSM> UpdateApplicationUser(int objIdToUpdate, ApplicationUserSM applicationUserSM)
        {
            if (applicationUserSM != null && objIdToUpdate > 0)
            {
                ApplicationUserDM objDM = await _apiDbContext.ApplicationUsers.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    applicationUserSM.Id = objIdToUpdate;
                    _mapper.Map(applicationUserSM, objDM);

                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ApplicationUserSM>(objDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ApplicationUser not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        public async Task<string> AddOrUpdateProfilePictureInDb(int userId, string webRootPath, IFormFile postedFile)
            => await base.AddOrUpdateProfilePictureInDb(await _apiDbContext.ClientUsers.FirstOrDefaultAsync(x => x.Id == userId), webRootPath, postedFile);

        #endregion Add Update

        #region Delete
        public async Task<DeleteResponseRoot> DeleteApplicationUserById(int id)
        {
            var isPresent = await _apiDbContext.ApplicationUsers.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.ApplicationUsers  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ApplicationUserDM() { Id = id };
                _apiDbContext.ApplicationUsers.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true);
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        public async Task<DeleteResponseRoot> DeleteProfilePictureById(int userId, string webRootPath)
            => await base.DeleteProfilePictureById(await _apiDbContext.ClientUsers.FirstOrDefaultAsync(x => x.Id == userId), webRootPath);

        #endregion Delete

        #endregion CRUD

        #region Private Functions
        #endregion Private Functions

    }
}
