using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.License;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.BAL.License
{
    public class CompanyLicenseDetailsProcess : SiffrumPayrollBalOdataBase<CompanyLicenseDetailsSM>
    {
        #region Properties
        private readonly ILoginUserDetail _loginUserDetail;
        //private readonly ClientUserProcess _clientUserProcess;
        private readonly SuperAdminProcess _superAdminProcess;
        private readonly APIConfiguration _apiConfiguration;
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        //private readonly FeatureProcess _featureProcess;
        #endregion Properties

        #region Constructor
        public CompanyLicenseDetailsProcess(IMapper mapper, ApiDbContext apiDbContext,
            ILoginUserDetail loginUserDetail, SuperAdminProcess superAdminProcess, APIConfiguration apiConfiguration,
            ClientCompanyDetailProcess clientCompanyDetailProcess) : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            //_clientUserProcess = clientUserProcess;
            _superAdminProcess = superAdminProcess;
            _apiConfiguration = apiConfiguration;
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            //_featureProcess = featureProcess;
        }
        #endregion Constructor

        #region Odata
        /// <summary>
        /// This method gets any UserLicenseDetail(s) by filtering/sorting the data
        /// </summary>
        /// <returns>UserLicenseDetail(s)</returns>
        public override async Task<IQueryable<CompanyLicenseDetailsSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.CompanyLicenseDetails;
            IQueryable<CompanyLicenseDetailsSM> retSM = await base.MapEntityAsToQuerable<CompanyLicenseDetailsDM, CompanyLicenseDetailsSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region Get All
        /// <summary>
        /// Retrieves a list of all user subscriptions.
        /// </summary>
        /// <returns>A list of CompanyLicenseDetailsSM or null if no subscriptions are found.</returns>
        public async Task<List<CompanyLicenseDetailsSM>?> GetAllCompanyLicenseDetails()
        {
            try
            {
                var productsFromDb = await _apiDbContext.CompanyLicenseDetails.ToListAsync();
                if (productsFromDb == null)
                    return null;
                return _mapper.Map<List<CompanyLicenseDetailsSM>>(productsFromDb);
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get products, please try again", ex.InnerException);
            }
        }

        #endregion Get All

        #region Get Single

        #region Get By UserId
        /// <summary>
        /// Retrieves a user subscription by its UserID.
        /// </summary>
        /// <param name="Id">The ID of the user subscription to retrieve.</param>
        /// <returns>The CompanyLicenseDetailsSM with the specified ID, or null if not found.</returns>
        public async Task<CompanyLicenseDetailsSM?> GetUserSubscriptionByCompanyId(int cId)
        {
            try
            {
                var singleUserSubscriptionFromDb = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.ClientCompanyDetailId == cId);
                if (singleUserSubscriptionFromDb == null)
                    return null;
                return _mapper.Map<CompanyLicenseDetailsSM>(singleUserSubscriptionFromDb);
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get the product, please try again", ex.InnerException);
            }
        }
        #endregion Get By UserId

        #region Get By Id

        /// <summary>
        /// Retrieves a user subscription by its unique ID.
        /// </summary>
        /// <param name="Id">The ID of the user subscription to retrieve.</param>
        /// <returns>The CompanyLicenseDetailsSM with the specified ID, or null if not found.</returns>
        public async Task<CompanyLicenseDetailsSM?> GetUserSubscriptionById(int Id)
        {
            try
            {
                var singleUserSubscriptionFromDb = await _apiDbContext.CompanyLicenseDetails.FindAsync(Id);
                if (singleUserSubscriptionFromDb == null)
                    return null;
                return _mapper.Map<CompanyLicenseDetailsSM>(singleUserSubscriptionFromDb);
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get the product, please try again", ex.InnerException);
            }
        }

        #endregion Get By Id

        #region Get By Stripe Customer ID
        /// <summary>
        /// Retrieves a user subscription by its stripeCustomerId
        /// </summary>
        /// <param name="stripeCustomerId">The stripeCustomerID of the user subscription to retrieve.</param>
        /// <returns>The CompanyLicenseDetailsSM with the specified ID, or null if not found.</returns>
        public async Task<CompanyLicenseDetailsSM?> GetUserSubscriptionByStripeCustomerId(string stripeCustomerId)
        {
            //try
            //{
            var singleUserSubscriptionFromDb = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.StripeCustomerId == stripeCustomerId);
            if (singleUserSubscriptionFromDb == null)
                return null;
            return _mapper.Map<CompanyLicenseDetailsSM>(singleUserSubscriptionFromDb);
            //}
            //catch (Exception ex)
            //{
            //    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not get the product, please try again", ex.InnerException);
            //}
        }
        #endregion Get By Stripe Customer ID


        #endregion Get Single

        #region Add
        /// <summary>
        /// Adds a new user subscription to the database.
        /// </summary>
        /// <param name="CompanyLicenseDetailsSM">The CompanyLicenseDetailsSM to be added.</param>
        /// <returns>The added CompanyLicenseDetailsSM, or null if addition fails.</returns>
        public async Task<CompanyLicenseDetailsSM?> AddUserSubscription(CompanyLicenseDetailsSM CompanyLicenseDetailsSM)
        {
            if (CompanyLicenseDetailsSM == null)
                return null;

            var CompanyLicenseDetailsDM = _mapper.Map<CompanyLicenseDetailsDM>(CompanyLicenseDetailsSM);
            CompanyLicenseDetailsDM.CreatedBy = _loginUserDetail.LoginId;
            CompanyLicenseDetailsDM.CreatedOnUTC = DateTime.UtcNow;

            try
            {
                await _apiDbContext.CompanyLicenseDetails.AddAsync(CompanyLicenseDetailsDM);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<CompanyLicenseDetailsSM>(CompanyLicenseDetailsDM);
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not add user subscription, please try again", ex.InnerException);
            }

            return null;
        }

        #endregion Add

        #region Update
        /// <summary>
        /// Updates a user subscription in the database.
        /// </summary>
        /// <param name="objIdToUpdate">The Id of the job opening to update.</param>
        /// <param name="CompanyLicenseDetailsSM">The updated CompanyLicenseDetailsSM object.</param>
        /// <returns>
        /// If successful, returns the updated CompanyLicenseDetailsSM; otherwise, returns null.
        /// </returns>
        public async Task<CompanyLicenseDetailsSM?> UpdateUserSubscription(int objIdToUpdate, CompanyLicenseDetailsSM CompanyLicenseDetailsSM)
        {
            try
            {
                if (CompanyLicenseDetailsSM != null && objIdToUpdate > 0)
                {
                    //retrieves target user subscription from db
                    CompanyLicenseDetailsDM? objDM = await _apiDbContext.CompanyLicenseDetails.FindAsync(objIdToUpdate);

                    if (objDM != null)
                    {
                        CompanyLicenseDetailsSM.Id = objIdToUpdate;
                        _mapper.Map(CompanyLicenseDetailsSM, objDM);

                        objDM.LastModifiedBy = _loginUserDetail.LoginId;
                        objDM.LastModifiedOnUTC = DateTime.UtcNow;

                        if (await _apiDbContext.SaveChangesAsync() > 0)
                        {
                            return _mapper.Map<CompanyLicenseDetailsSM>(objDM);
                        }
                        return null;
                    }
                    else
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"User subscription not found: {objIdToUpdate}", "User subscription to update not found, add as new instead.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not update user subscription, please try again", ex.InnerException);
            }
            return null;
        }

        #endregion Update

        #region Delete
        /// <summary>
        /// Deletes a user subscription by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>A DeleteResponseRoot indicating the result of the deletion operation.</returns>
        public async Task<DeleteResponseRoot> DeleteUserSubscriptionById(int id)
        {
            try
            {
                // Check if the product with the specified ID exists in the database
                var isPresent = await _apiDbContext.CompanyLicenseDetails.AnyAsync(x => x.Id == id);

                if (isPresent)
                {
                    // Create an instance of CompanyLicenseDetailsDM with the specified ID for deletion
                    var dmToDelete = new CompanyLicenseDetailsDM() { Id = id };

                    // Remove the user subscription from the database
                    _apiDbContext.CompanyLicenseDetails.Remove(dmToDelete);

                    // Save changes to the database
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        // If deletion is successful, return a success response
                        return new DeleteResponseRoot(true, "User subscription with Id " + id + " deleted successfully!");
                    }
                }

                // If no product was found with the specified ID, return a failure response
                return new DeleteResponseRoot(false, "No such user subscription found");
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, @$"{ex.Message}", @"Could not delete the user subscription, please try again", ex.InnerException);
            }
        }

        #endregion Delete

        #region My-License

        //public async Task<CompanyLicenseDetailsSM> GetActiveTrialCompanyLicenseDetailsByUserId(int currentUserId)
        //{
        //    DateTime startDateTime = DateTime.UtcNow;
        //    var validityInDays = _apiConfiguration.ValidityInDays;
        //    DateTime endDateTime = DateTime.UtcNow.AddDays(validityInDays);
        //    CompanyLicenseDetailsDM? userLicenseDetailDb = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.ClientUserId == currentUserId && x.ExpiryDateUTC.Value.Date <= endDateTime && x.Status == "active");
        //    if (userLicenseDetailDb != null)
        //    {
        //        var userLicenseDetailsSM = _mapper.Map<CompanyLicenseDetailsSM>(userLicenseDetailDb);
        //        return userLicenseDetailsSM;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public async Task<CompanyLicenseDetailsSM> GetActiveLicenseDetailsByCompanyCode(string companyCode)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            ClientCompanyDetailSM company = await this._clientCompanyDetailProcess.GetClientCompanyByCompanyCode(companyCode);
            if (company == null || company.Id <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"Company with code {companyCode} not found.", "Something went wrong!.Please refresh and login again.");
            CompanyLicenseDetailsDM? userLicenseDetailDb = null;
            if (_apiConfiguration.IsTestLicenseUsed == true)
            {
                var testPremiumResponse = await GetTestPremiumLicense();
                return testPremiumResponse;
            }
            userLicenseDetailDb = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.ClientCompanyDetailId == company.Id && (x.StartDateUTC.Value.Date <= currentDateTime.Date && x.ExpiryDateUTC.Value.Date >= currentDateTime.Date) && x.Status == "active");
            if (userLicenseDetailDb != null)
            {
                var userLicenseDetailsSM = _mapper.Map<CompanyLicenseDetailsSM>(userLicenseDetailDb);
                return userLicenseDetailsSM;
            }
            else
            {
                //check trial license
                if (company.IsTrialUsed && company.TrailLastDate.HasValue && company.TrailLastDate.Value.Date <= currentDateTime)
                    return await AddTrialLicenseDetails(companyCode);
            }
            return null;
        }

        public async Task<CompanyLicenseDetailsSM?> GetActiveLicenseByCompanyId(int companyID)
        {
            CompanyLicenseDetailsDM? userLicenseDetailDb = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.ClientCompanyDetailId == companyID && x.Status == "active");
            if (userLicenseDetailDb != null)
            {
                var userLicenseDetailsSM = _mapper.Map<CompanyLicenseDetailsSM>(userLicenseDetailDb);
                return userLicenseDetailsSM;
            }
            return null;
        }

        //public async Task<List<PermissionSM>> GetActiveCompanyLicenseDetailsPermissionByUserId(string companyCode, int userID, RoleTypeSM roleTypeSM)
        //{
        //    var company = await _clientCompanyDetailProcess.GetClientCompanyByCompanyCode(companyCode);
        //    var activeLicense = await GetActiveLicenseByCompanyId(company.Id);
        //    if (activeLicense != null)
        //    {
        //        var permissions = await _superAdminProcess.GetMineModulePermissions(company.Id, roleTypeSM, userID);
        //        return permissions;
        //    }
        //    return null;
        //}

        #endregion Mine-License

        #region Trial License Methods
        /// <summary>
        /// This method adds a trial license for the logged in user for a period of 7 days
        /// </summary>
        /// <param name="userID">loggedIn userID</param>
        /// <returns>CompanyLicenseDetailsSM</returns>
        public async Task<CompanyLicenseDetailsSM?> AddTrialLicenseDetails(string companyCode)
        {
            ClientCompanyDetailSM company = await this._clientCompanyDetailProcess.GetClientCompanyByCompanyCode(companyCode);
            if (company == null || company.Id <= 0)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"Company with code {companyCode} not found.", "Something went wrong!.Please refresh and login again.");

            var companyLicense = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.ClientCompanyDetailId == company.Id);

            if (companyLicense != null && companyLicense.ExpiryDateUTC <= DateTime.UtcNow)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_NoLog, $"Trial license already exists for userId {company.Id} and it is expired", $"Your trail period has expired");
            else if (companyLicense != null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_NoLog, $"Trial license already exists for userId {company.Id}", $"Trail license already exists");

            var userLicenseDetailDM = new CompanyLicenseDetailsDM();
            userLicenseDetailDM.CreatedBy = _loginUserDetail.LoginId;
            userLicenseDetailDM.CreatedOnUTC = DateTime.UtcNow;
            userLicenseDetailDM.ValidityInDays = _apiConfiguration.ValidityInDays;
            userLicenseDetailDM.LicenseTypeId = 1;
            //userLicenseDetailDM.c = "Coin Management";
            userLicenseDetailDM.SubscriptionPlanName = "Trial";
            userLicenseDetailDM.ExpiryDateUTC = DateTime.UtcNow.AddDays(7);
            userLicenseDetailDM.CancelAt = DateTime.UtcNow.AddDays(7);
            userLicenseDetailDM.StartDateUTC = DateTime.UtcNow;
            userLicenseDetailDM.IsCancelled = false;
            userLicenseDetailDM.IsSuspended = false;
            userLicenseDetailDM.CancelledOn = null;
            userLicenseDetailDM.ClientCompanyDetailId = company.Id;
            userLicenseDetailDM.StripeSubscriptionId = "sub_coinTrial";
            userLicenseDetailDM.ActualPaidPrice = 0;
            userLicenseDetailDM.Currency = "nil";
            userLicenseDetailDM.Status = "active";
            userLicenseDetailDM.DiscountInPercentage = 0;
            userLicenseDetailDM.CompanyInvoices = null;


            await _apiDbContext.CompanyLicenseDetails.AddAsync(userLicenseDetailDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
                return _mapper.Map<CompanyLicenseDetailsSM>(userLicenseDetailDM);
            return null;
        }
        /// <summary>
        /// Retrieves a test premium license with default values for testing purposes.  
        /// Throws an exception if the premium license type is not found in the database.
        /// </summary>
        /// <returns>
        /// A <see cref="CompanyLicenseDetailsSM"/> object containing details of the test premium license.
        /// </returns>
        /// <exception cref="SiffrumPayrollException">
        /// Thrown when no license type with the title "Premium" exists in the database.
        /// </exception>
        public async Task<CompanyLicenseDetailsSM?> GetTestPremiumLicense()
        {            
            var licenseDetails = await _apiDbContext.LicenseTypes.FirstOrDefaultAsync(x => x.Title == "Premium");
            if(licenseDetails == null)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"License with title {licenseDetails} not found.", "Something went wrong!.Please try again later.");
            }            
            var response = new CompanyLicenseDetailsSM();
            response.CreatedBy = _loginUserDetail.LoginId;
            response.CreatedOnUTC = DateTime.UtcNow;
            response.ValidityInDays = _apiConfiguration.ValidityInDays;
            response.LicenseTypeId = licenseDetails.Id;
            response.SubscriptionPlanName = licenseDetails.Title;
            response.ExpiryDateUTC = DateTime.UtcNow.AddDays(30);
            response.CancelAt = DateTime.UtcNow.AddDays(30);
            response.StartDateUTC = DateTime.UtcNow;
            response.IsCancelled = false;
            response.IsSuspended = false;
            response.CancelledOn = null;
            response.StripeSubscriptionId = "test_premium_subscription";
            response.ActualPaidPrice = 0;
            response.Currency = "nil";
            response.Status = "active";
            response.DiscountInPercentage = 0;
            response.CompanyInvoices = null;
            return response;
        }

        public async Task<CompanyLicenseDetailsSM?> UpdateTrialLicenseStatus(int userId)
        {
            var userLicenseDetails = await _apiDbContext.CompanyLicenseDetails.FirstOrDefaultAsync(x => x.ClientCompanyDetailId == userId && x.SubscriptionPlanName == "Trial");
            if (userLicenseDetails != null)
            {
                if (userLicenseDetails.ExpiryDateUTC <= DateTime.UtcNow)
                {
                    userLicenseDetails.IsCancelled = true;
                    userLicenseDetails.Status = "trial_period_ended";

                    _apiDbContext.CompanyLicenseDetails.Update(userLicenseDetails);
                    if (_apiDbContext.SaveChanges() > 0)
                        return _mapper.Map<CompanyLicenseDetailsSM>(userLicenseDetails);
                }
            }
            return null;
        }
        #endregion Trial License Methods


    }
}
