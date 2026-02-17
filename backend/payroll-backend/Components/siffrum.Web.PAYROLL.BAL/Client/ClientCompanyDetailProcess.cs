using Microsoft.AspNetCore.Http;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.IO;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public partial class ClientCompanyDetailProcess : SiffrumPayrollBalOdataBase<ClientCompanyDetailSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly SuperAdminProcess _superAdminProcess;

        #endregion --Properties--

        #region --Constructor--

        public ClientCompanyDetailProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, SuperAdminProcess superAdminProcess)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _superAdminProcess = superAdminProcess;
        }

        #endregion --Constructor--

        #region Odata

        public override async Task<IQueryable<ClientCompanyDetailSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientCompanyDetails;
            IQueryable<ClientCompanyDetailSM> retSM = await MapEntityAsToQuerable<ClientCompanyDetailDM, ClientCompanyDetailSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD 

        #region --Get--

        /// <summary>
        /// Get All Company Details
        /// </summary>
        /// <returns>Service Model of List of ClientCompanyDetail in database</returns>

        public async Task<List<ClientCompanyDetailSM>> GetAllClientCompanyDetails()
        {
            var dm = await _apiDbContext.ClientCompanyDetails.ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyDetailSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get Company Detail based on CompanyId in a database.
        /// </summary>
        /// <param name="id">Primary key of ClientCompanyDetail Object.</param>
        /// <returns>Service Model of List of ClientCompanyDetail in database</returns>

        public async Task<ClientCompanyDetailSM> GetClientCompanyDetailById(int id)
        {
            ClientCompanyDetailDM objDM = await _apiDbContext.ClientCompanyDetails.FindAsync(id);

            if (objDM != null)
            {
                return _mapper.Map<ClientCompanyDetailSM>(objDM);
                //clientCompanyDetailSM.CompanyLogoPath = objDM.CompanyLogoPath == null ? objDM.CompanyLogoPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(objDM.CompanyLogoPath)));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Company Detail based on CompanyCode in a database.
        /// </summary>
        /// <param name="cCode">Primary key of ClientCompanyDetail Object.</param>
        /// <returns>Service Model of List of ClientCompanyDetail in database</returns>

        public async Task<ClientCompanyDetailSM> GetClientCompanyDetailByCompanyCode(string cCode)
        {
            ClientCompanyDetailDM objDM = await _apiDbContext.ClientCompanyDetails.FirstOrDefaultAsync(x => x.CompanyCode.Equals(cCode));
            return objDM != null ? _mapper.Map<ClientCompanyDetailSM>(objDM) : null;
        }


        /// <summary>
        /// Get Company Detail based on CompanyCode in a database.
        /// </summary>
        /// <param name="cCode">Primary key of ClientCompanyDetail Object.</param>
        /// <returns>Service Model of List of ClientCompanyDetail in database</returns>

        public async Task<ClientCompanyDetailSM> GetClientCompanyByEmail(string cEmail)
        {
            ClientCompanyDetailDM objDM = await _apiDbContext.ClientCompanyDetails.FirstOrDefaultAsync(x => x.CompanyContactEmail.Equals(cEmail));
            return objDM != null ? _mapper.Map<ClientCompanyDetailSM>(objDM) : null;
        }

        /// <summary>
        /// Get Company Detail based on CompanyCode in a database.
        /// </summary>
        /// <param name="cCode">CompanyCode of ClientCompanyDetail Object.</param>
        /// <returns>Service Model of List of ClientCompanyDetail in database</returns>

        public async Task<ClientCompanyDetailSM> GetClientCompanyByCompanyCode(string cCode)
        {
            ClientCompanyDetailDM objDM = await _apiDbContext.ClientCompanyDetails.FirstOrDefaultAsync(x => x.CompanyCode.Equals(cCode));
            return objDM != null ? _mapper.Map<ClientCompanyDetailSM>(objDM) : null;
        }


        #region Mine-Company-Logo

        /// <summary>
        /// Gets CompanyLogo of a LoggedIn User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the image the image in the form of base64 from a database</returns>
        public async Task<string> GetMineCompanyLogo(int id)
        {
            var clientUserDM = await _apiDbContext.ClientCompanyDetails.Where(x => x.Id == id).Select(x => new { x.CompanyLogoPath }).FirstOrDefaultAsync();
            var companyLogo = clientUserDM.CompanyLogoPath == null ? clientUserDM.CompanyLogoPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(clientUserDM.CompanyLogoPath)));
            return companyLogo;

        }

        #endregion Mine-Company-Logo

        #endregion --Get--

        #region --Count--

        /// <summary>
        /// Get ClientCompanyDetail Count in a database.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>integer response based on employeeId</returns>

        public async Task<int> GetClientCompanyDetailCountsResponse()
        {
            int resp = _apiDbContext.ClientCompanyDetails.Count();
            return resp;
        }

        #endregion --Count--

        #region --Add Update--

        /// <summary>
        /// Add new ClientCompanyDetail
        /// </summary>
        /// <param name="dummySubjectSM">ClientCompanyDetail Object</param>
        /// <returns>the added Record</returns>

        public async Task<ClientCompanyDetailSM> AddClientCompanyDetail(ClientCompanyDetailSM dummySubjectSM)
        {
            var dummyDM = _mapper.Map<ClientCompanyDetailDM>(dummySubjectSM);
            dummyDM.CreatedBy = _loginUserDetail.LoginId;
            dummyDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientCompanyDetails.AddAsync(dummyDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                var company = await _superAdminProcess.AddDefaultPermissionsForCompany(dummyDM);
                return _mapper.Map<ClientCompanyDetailSM>(dummyDM);
            }
            else
            {
                return null;
            }
        }

        public async Task<ClientCompanyDetailSM> AddClientCompanyDetailForRegistration(ClientCompanyDetailSM dummySubjectSM)
        {
            var dummyDM = _mapper.Map<ClientCompanyDetailDM>(dummySubjectSM);
            dummyDM.CreatedBy = _loginUserDetail.LoginId;
            dummyDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ClientCompanyDetails.AddAsync(dummyDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientCompanyDetailSM>(dummyDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientCompanyDetail of Record Added.
        /// </summary>
        /// <param name="objIdToUpdate">Primary Key of ClientCompanyDetail</param>
        /// <param name="dummySubjectSM">ClientCompanyDetail Object</param>
        /// <returns>the Updated Record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientCompanyDetailSM> UpdateClientCompanyDetail(int objIdToUpdate, ClientCompanyDetailSM dummySubjectSM)
        {
            if (dummySubjectSM != null && objIdToUpdate > 0)
            {
                ClientCompanyDetailDM objDM = await _apiDbContext.ClientCompanyDetails.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    dummySubjectSM.Id = objIdToUpdate;
                    _mapper.Map<ClientCompanyDetailSM, ClientCompanyDetailDM>(dummySubjectSM, objDM);

                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientCompanyDetailSM>(objDM);
                    }

                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientCompanyDetail not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }
        public async Task<string> AddOrUpdateCompanyDetailLogoInDb(int companyId, string webRootPath, IFormFile postedFile)
        {
            var companyDM = await _apiDbContext.ClientCompanyDetails.FirstOrDefaultAsync(x => x.Id == companyId);
            if (companyDM != null)
            {
                var currLogoPath = companyDM.CompanyLogoPath;
                var targetRelativePath = Path.Combine("content\\companies\\logos", $"{companyId}_{Guid.NewGuid()}_original{Path.GetExtension(postedFile.FileName)}");
                var targetPath = Path.Combine(webRootPath, targetRelativePath);
                if (await base.SavePostedFileAtPath(postedFile, targetPath))
                {
                    //Entry Method//
                    //var comp = new ClientCompanyDetailDM() { Id = companyId, CompanyLogoPath = targetRelativePath };
                    //_apiDbContext.ClientCompanyDetails.Attach(comp);
                    //_apiDbContext.Entry(comp).Property(e => e.CompanyLogoPath).IsModified = true;
                    companyDM.CompanyLogoPath = WebExtensions.ConvertFromFilePathToUrl(targetRelativePath);
                    companyDM.LastModifiedBy = _loginUserDetail.LoginId;
                    companyDM.LastModifiedOnUTC = DateTime.UtcNow;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(currLogoPath))
                        { File.Delete(Path.Combine(webRootPath, currLogoPath)); }
                        return WebExtensions.ConvertFromFilePathToUrl(targetRelativePath);
                    }
                }
            }
            return "";
        }

        #endregion --Add Update--

        #region --Add-Update-CompanyLogo--

        /// <summary>
        /// Add or Update Company Logo.
        /// </summary>
        /// <param name="companyLogoPic">Image to be Added or Updated</param>
        /// <param name="currentCompanyId">Id to Update</param>
        /// <returns>The Added/Updated Image</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<string> AddUpdateCompanyLogo(string companyLogoPic, int currentCompanyId)
        {
            if (!String.IsNullOrWhiteSpace(companyLogoPic) && currentCompanyId > 0)
            {
                var objDM = await _apiDbContext.ClientCompanyDetails.FindAsync(currentCompanyId);
                if (objDM != null)
                {
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    Byte[] pictureToUploadBytes = Convert.FromBase64String(companyLogoPic);
                    string companyLogo = objDM.Name + ".jpeg";
                    var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CompanyLogos"));
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, companyLogo);
                    File.WriteAllBytes(filePath, pictureToUploadBytes);
                    objDM.CompanyLogoPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return companyLogoPic;
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientCompanyDetail not found: {currentCompanyId}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add-Update-CompanyLogo--

        #region --Delete CompanyLogo EndPoint--

        /// <summary>
        /// Delete Company Logo by  Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteCompanyLogoById(int currentCompanyId)
        {
            var objDM = await _apiDbContext.ClientCompanyDetails.Where(x => x.Id == currentCompanyId && x.CompanyLogoPath != null).FirstOrDefaultAsync();

            if (objDM != null)
            {

                var companyFilePath = Path.GetFullPath(objDM.CompanyLogoPath);
                var companyFolderPath = Path.GetDirectoryName(companyFilePath);
                if (Directory.Exists(companyFolderPath))
                {
                    File.Delete(companyFilePath);

                    objDM.CompanyLogoPath = null;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new DeleteResponseRoot(true, "Company Logo deleted successfully.");
                    }
                }
                return new DeleteResponseRoot(false, "Company Logo could not be deleted, please try again.");
            }
            return new DeleteResponseRoot(false, "CompanyLogo Not found");

        }

        #endregion --Delete CompanyLogo EndPoint--

        #region --Delete--

        /// <summary>
        /// Deletes the Record Based on ClientCompanyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the ServiceModel of BooleanResponseRoot</returns>

        public async Task<DeleteResponseRoot> DeleteClientCompanyDetailById(int id)
        {
            var isPresent = await _apiDbContext.ClientCompanyDetails.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyDetailDM() { Id = id };
                _apiDbContext.ClientCompanyDetails.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Company Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        public async Task<DeleteResponseRoot> DeleteClientCompanyDetailLogoById(int companyId, string webRootPath)
        {
            var companyDM = await _apiDbContext.ClientCompanyDetails.FirstOrDefaultAsync(x => x.Id == companyId);
            if (companyDM != null)
            {
                var currLogoPath = companyDM.CompanyLogoPath;
                companyDM.CompanyLogoPath = "";
                companyDM.LastModifiedBy = _loginUserDetail.LoginId;
                companyDM.LastModifiedOnUTC = DateTime.UtcNow;

                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    if (!string.IsNullOrWhiteSpace(currLogoPath))
                    {
                        File.Delete(Path.Combine(webRootPath, currLogoPath));
                        return new DeleteResponseRoot(true);
                    }
                }
            }
            return new DeleteResponseRoot(false, "Company or Logo Not found");
        }

        #endregion --Delete--

        #endregion CRUD
    }
}
