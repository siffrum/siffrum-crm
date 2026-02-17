using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.License;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.BAL.License
{
    public class CompanyInvoiceProcess : SiffrumPayrollBalOdataBase<CompanyInvoiceSM>
    {
        #region Properties
        private readonly ILoginUserDetail _loginUserDetail;
        #endregion Properties

        #region Constructor
        public CompanyInvoiceProcess(IMapper mapper, ApiDbContext apiDbContext, ILoginUserDetail loginUserDetail) : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }
        #endregion Constructor

        #region Odata

        /// <summary>
        /// This method gets any UserInvoice(s) by filtering/sorting the data
        /// </summary>
        /// <returns>UserInvoice(s)</returns>
        public override async Task<IQueryable<CompanyInvoiceSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.CompanyInvoices;
            IQueryable<CompanyInvoiceSM> retSM = await base.MapEntityAsToQuerable<CompanyInvoiceDM, CompanyInvoiceSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Count--

        /// <summary>
        /// Get CompanyInvoices Count in database.
        /// </summary>
        /// <returns>integer response</returns>

        public async Task<int> GetAllCompanyInvoicesCountResponse()
        {
            int resp = _apiDbContext.CompanyInvoices.Count();
            return resp;
        }

        #endregion --Count--

        #region Get All
        /// <summary>
        /// Retrieves a list of all user invoices.
        /// </summary>
        /// <returns>A list of CompanyInvoiceSM or null if no invoices are found.</returns>
        public async Task<List<CompanyInvoiceSM>?> GetAllCompanyInvoices()
        {
            try
            {
                var userInvoicesFromDb = await _apiDbContext.CompanyInvoices.ToListAsync();
                if (userInvoicesFromDb == null)
                    return null;
                return _mapper.Map<List<CompanyInvoiceSM>>(userInvoicesFromDb);
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get user invoices, please try again", ex.InnerException);
            }
        }

        #endregion Get All

        #region Get Single

        #region Get By Id
        /// <summary>
        /// Retrieves a user invoice by its unique ID.
        /// </summary>
        /// <param name="Id">The ID of the user invoice to retrieve.</param>
        /// <returns>The CompanyInvoiceSM with the specified ID, or null if not found.</returns>
        public async Task<CompanyInvoiceSM?> GetUserInvoiceById(int Id)
        {
            try
            {
                var singleUserInvoiceFromDb = await _apiDbContext.CompanyInvoices.FindAsync(Id);
                if (singleUserInvoiceFromDb == null)
                    return null;
                return _mapper.Map<CompanyInvoiceSM>(singleUserInvoiceFromDb);
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get user invoice, please try again", ex.InnerException);
            }
        }

        #endregion Get By Id

        #region Get By InvoiceId
        public async Task<CompanyInvoiceSM?> GetSingleInvoiceByStripeInvoiceId(string id)
        {
            CompanyInvoiceDM? UserInvoiceDb = await _apiDbContext.CompanyInvoices.FirstOrDefaultAsync(x => x.StripeInvoiceId == id);
            if (UserInvoiceDb != null)
            {
                var CompanyInvoiceSM = _mapper.Map<CompanyInvoiceSM>(UserInvoiceDb);
                return CompanyInvoiceSM;
            }
            else
            {
                return null;
            }
        }
        #endregion Get By InvoiceId

        #endregion Get Single

        #region Add
        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="userInvoiceSM">The CompanyInvoiceSM to be added.</param>
        /// <returns>The added CompanyInvoiceSM, or null if addition fails.</returns>
        public async Task<CompanyInvoiceSM?> AddUserInvoice(CompanyInvoiceSM userInvoiceSM)
        {
            if (userInvoiceSM == null)
                return null;

            var userInvoiceDM = _mapper.Map<CompanyInvoiceDM>(userInvoiceSM);
            userInvoiceDM.CreatedBy = _loginUserDetail.LoginId;
            userInvoiceDM.CreatedOnUTC = DateTime.UtcNow;

            try
            {
                await _apiDbContext.CompanyInvoices.AddAsync(userInvoiceDM);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<CompanyInvoiceSM>(userInvoiceDM);
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not add user invoice, please try again", ex.InnerException);
            }

            return null;
        }

        #endregion Add

        #region Update
        /// <summary>
        /// Updates a user invoice in the database.
        /// </summary>
        /// <param name="objIdToUpdate">The Id of the job opening to update.</param>
        /// <param name="userInvoiceSM">The updated CompanyInvoiceSM object.</param>
        /// <returns>
        /// If successful, returns the updated CompanyInvoiceSM; otherwise, returns null.
        /// </returns>
        public async Task<CompanyInvoiceSM?> UpdateUserInvoice(int objIdToUpdate, CompanyInvoiceSM userInvoiceSM)
        {
            try
            {
                if (userInvoiceSM != null && objIdToUpdate > 0)
                {
                    //retrieves target user invoice from db
                    CompanyInvoiceDM? objDM = await _apiDbContext.CompanyInvoices.FindAsync(objIdToUpdate);

                    if (objDM != null)
                    {
                        userInvoiceSM.Id = objIdToUpdate;
                        _mapper.Map(userInvoiceSM, objDM);

                        objDM.LastModifiedBy = _loginUserDetail.LoginId;
                        objDM.LastModifiedOnUTC = DateTime.UtcNow;

                        if (await _apiDbContext.SaveChangesAsync() > 0)
                        {
                            return _mapper.Map<CompanyInvoiceSM>(objDM);
                        }
                        return null;
                    }
                    else
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"User invoice not found: {objIdToUpdate}", "User invoice to update not found, add as new instead.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not user invoice, please try again", ex.InnerException);
            }
            return null;
        }

        #endregion Update

        #region Delete
        /// <summary>
        /// Deletes a user invoice by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the user invoice to be deleted.</param>
        /// <returns>A DeleteResponseRoot indicating the result of the deletion operation.</returns>
        public async Task<DeleteResponseRoot> DeleteUserInvoiceById(int id)
        {
            try
            {
                // Check if the product with the specified ID exists in the database
                var isPresent = await _apiDbContext.CompanyInvoices.AnyAsync(x => x.Id == id);

                if (isPresent)
                {
                    // Create an instance of ProductDM with the specified ID for deletion
                    var dmToDelete = new CompanyInvoiceDM() { Id = id };

                    // Remove the user invoice from the database
                    _apiDbContext.CompanyInvoices.Remove(dmToDelete);

                    // Save changes to the database
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        // If deletion is successful, return a success response
                        return new DeleteResponseRoot(true, "User invoice with Id " + id + " deleted successfully!");
                    }
                }

                // If no product was found with the specified ID, return a failure response
                return new DeleteResponseRoot(false, "No such invoice found");
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not delete user invoice, please try again", ex.InnerException);
            }
        }

        #endregion Delete

    }
}
