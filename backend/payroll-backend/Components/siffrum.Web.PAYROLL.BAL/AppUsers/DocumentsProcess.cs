using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.IO;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class DocumentsProcess : SiffrumPayrollBalOdataBase<DocumentsSM>
    {

        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly ClientUserProcess _clientUserProcess;
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        private readonly ClientCompanyAddressProcess _clientCompanyAddressProcess;

        #endregion --Properties--

        #region --Constructor--

        public DocumentsProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, ClientUserProcess clientEmployeeUserProcess, ClientCompanyDetailProcess clientCompanyDetailProcess, ClientCompanyAddressProcess clientCompanyAddressProcess)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _clientUserProcess = clientEmployeeUserProcess;
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            _clientCompanyAddressProcess = clientCompanyAddressProcess;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<DocumentsSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.Documents;
            IQueryable<DocumentsSM> retSM = await MapEntityAsToQuerable<DocumentsDM, DocumentsSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All Documents details in database
        /// </summary>
        /// <returns>Service Model of List of Documents in database</returns>
        public async Task<List<DocumentsSM>> GetDocuments()
        {
            var dm = await _apiDbContext.Documents.ToListAsync();
            List<DocumentsSM> clientDocument = new List<DocumentsSM>();
            if (dm.Count > 0)
            {
                foreach (var item in dm)
                {
                    string temp_inBase64 = Convert.ToBase64String(item.LetterData);
                    clientDocument.Add(new DocumentsSM()
                    {
                        LetterData = temp_inBase64,
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Extension = item.Extension,
                        LastModifiedOnUTC = item.LastModifiedOnUTC,
                        LastModifiedBy = item.LastModifiedBy
                    });
                }
            }
            return clientDocument;
        }

        /// <summary>
        /// Get All Documents Partial Data details in database
        /// </summary>
        /// <returns>Service Model of List of Documents with Partial Data in database</returns>

        public async Task<List<DocumentsSM>> GetPartialDocuments()
        {
            var dm = await _apiDbContext.Documents.ToListAsync();
            List<DocumentsSM> clientDocument = new List<DocumentsSM>();
            if (dm.Count > 0)
            {
                foreach (var item in dm)
                {
                    clientDocument.Add(new DocumentsSM() { Id = item.Id, Name = item.Name, Description = item.Description, Extension = item.Extension });
                }
            }
            return clientDocument;
        }

        /// <summary>
        /// Get Documents Details by Id
        /// </summary>
        /// <param name="id">Primary Key of Documents</param>
        /// <returns>Service Model of Documents in database of the id</returns>

        public async Task<DocumentsSM> GetLetterDocumentsById(int id)
        {
            var dm = await _apiDbContext.Documents.FindAsync(id);
            if (dm == null)
                return null;
            var document = _mapper.Map<DocumentsSM>(dm);
            document.LetterData = Convert.ToBase64String(dm.LetterData);
            return document;

        }

        /// <summary>
        ///  Get Documents Details by Id 
        /// </summary>
        /// <param name="employeeId">Foreign Key of ClientUser</param>
        /// <param name="letterId">Primary Key of Document</param>
        /// <param name="companyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of Documents in database of the id</returns>

        public async Task<DocumentsSM> GenerateLetterForEmployee(int employeeId, int letterId, int companyId)
        {
            var doc = await _apiDbContext.Documents.Where(x => x.Id == letterId && x.ClientCompanyDetailId == companyId).FirstOrDefaultAsync();
            if (doc == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Letter not found: {letterId}", "Letter not found");
            try
            {

                var emp = await _clientUserProcess.GetClientUserByIdOfMyCompany(employeeId, companyId);
                var companyAddress = await _clientCompanyAddressProcess.GetClientCompanyAddressById(companyId);
                var company = await _clientCompanyDetailProcess.GetClientCompanyDetailById(companyId);

                MemoryStream mss = new MemoryStream(doc.LetterData);
                using (WordprocessingDocument documentReplaingText = WordprocessingDocument.Open(mss, true))
                {
                    var body = documentReplaingText.MainDocumentPart.Document.Body;
                    var paragraphs = body.Elements<Paragraph>();
                    var texts = paragraphs.SelectMany(p => p.Elements<Run>()).SelectMany(r => r.Elements<Text>());

                    foreach (Text text in texts)
                    {
                        switch (text.Text)
                        {
                            case "companyname":
                                text.Text = company?.Name;
                                break;
                            case "companycode":
                                text.Text = company?.CompanyCode;
                                break;
                            case "companyemail":
                                text.Text = company?.CompanyContactEmail;
                                break;
                            case "companywebsite":
                                text.Text = company?.CompanyWebsite;
                                break;
                            case "companyphone":
                                text.Text = company?.CompanyMobileNumber;
                                break;
                            case "companyaddress1":
                                text.Text = companyAddress?.Address1;
                                break;
                            case "companyaddress2":
                                text.Text = companyAddress?.Address2;
                                break;
                            case "companycountry":
                                text.Text = companyAddress?.Country;
                                break;
                            case "companypincode":
                                text.Text = companyAddress?.PinCode;
                                break;
                            case "employeename":
                                text.Text = emp?.LoginId;
                                break;
                            case "employeeemail":
                                text.Text = emp?.EmailId;
                                break;
                            case "employeedesignation":
                                text.Text = emp?.Designation;
                                break;
                            case "employeedateofjoining":
                                text.Text = emp?.DateOfJoining.ToString();
                                break;
                            default:
                                break;
                        }
                    }

                }
                byte[] bytes = mss.ToArray();
                var documentNew = _mapper.Map<DocumentsSM>(doc);
                documentNew.LetterData = Convert.ToBase64String(bytes);

                return documentNew;
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Error in Generating Letter : {ex}", "Error in Generating Letter.");
            }
            return null;

        }

        #endregion --Get--

        #region --My-EndPoints--

        /// <summary>
        /// Get All Documents Partial Data details in database of My Company.
        /// </summary>
        /// <returns>Service Model of List of Documents with Partial Data in database</returns>
        public async Task<List<DocumentsSM>> GetPartialDocumentsOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.Documents.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            List<DocumentsSM> clientDocument = new List<DocumentsSM>();
            if (dm.Count > 0)
            {
                foreach (var item in dm)
                {
                    clientDocument.Add(new DocumentsSM() { Id = item.Id, Name = item.Name, Description = item.Description, Extension = item.Extension });
                }
            }
            return clientDocument;
        }

        /// <summary>
        /// Get Document Based on DocumentId and CompanyId in a database.
        /// </summary>
        /// <param name="id">Primary Key of Document Object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail Object</param>
        /// <returns>Service Model of List of Document in database</returns>
        public async Task<DocumentsSM> GetMyLetterDocumentsById(int id, int currentCompanyId)
        {
            var dm = await _apiDbContext.Documents.Where(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId).FirstOrDefaultAsync();
            if (dm == null)
                return null;
            var document = _mapper.Map<DocumentsSM>(dm);
            document.LetterData = Convert.ToBase64String(dm.LetterData);
            return document;

        }

        #endregion --My-EndPoints--

        #region --Add/Update--

        /// <summary>
        /// Add new Documents
        /// </summary>
        /// <param name="documentsSM">Documents object</param>
        /// <returns> the added record</returns>

        public async Task<DocumentsSM> AddDocuments(DocumentsSM documentsSM)
        {
            DocumentsDM doc = new DocumentsDM();
            doc.LetterData = Convert.FromBase64String(documentsSM.LetterData);
            doc.Description = documentsSM.Description;
            doc.Name = documentsSM.Name;
            doc.Extension = documentsSM.Extension;
            doc.LastModifiedBy = "TODO:LoggedInUserName";
            doc.LastModifiedOnUTC = DateTime.UtcNow;
            doc.CreatedBy = _loginUserDetail.LoginId;
            doc.CreatedOnUTC = DateTime.UtcNow;
            doc.ClientCompanyDetailId = documentsSM.ClientCompanyDetailId;
            await _apiDbContext.Documents.AddAsync(doc);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                documentsSM.Id = doc.Id;
                return _mapper.Map<DocumentsSM>(documentsSM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete Documents by  Id
        /// </summary>
        /// <param name="id">Primary key of Documents</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteLetterDocument(int id)
        {
            var isPresent = await _apiDbContext.Documents.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new DocumentsDM() { Id = id };
                _apiDbContext.Documents.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Document Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientUser by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyLetterDocumentById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.Documents.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new DocumentsDM() { Id = id };
                _apiDbContext.Documents.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Document Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #endregion CRUD

    }
}
