using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.IO;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class ClientEmployeeDocumentProcess : SiffrumPayrollBalOdataBase<ClientEmployeeDocumentSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientEmployeeDocumentProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientEmployeeDocumentSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientEmployeeDocuments;
            IQueryable<ClientEmployeeDocumentSM> retSM = await MapEntityAsToQuerable<ClientEmployeeDocumentDM, ClientEmployeeDocumentSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientEmployeeDocument details in database
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeDocument in database</returns>
        public async Task<List<ClientEmployeeDocumentSM>> GetAllClientEmployeeDocuments()
        {
            var dm = await _apiDbContext.ClientEmployeeDocuments.ToListAsync();
            List<ClientEmployeeDocumentSM> clientEmployeeDocuments = new List<ClientEmployeeDocumentSM>();
            foreach (var item in dm)
            {
                clientEmployeeDocuments.Add(new ClientEmployeeDocumentSM()
                {
                    Id = item.Id,
                    EmployeeDocumentType = (EmployeeDocumentTypeSM)item.EmployeeDocumentType,
                    Name = item.Name,
                    DocumentDescription = item.DocumentDescription,
                    EmployeeDocumentPath = item.EmployeeDocumentPath == null ? item.EmployeeDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.EmployeeDocumentPath))),
                    Extension = item.Extension,
                    ClientUserId = item.ClientUserId,
                    ClientCompanyDetailId = item.ClientCompanyDetailId,
                    CreatedBy = item.CreatedBy,
                    CreatedOnUTC = item.CreatedOnUTC
                });
            }
            var sm = _mapper.Map<List<ClientEmployeeDocumentSM>>(clientEmployeeDocuments);
            return sm;
        }

        /// <summary>
        /// Get ClientEmployeeDocument Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeDocument</param>
        /// <returns>Service Model of ClientEmployeeDocument in database of the id</returns>

        public async Task<ClientEmployeeDocumentSM> GetClientEmployeeDocumentsById(int id)
        {
            var clientEmployeeDocumentDM = await _apiDbContext.ClientEmployeeDocuments.FindAsync(id);

            if (clientEmployeeDocumentDM != null)
            {
                var clientEmployeeDocumentSM = _mapper.Map<ClientEmployeeDocumentSM>(clientEmployeeDocumentDM);
                clientEmployeeDocumentSM.EmployeeDocumentPath = clientEmployeeDocumentDM.EmployeeDocumentPath == null ? clientEmployeeDocumentDM.EmployeeDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(clientEmployeeDocumentDM.EmployeeDocumentPath)));
                return clientEmployeeDocumentSM;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get ClientEmployeeDocument Details by Id
        /// </summary>
        /// <param name="empId">Foreign Key of ClientEmployeeDocument</param>
        /// <returns>Service Model of ClientEmployeeDocument in database of the id</returns>

        public async Task<List<ClientEmployeeDocumentSM>> GetClientEmployeeDocumentsByUserId(int empId)
        {
            List<ClientEmployeeDocumentDM>? clientEmployeeDocumentDMs = await _apiDbContext.ClientEmployeeDocuments.Where(x => x.ClientUserId == empId).ToListAsync();
            List<ClientEmployeeDocumentSM> clientEmployeeDocuments = new List<ClientEmployeeDocumentSM>();
            if (clientEmployeeDocumentDMs.Count > 0)
            {
                foreach (var item in clientEmployeeDocumentDMs)
                {
                    clientEmployeeDocuments.Add(new ClientEmployeeDocumentSM()
                    {
                        Id = item.Id,
                        EmployeeDocumentType = (EmployeeDocumentTypeSM)item.EmployeeDocumentType,
                        Name = item.Name,
                        DocumentDescription = item.DocumentDescription,
                        EmployeeDocumentPath = item.EmployeeDocumentPath == null ? item.EmployeeDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.EmployeeDocumentPath))),
                        Extension = item.Extension,
                        ClientUserId = item.ClientUserId,
                        ClientCompanyDetailId = item.ClientCompanyDetailId,
                        CreatedBy = item.CreatedBy,
                        CreatedOnUTC = item.CreatedOnUTC
                    });
                }
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "Document for this User Not Found");
            }
            return clientEmployeeDocuments;
        }

        /// <summary>
        /// Get All EmployeeDocuments Partial Data details in database
        /// </summary>
        /// <returns>Service Model of List of Employee-Documents with Partial Data in database</returns>

        public async Task<List<ClientEmployeeDocumentSM>> GetPartialEmployeeDocuments()
        {
            var dm = await _apiDbContext.ClientEmployeeDocuments.ToListAsync();
            List<ClientEmployeeDocumentSM> clientEmployeeDocuments = new List<ClientEmployeeDocumentSM>();
            if (dm.Count > 0)
            {
                foreach (var item in dm)
                {
                    clientEmployeeDocuments.Add(new ClientEmployeeDocumentSM() { Id = item.Id, Name = item.Name });
                }
            }
            return clientEmployeeDocuments;
        }

        #endregion --Get--

        #region --Count--

        /// <summary>
        /// Get  ClientEmployeeDocument Count by ClientEmployeeUserId
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>

        public async Task<int> GetClientEmployeeDocumentCounts(int empId, int currentCompanyId)
        {
            int resp = _apiDbContext.ClientEmployeeDocuments.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        #endregion --Count--

        #region --My End-Points--

        /// <summary>
        /// Get ClientUserDocuments by Employee-Id and Company-Id
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="empId">Primary Key of ClientEmployeeUser</param>
        /// <returns>Service Model of ClientUsersDocumentByEmployee in database of the ClientUserId</returns>

        public async Task<List<ClientEmployeeDocumentSM>> GetClientUsersDocumentByEmployeeIdOfMyCompany(int currentCompanyId, int empId)
        {
            List<ClientEmployeeDocumentDM>? clientEmployeeDocumentDMs = await _apiDbContext.ClientEmployeeDocuments.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            List<ClientEmployeeDocumentSM> clientEmployeeDocuments = new List<ClientEmployeeDocumentSM>();
            if (clientEmployeeDocumentDMs.Count > 0)
            {
                foreach (var item in clientEmployeeDocumentDMs)
                {
                    clientEmployeeDocuments.Add(new ClientEmployeeDocumentSM()
                    {
                        Id = item.Id,
                        EmployeeDocumentType = (EmployeeDocumentTypeSM)item.EmployeeDocumentType,
                        Name = item.Name,
                        DocumentDescription = item.DocumentDescription,
                        EmployeeDocumentPath = item.EmployeeDocumentPath == null ? item.EmployeeDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.EmployeeDocumentPath))),
                        Extension = item.Extension,
                        ClientUserId = item.ClientUserId,
                        ClientCompanyDetailId = item.ClientCompanyDetailId,
                        CreatedBy = item.CreatedBy,
                        CreatedOnUTC = item.CreatedOnUTC
                    });
                }
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "Document for this User Not Found");
            }
            return clientEmployeeDocuments;
        }

        /// <summary>
        /// Get ClientEmployeeDocument Details by Company-Id
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns>Service Model of ClientEmployeeDocument in database of the ClientCompanyDetailId</returns>

        public async Task<List<ClientEmployeeDocumentSM>> GetEmployeesDocumentsOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientEmployeeDocuments.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            List<ClientEmployeeDocumentSM> clientEmployeeDocuments = new List<ClientEmployeeDocumentSM>();
            foreach (var item in dm)
            {
                clientEmployeeDocuments.Add(new ClientEmployeeDocumentSM()
                {
                    Id = item.Id,
                    EmployeeDocumentType = (EmployeeDocumentTypeSM)item.EmployeeDocumentType,
                    Name = item.Name,
                    DocumentDescription = item.DocumentDescription,
                    EmployeeDocumentPath = item.EmployeeDocumentPath == null ? item.EmployeeDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.EmployeeDocumentPath))),
                    Extension = item.Extension,
                    ClientUserId = item.ClientUserId,
                    ClientCompanyDetailId = item.ClientCompanyDetailId,
                    CreatedBy = item.CreatedBy,
                    CreatedOnUTC = item.CreatedOnUTC
                });
            }
            var sm = _mapper.Map<List<ClientEmployeeDocumentSM>>(clientEmployeeDocuments);
            return sm;
        }

        /// <summary>
        /// Get All EmployeeDocuments Partial Data details in database of My Company.
        /// </summary>
        /// <returns>Service Model of List of Employee-Documents with Partial Data in database</returns>

        public async Task<List<ClientEmployeeDocumentSM>> GetEmployeesPartialDocumentsOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientEmployeeDocuments.Where(x => x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            List<ClientEmployeeDocumentSM> clientEmployeeDocuments = new List<ClientEmployeeDocumentSM>();
            if (dm.Count > 0)
            {
                foreach (var item in dm)
                {
                    clientEmployeeDocuments.Add(new ClientEmployeeDocumentSM() { Id = item.Id, Name = item.Name });
                }
            }
            return clientEmployeeDocuments;
        }

        /// <summary>
        /// Gets Profile Picture of a LoggedIn User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the image the image in the form of base64 from a database</returns>
        public async Task<ClientEmployeeDocumentSM> GetEmployeeDocumentsByIdOfMyCompany(int id, int currentCompanyId)
        {
            var clientEmployeeDocumentDM = await _apiDbContext.ClientEmployeeDocuments.Where(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId).FirstOrDefaultAsync();
            if (clientEmployeeDocumentDM != null)
            {
                var clientEmployeeDocumentSM = _mapper.Map<ClientEmployeeDocumentSM>(clientEmployeeDocumentDM);
                clientEmployeeDocumentSM.EmployeeDocumentPath = clientEmployeeDocumentDM.EmployeeDocumentPath == null ? clientEmployeeDocumentDM.EmployeeDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(clientEmployeeDocumentDM.EmployeeDocumentPath)));
                return clientEmployeeDocumentSM;

            }
            else
            {
                return null;
            }

        }

        #endregion --My End-Points--

        #region --Add/Update--

        /// <summary>
        /// Add new ClientEmployeeDocument
        /// </summary>
        /// <param name="clientEmployeeDocumentSM">ClientEmployeeDocument object</param>
        /// <returns> the added record</returns>

        public async Task<ClientEmployeeDocumentSM> AddClientEmployeeDocument(ClientEmployeeDocumentSM clientEmployeeDocumentSM)
        {
            ClientEmployeeDocumentDM clientEmployeeDocumentDM = _mapper.Map<ClientEmployeeDocumentDM>(clientEmployeeDocumentSM);
            clientEmployeeDocumentDM.CreatedBy = _loginUserDetail.LoginId;
            clientEmployeeDocumentDM.CreatedOnUTC = DateTime.UtcNow;

            if (!String.IsNullOrWhiteSpace(clientEmployeeDocumentSM.EmployeeDocumentPath))
            {
                Byte[] documentToUploadBytes = Convert.FromBase64String(clientEmployeeDocumentSM.EmployeeDocumentPath);
                string documentName = clientEmployeeDocumentDM.Name;
                var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\EmployeeDocuments"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string documentFullPath = Path.Combine(folderPath, documentName);
                File.WriteAllBytes(documentFullPath, documentToUploadBytes);
                clientEmployeeDocumentDM.EmployeeDocumentPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), documentFullPath);
                clientEmployeeDocumentDM.Extension = clientEmployeeDocumentSM.Extension;
            }
            else
            {
                clientEmployeeDocumentDM.EmployeeDocumentPath = null;
                clientEmployeeDocumentDM.Extension = null;
            }

            await _apiDbContext.ClientEmployeeDocuments.AddAsync(clientEmployeeDocumentDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                clientEmployeeDocumentSM.Id = clientEmployeeDocumentDM.Id;
                return _mapper.Map<ClientEmployeeDocumentSM>(clientEmployeeDocumentDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientEmployeeDocument of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientEmployeeDocumentSM">ClientEmployeeDocument object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeDocumentSM> UpdateEmployeeDocument(int objIdToUpdate, ClientEmployeeDocumentSM clientEmployeeDocumentSM)
        {
            if (clientEmployeeDocumentSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientEmployeeDocuments.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientEmployeeDocumentSM.Id = objIdToUpdate;

                    ClientEmployeeDocumentDM dbDM = await _apiDbContext.ClientEmployeeDocuments.FindAsync(objIdToUpdate);
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;
                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    _mapper.Map(clientEmployeeDocumentSM, dbDM);
                    if (!String.IsNullOrWhiteSpace(clientEmployeeDocumentSM.EmployeeDocumentPath))
                    {
                        Byte[] documentToUploadBytes = Convert.FromBase64String(clientEmployeeDocumentSM.EmployeeDocumentPath);
                        string documentName = clientEmployeeDocumentSM.Name;
                        var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\EmployeeDocuments"));
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        string documentFullPath = Path.Combine(folderPath, documentName);
                        File.WriteAllBytes(documentFullPath, documentToUploadBytes);
                        dbDM.EmployeeDocumentPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), documentFullPath);
                        dbDM.Extension = clientEmployeeDocumentSM.Extension;
                    }
                    else
                    {
                        dbDM.EmployeeDocumentPath = null;
                        dbDM.Extension = null;
                    }

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientEmployeeDocumentSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeDocument not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientEmployeeDocument by  Id
        /// </summary>
        /// <param name="id">primary key of ClientEmployeeDocument</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientEmployeeDocument(int id)
        {
            var isPresent = await _apiDbContext.ClientEmployeeDocuments.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeDocumentDM() { Id = id };
                _apiDbContext.ClientEmployeeDocuments.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Document Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientEmployeeDocument by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientEmployeeDocumentById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeDocuments.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeDocumentDM() { Id = id };
                _apiDbContext.ClientEmployeeDocuments.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Leave Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #region --My & Mine Delete-Employee-Document EndPoint--

        /// <summary>
        /// Delete User Documents by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientEmployeeDocuments</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteEmployeeDocumentsById(int id, int currentCompanyId)
        {
            var objDM = await _apiDbContext.ClientEmployeeDocuments.Where(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId && x.EmployeeDocumentPath != null).FirstOrDefaultAsync();

            if (objDM != null)
            {

                var profilePictureFilePath = Path.GetFullPath(objDM.EmployeeDocumentPath);
                var profilePictureFolderPath = Path.GetDirectoryName(profilePictureFilePath);
                if (Directory.Exists(profilePictureFolderPath))
                {
                    File.Delete(profilePictureFilePath);

                    objDM.EmployeeDocumentPath = null;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new DeleteResponseRoot(true, $"Employee Document : {objDM.Name} deleted successfully.");
                    }
                }
                return new DeleteResponseRoot(false, "Employee Document could not be deleted, please try again.");
            }
            return new DeleteResponseRoot(false, "Employee Document Not found");

        }


        #endregion --My & Mine Delete-Employee-Document EndPoint--

        #endregion CRUD
    }
}
