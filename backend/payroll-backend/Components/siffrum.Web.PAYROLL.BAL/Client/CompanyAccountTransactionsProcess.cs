using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class CompanyAccountTransactionsProcess : SiffrumPayrollBalOdataBase<CompanyAccountsTransactionSM>
    {

        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public CompanyAccountTransactionsProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<CompanyAccountsTransactionSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.CompanyAccountsTransactions;
            IQueryable<CompanyAccountsTransactionSM> retSM = await MapEntityAsToQuerable<CompanyAccountsTransactionDM, CompanyAccountsTransactionSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All CompanyAccountsTransaction details in database
        /// </summary>
        /// <returns>Service Model of List of CompanyAccountsTransaction in database</returns>
        public async Task<List<CompanyAccountsTransactionSM>> GetAllCompanyAccountTransactions()
        {
            var dm = await _apiDbContext.CompanyAccountsTransactions.ToListAsync();
            var sm = _mapper.Map<List<CompanyAccountsTransactionSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get CompanyAccountsTransaction Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Service Model of ClientEmployeeBankDetail in database of the id</returns>

        public async Task<CompanyAccountsTransactionSM> GetCompanyAccountsTransactionById(int id)
        {
            CompanyAccountsTransactionDM companyAccountsTransactionDM = await _apiDbContext.CompanyAccountsTransactions.FindAsync(id);
            if (companyAccountsTransactionDM != null)
            {
                return _mapper.Map<CompanyAccountsTransactionSM>(companyAccountsTransactionDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --My-EndPoints--

        /// <summary>
        /// Get All CompanyAccountsTransaction details in database of My Company
        /// </summary>
        /// <returns>Service Model of List of CompanyAccountsTransaction in database</returns>
        public async Task<List<CompanyAccountsTransactionSM>> GetCompanyAccountTransactionsOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.CompanyAccountsTransactions.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<CompanyAccountsTransactionSM>>(dm);
            return sm;
        }


        #endregion --My-EndPoints--

        #region --Count--

        /// <summary>
        /// Get All CompanyAccountTransaction Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns>number</returns>
        public async Task<int> GetAllCompanyAccountTransactionCounts(int currentCompanyId)
        {
            int resp = _apiDbContext.CompanyAccountsTransactions.Where(x => x.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        #endregion --Count--

        #region --Add/Update--

        /// <summary>
        /// Add new CompanyAccountsTransaction
        /// </summary>
        /// <param name="companyAccountsTransactionSM">CompanyAccountsTransaction object</param>
        /// <returns> the added record</returns>

        public async Task<CompanyAccountsTransactionSM> AddCompanyAccountsTransaction(CompanyAccountsTransactionSM companyAccountsTransactionSM)
        {
            var companyAccountsTransactionDM = _mapper.Map<CompanyAccountsTransactionDM>(companyAccountsTransactionSM);
            companyAccountsTransactionDM.CreatedBy = _loginUserDetail.LoginId;
            companyAccountsTransactionDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.CompanyAccountsTransactions.AddAsync(companyAccountsTransactionDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<CompanyAccountsTransactionSM>(companyAccountsTransactionDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update CompanyAccountsTransaction of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="companyAccountsTransactionSM">CompanyAccountsTransaction object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<CompanyAccountsTransactionSM> UpdateCompanyAccountsTransaction(int objIdToUpdate, CompanyAccountsTransactionSM companyAccountsTransactionSM)
        {
            if (companyAccountsTransactionSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.CompanyAccountsTransactions.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    companyAccountsTransactionSM.Id = objIdToUpdate;

                    CompanyAccountsTransactionDM dbDM = await _apiDbContext.CompanyAccountsTransactions.FindAsync(objIdToUpdate);
                    _mapper.Map(companyAccountsTransactionSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<CompanyAccountsTransactionSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"CompanyAccountsTransaction not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete CompanyAccountsTransaction by  Id
        /// </summary>
        /// <param name="id">primary key of CompanyAccountsTransaction</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteCompanyAccountsTransactionById(int id)
        {
            var isPresent = await _apiDbContext.CompanyAccountsTransactions.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new CompanyAccountsTransactionDM() { Id = id };
                _apiDbContext.CompanyAccountsTransactions.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Company-Account-Transactions Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #endregion CRUD


    }
}
