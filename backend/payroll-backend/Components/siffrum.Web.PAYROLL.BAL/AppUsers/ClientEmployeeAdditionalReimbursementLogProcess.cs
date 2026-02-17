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
    public class ClientEmployeeAdditionalReimbursementLogProcess : SiffrumPayrollBalOdataBase<ClientEmployeeAdditionalReimbursementLogSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--
        public ClientEmployeeAdditionalReimbursementLogProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientEmployeeAdditionalReimbursementLogSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientEmployeeAdditionalReimbursementLogs;
            IQueryable<ClientEmployeeAdditionalReimbursementLogSM> retSM = await MapEntityAsToQuerable<ClientEmployeeAdditionalReimbursementLogDM, ClientEmployeeAdditionalReimbursementLogSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All ClientEmployeeAdditionalReimbursementLog details in database
        /// </summary>
        /// <returns>Service Model of List of ClientEmployeeAdditionalReimbursementLog in database</returns>
        public async Task<List<ClientEmployeeAdditionalReimbursementLogSM>> GetAllClientEmployeeAdditionalReimbursementLogs()
        {
            var dm = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.ToListAsync();
            List<ClientEmployeeAdditionalReimbursementLogSM> clientEmployeeReimburseDetails = new List<ClientEmployeeAdditionalReimbursementLogSM>();
            foreach (var item in dm)
            {
                clientEmployeeReimburseDetails.Add(new ClientEmployeeAdditionalReimbursementLogSM()
                {
                    Id = item.Id,
                    ReimbursementType = (ReimbursementTypeSM)item.ReimbursementType,
                    ReimbursementAmount = item.ReimbursementAmount,
                    ReimbursementDate = item.ReimbursementDate,
                    ReimbursementDescription = item.ReimbursementDescription,
                    ReimburseDocumentName = item.ReimburseDocumentName,
                    Extension = item.Extension,
                    ReimbursementDocumentPath = item.ReimbursementDocumentPath == null ? item.ReimbursementDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.ReimbursementDocumentPath))),
                    ClientUserId = item.ClientUserId,
                    ClientCompanyDetailId = item.ClientCompanyDetailId,
                });
            }
            //var sm = _mapper.Map<List<ClientEmployeeAdditionalReimbursementLogSM>>(clientEmployeeReimburseDetails);
            return clientEmployeeReimburseDetails;
        }

        /// <summary>
        /// Get ClientEmployeeAdditionalReimbursementLog Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientEmployeeAdditionalReimbursementLog</param>
        /// <returns>Service Model of ClientEmployeeAdditionalReimbursementLog in database of the id</returns>

        public async Task<ClientEmployeeAdditionalReimbursementLogSM> GetClientEmployeeAdditionalReimbursementLogById(int id)
        {
            ClientEmployeeAdditionalReimbursementLogDM? clientEmployeeAdditionalReimbursementLogDM = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.FindAsync(id);

            if (clientEmployeeAdditionalReimbursementLogDM != null)
            {
                var reimbursedocument = _mapper.Map<ClientEmployeeAdditionalReimbursementLogSM>(clientEmployeeAdditionalReimbursementLogDM);
                reimbursedocument.ReimbursementDocumentPath = reimbursedocument.ReimbursementDocumentPath == null ? reimbursedocument.ReimbursementDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(reimbursedocument.ReimbursementDocumentPath)));
                return reimbursedocument;
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeAdditionalReimbursementLog not found: {id}", "EmployeeAdditionalReimbursement for this User Not Found");
            }
        }

        /// <summary>
        /// Get ClientEmployeeAdditionalReimbursementLog Details by EmployeeId
        /// </summary>
        /// <param name="empId">Primary Key of ClientUsers</param>
        /// <returns>Service Model of ClientEmployeeAdditionalReimbursementLog in database of the id</returns>

        public async Task<List<ClientEmployeeAdditionalReimbursementLogSM>> GetClientEmployeeAdditionalReimbursementLogsByUserId(int empId)
        {
            List<ClientEmployeeAdditionalReimbursementLogDM>? clientEmployeeAdditionalReimbursementLogDM = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Where(x => x.ClientUserId == empId).ToListAsync();
            List<ClientEmployeeAdditionalReimbursementLogSM> clientEmployeeReimburseDetails = new List<ClientEmployeeAdditionalReimbursementLogSM>();
            if (clientEmployeeAdditionalReimbursementLogDM.Count > 0)
            {
                foreach (var item in clientEmployeeAdditionalReimbursementLogDM)
                {
                    clientEmployeeReimburseDetails.Add(new ClientEmployeeAdditionalReimbursementLogSM()
                    {
                        Id = item.Id,
                        ReimbursementType = (ReimbursementTypeSM)item.ReimbursementType,
                        ReimbursementAmount = item.ReimbursementAmount,
                        ReimbursementDate = item.ReimbursementDate,
                        ReimbursementDescription = item.ReimbursementDescription,
                        ReimburseDocumentName = item.ReimburseDocumentName,
                        Extension = item.Extension,
                        ReimbursementDocumentPath = item.ReimbursementDocumentPath == null ? item.ReimbursementDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.ReimbursementDocumentPath))),
                        ClientUserId = item.ClientUserId,
                        ClientCompanyDetailId = item.ClientCompanyDetailId,
                    });
                }
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "Reimbursement for this User Not Found");
            }
            return clientEmployeeReimburseDetails;
        }


        #endregion --Get-Region--

        #region --Count--

        /// <summary>
        /// Get  ClientEmployeeAdditionalReimbursementLogs Count by ClientEmployeeUserId
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>
        public async Task<int> GetClientEmployeeAdditionalReimbursementLogsCounts(int empId, int currentCompanyId)
        {
            int resp = _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }
        #endregion --Count--

        #region --My End-Points--

        /// <summary>
        /// Get ClientEmployeeAdditionalReimbursementLog Details by EmployeeId
        /// </summary>
        /// <param name="empId">Primary Key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of ClientEmployeeAdditionalReimbursementLog in database of the id</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<List<ClientEmployeeAdditionalReimbursementLogSM>> GetClientEmployeeAdditionalReimbursementLogsByEmployeeIdOfMyCompany(int empId, int currentCompanyId)
        {
            List<ClientEmployeeAdditionalReimbursementLogDM>? clientEmployeeAdditionalReimbursementLogDM = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Where(x => x.ClientUserId == empId && x.ClientUser.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            List<ClientEmployeeAdditionalReimbursementLogSM> clientEmployeeReimburseDetails = new List<ClientEmployeeAdditionalReimbursementLogSM>();
            if (clientEmployeeAdditionalReimbursementLogDM.Count > 0)
            {
                foreach (var item in clientEmployeeAdditionalReimbursementLogDM)
                {
                    clientEmployeeReimburseDetails.Add(new ClientEmployeeAdditionalReimbursementLogSM()
                    {
                        Id = item.Id,
                        ReimbursementType = (ReimbursementTypeSM)item.ReimbursementType,
                        ReimbursementAmount = item.ReimbursementAmount,
                        ReimbursementDate = item.ReimbursementDate,
                        ReimbursementDescription = item.ReimbursementDescription,
                        ReimburseDocumentName = item.ReimburseDocumentName,
                        Extension = item.Extension,
                        ReimbursementDocumentPath = item.ReimbursementDocumentPath == null ? item.ReimbursementDocumentPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(item.ReimbursementDocumentPath))),
                        ClientUserId = item.ClientUserId,
                        ClientCompanyDetailId = item.ClientCompanyDetailId
                    });
                }
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeUser not found: {empId}", "Reimbursement for this User Not Found");
            }
            return clientEmployeeReimburseDetails;
        }

        #endregion

        #region --Add/Update--

        /// <summary>
        /// Add new ClientEmployeeAdditionalReimbursementLog
        /// </summary>
        /// <param name="clientEmployeeAdditionalReimbursementLogSM">ClientEmployeeAdditionalReimbursementLog object</param>
        /// <returns> the added record</returns>

        public async Task<ClientEmployeeAdditionalReimbursementLogSM> AddClientEmployeeAdditionalReimbursementLog(ClientEmployeeAdditionalReimbursementLogSM clientEmployeeAdditionalReimbursementLogSM)
        {
            ClientEmployeeAdditionalReimbursementLogDM clientEmployeeAdditionalReimbursementLogDM = _mapper.Map<ClientEmployeeAdditionalReimbursementLogDM>(clientEmployeeAdditionalReimbursementLogSM);
            clientEmployeeAdditionalReimbursementLogDM.CreatedBy = _loginUserDetail.LoginId;
            clientEmployeeAdditionalReimbursementLogDM.CreatedOnUTC = DateTime.UtcNow;
            if (!String.IsNullOrWhiteSpace(clientEmployeeAdditionalReimbursementLogSM.ReimbursementDocumentPath))
            {
                Byte[] documentToUploadBytes = Convert.FromBase64String(clientEmployeeAdditionalReimbursementLogSM.ReimbursementDocumentPath);
                string documentName = clientEmployeeAdditionalReimbursementLogDM.ReimburseDocumentName;
                var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\EmployeeDocuments"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string documentFullPath = Path.Combine(folderPath, documentName);
                File.WriteAllBytes(documentFullPath, documentToUploadBytes);
                clientEmployeeAdditionalReimbursementLogDM.ReimbursementDocumentPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), documentFullPath);
                clientEmployeeAdditionalReimbursementLogDM.Extension = clientEmployeeAdditionalReimbursementLogSM.Extension;
            }
            else
            {
                clientEmployeeAdditionalReimbursementLogDM.ReimbursementDocumentPath = null;
                clientEmployeeAdditionalReimbursementLogDM.Extension = null;
            }

            await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.AddAsync(clientEmployeeAdditionalReimbursementLogDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                clientEmployeeAdditionalReimbursementLogSM.Id = clientEmployeeAdditionalReimbursementLogDM.Id;
                return _mapper.Map<ClientEmployeeAdditionalReimbursementLogSM>(clientEmployeeAdditionalReimbursementLogDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientEmployeeAdditionalReimbursementLog of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientEmployeeAdditionalReimbursementLogSM">ClientEmployeeAdditionalReimbursementLog object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientEmployeeAdditionalReimbursementLogSM> UpdateEmployeeAdditionalReimbursementDetail(int objIdToUpdate, ClientEmployeeAdditionalReimbursementLogSM clientEmployeeAdditionalReimbursementLogSM)
        {
            if (clientEmployeeAdditionalReimbursementLogSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientEmployeeAdditionalReimbursementLogSM.Id = objIdToUpdate;

                    ClientEmployeeAdditionalReimbursementLogDM dbDM = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.FindAsync(objIdToUpdate);
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;
                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    _mapper.Map(clientEmployeeAdditionalReimbursementLogSM, dbDM);
                    if (!String.IsNullOrWhiteSpace(clientEmployeeAdditionalReimbursementLogSM.ReimbursementDocumentPath))
                    {
                        Byte[] documentToUploadBytes = Convert.FromBase64String(clientEmployeeAdditionalReimbursementLogSM.ReimbursementDocumentPath);
                        string documentName = dbDM.ReimburseDocumentName;
                        var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\EmployeeDocuments"));
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        string documentFullPath = Path.Combine(folderPath, documentName);
                        File.WriteAllBytes(documentFullPath, documentToUploadBytes);
                        dbDM.ReimbursementDocumentPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), documentFullPath);
                        dbDM.Extension = clientEmployeeAdditionalReimbursementLogSM.Extension;
                    }
                    else
                    {
                        dbDM.ReimbursementDocumentPath = null;
                        dbDM.Extension = null;
                    }

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientEmployeeAdditionalReimbursementLogSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeAdditionalReimbursementLog not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        /// <summary>
        /// Get all ClientEmployeeAdditionalReimbursementLog
        /// </summary>
        /// <param name="employeeAdditionalReimbursementLogReportRequestSM">EmployeeAdditionalReimbursementLogReportRequest Object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of ClientEmployeeLeaves in database.</returns>
        public async Task<List<ClientEmployeeAdditionalReimbursementLogSM>> GetTotalEmployeeAdditionalReimbursementReport(EmployeeAdditionalReimbursementLogReportRequestSM employeeAdditionalReimbursementLogReportRequestSM, int currentCompanyId)
        {
            var today = employeeAdditionalReimbursementLogReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            switch (employeeAdditionalReimbursementLogReportRequestSM.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = employeeAdditionalReimbursementLogReportRequestSM.DateFrom;
                    endDate = employeeAdditionalReimbursementLogReportRequestSM.DateTo;
                    break;
                default:
                    break;
            }
            var dm = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Where(x => ((x.ReimbursementDate > startDate && x.ReimbursementDate < endDate) || (x.ClientUser.FirstName.Contains(employeeAdditionalReimbursementLogReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(employeeAdditionalReimbursementLogReportRequestSM.SearchString)))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeAdditionalReimbursementLogSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ReimbursementReport by UserId in a database.
        /// </summary>
        /// <param name="employeeAdditionalReimbursementLogReportRequestSM">EmployeeAdditionalReimbursementLogReportRequest object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail </param>
        /// <returns></returns>

        public async Task<List<ClientEmployeeAdditionalReimbursementLogSM>> GetTotalEmployeeAdditionalReimbursementReportByUserId(EmployeeAdditionalReimbursementLogReportRequestSM employeeAdditionalReimbursementLogReportRequestSM, int currentCompanyId)
        {
            var today = employeeAdditionalReimbursementLogReportRequestSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            switch (employeeAdditionalReimbursementLogReportRequestSM.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = employeeAdditionalReimbursementLogReportRequestSM.DateFrom;
                    endDate = employeeAdditionalReimbursementLogReportRequestSM.DateTo;
                    break;
                default:
                    break;
            }
            var dm = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Where(x => (((x.ReimbursementDate > startDate && x.ReimbursementDate < endDate) || (x.ClientUser.LoginId.Contains(employeeAdditionalReimbursementLogReportRequestSM.SearchString)) || (x.ClientUser.FirstName.Contains(employeeAdditionalReimbursementLogReportRequestSM.SearchString) || (x.ClientUser.LastName.Contains(employeeAdditionalReimbursementLogReportRequestSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId && x.ClientUserId == employeeAdditionalReimbursementLogReportRequestSM.ClientUserId)).ToListAsync();
            var sm = _mapper.Map<List<ClientEmployeeAdditionalReimbursementLogSM>>(dm);
            return sm;
        }


        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientEmployeeAdditionalReimbursementLog by  Id
        /// </summary>
        /// <param name="id">primary key of ClientEmployeeAdditionalReimbursementLog</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientEmployeeAdditionalReimbursementLog(int id)
        {
            var isPresent = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeAdditionalReimbursementLogDM() { Id = id };
                _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Reimbursement Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientEmployeeAdditionalReimbursementLog by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientEmployeeAdditionalReimbursementLogById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new ClientEmployeeAdditionalReimbursementLogDM() { Id = id };
                _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Employee Reimbursement Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #region --My & Mine Delete-Employee-Reimbursement-Document EndPoint--

        /// <summary>
        /// Delete User Reimbursement Documents by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientEmployeeDocuments</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyReimburseDocumentById(int id, int currentCompanyId)
        {
            var objDM = await _apiDbContext.ClientEmployeeAdditionalReimbursementLogs.Where(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId && x.ReimbursementDocumentPath != null).FirstOrDefaultAsync();

            if (objDM != null)
            {

                var profilePictureFilePath = Path.GetFullPath(objDM.ReimbursementDocumentPath);
                var profilePictureFolderPath = Path.GetDirectoryName(profilePictureFilePath);
                if (Directory.Exists(profilePictureFolderPath))
                {
                    File.Delete(profilePictureFilePath);

                    objDM.ReimbursementDocumentPath = null;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new DeleteResponseRoot(true, $"Employee Additional Reimbursement {objDM.ReimburseDocumentName} Document deleted successfully.");
                    }
                }
                return new DeleteResponseRoot(false, "Employee Additional Reimbursement  could not be deleted, please try again.");
            }
            return new DeleteResponseRoot(false, "Employee Additional Reimbursement  Not found");

        }


        #endregion --My & Mine Delete-Employee-Reimbursement-Document EndPoint--

        #endregion CRUD


    }
}
