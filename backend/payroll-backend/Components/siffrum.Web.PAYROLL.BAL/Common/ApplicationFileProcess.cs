using Microsoft.AspNetCore.Http;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.FilesInDb;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.FilesInDb;

namespace Siffrum.Web.Payroll.BAL.Common
{
    public partial class ApplicationFileProcess : SiffrumPayrollBalBase
    {
        private readonly ILoginUserDetail _loginUserDetail;

        public ApplicationFileProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #region CRUD 

        #region Get
        public async Task<ApplicationFileSM> GetApplicationFileById(int id, bool getBytes)
        {
            ApplicationFileDM? applicationFileDM = null;

            if (getBytes)
                applicationFileDM = await _apiDbContext.ApplicationFiles.FirstOrDefaultAsync(x => x.Id == id);
            else
            {
                applicationFileDM = await _apiDbContext.ApplicationFiles.AsNoTracking().Where(x => x.Id == id)
                    .Select((o) => new ApplicationFileDM()
                    {
                        Id = o.Id,
                        CreatedBy = o.CreatedBy,
                        LastModifiedBy = o.LastModifiedBy,
                        CreatedOnUTC = o.CreatedOnUTC,
                        LastModifiedOnUTC = o.LastModifiedOnUTC,
                        FileName = o.FileName,
                        FileType = o.FileType,
                        FileDescription = o.FileDescription
                    }).FirstOrDefaultAsync();

            }
            return _mapper.Map<ApplicationFileSM>(applicationFileDM);
        }

        #endregion Get

        #region Add Update
        public async Task<ApplicationFileSM> AddApplicationFile(ApplicationFileSM applicationFileSM, IFormFile postedFile)
        {
            var objDM = _mapper.Map<ApplicationFileDM>(applicationFileSM);
            objDM.FileBytes = await base.GetPostedFileAsMemoryStream(postedFile);
            objDM.CreatedBy = _loginUserDetail.LoginId;
            objDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ApplicationFiles.AddAsync(objDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                // no need to return bytes in response.
                objDM.FileBytes = null;
                // Detach  this object from EF so no one can save it by mistake
                _apiDbContext.Entry(objDM).State = EntityState.Detached;
                return _mapper.Map<ApplicationFileSM>(objDM);
            }
            else
                return null;
        }
        public async Task<ApplicationFileSM> UpdateApplicationFile(int objIdToUpdate, ApplicationFileSM applicationFileSM, IFormFile? postedFile)
        {
            if (applicationFileSM != null && objIdToUpdate > 0)
            {
                ApplicationFileDM objDM = await _apiDbContext.ApplicationFiles.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    applicationFileSM.Id = objIdToUpdate;
                    _mapper.Map(applicationFileSM, objDM);
                    objDM.FileBytes = await base.GetPostedFileAsMemoryStream(postedFile);
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        // no need to return bytes in response.
                        objDM.FileBytes = null;
                        // Detach  this object from EF so no one can save it by mistake
                        _apiDbContext.Entry(objDM).State = EntityState.Detached;
                        return _mapper.Map<ApplicationFileSM>(objDM);
                    }
                    return null;
                }
                else
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ApplicationFile not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
            }
            return null;
        }

        #endregion Add Update

        #region Delete
        public async Task<DeleteResponseRoot> DeleteApplicationFileById(int id)
        {
            var isPresent = await _apiDbContext.ApplicationFiles.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.ApplicationFiles  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ApplicationFileDM() { Id = id };
                _apiDbContext.ApplicationFiles.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true);
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion Delete

        #endregion CRUD

        #region Private Functions
        #endregion Private Functions

    }
}
