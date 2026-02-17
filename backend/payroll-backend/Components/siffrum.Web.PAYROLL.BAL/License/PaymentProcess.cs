using Siffrum.Web.Payroll.BAL.AppUsers;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.v1.License;
using Stripe;
using Stripe.Checkout;

namespace Siffrum.Web.Payroll.BAL.License
{
    public class PaymentProcess : SiffrumPayrollBalBase
    {
        #region Properties
        private readonly ILoginUserDetail _loginUserDetail;
        private readonly ClientUserProcess _clientUserProcess;
        private readonly CompanyLicenseDetailsProcess _companyLicenseDetailsProcess;
        private readonly CompanyInvoiceProcess _companyInvoiceProcess;
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        private readonly LicenseTypeProcess _licenseTypeProcess;
        private readonly APIConfiguration _apiConfiguration;

        #endregion Properties

        #region Constructor
        public PaymentProcess(IMapper mapper, ApiDbContext apiDbContext, ILoginUserDetail loginUserDetail,
            ClientUserProcess clientUserProcess, CompanyLicenseDetailsProcess companyLicenseDetailsProcess,APIConfiguration apiConfiguration,
            CompanyInvoiceProcess companyInvoiceProcess, LicenseTypeProcess licenseTypeProcess, ClientCompanyDetailProcess clientCompanyDetailProcess) : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _clientUserProcess = clientUserProcess;
            _companyLicenseDetailsProcess = companyLicenseDetailsProcess;
            _companyInvoiceProcess = companyInvoiceProcess;
            _licenseTypeProcess = licenseTypeProcess;
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            _apiConfiguration = apiConfiguration;
        }
        #endregion Constructor

        #region Checkout Session
        public async Task<CheckoutSessionResponseSM> CheckoutSession(CheckoutSessionRequestSM reqData, int customerId, string companyCode)
        {
            
            var stripeKey = _apiConfiguration.StripeSettings.PublicKey;
            //StripeConfiguration.ApiKey = "sk_test_51O5j1LSJgnOZTfXcjkjVT5M61iXXIPCzIOOIArI9urf2cptGRFZGtwUhhZaDnSRhzcvZ3u5sk66hrNHDCUs09gTj00HvRkn5B2";
            //StripeConfiguration.ApiKey = "sk_test_51OYnJfSDaWILKYOeY73bTPgKuWIlToKSj19q7q9PdeSj3sapJ3YJLjhKNPU6sxPtP30ENbXLuASYfdgAPjpyZa3U00scR6SPLI";
            StripeConfiguration.ApiKey = stripeKey;

            //reqdata.productid
            var options1 = new PlanListOptions { Product = reqData.ProductId };
            var service1 = new PlanService();
            var plans = service1.List(options1);
            // check permission if user is allowed to see license details
            var user = await this._clientUserProcess.GetClientUserById(customerId);//check this method
            if (user == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"User {customerId} not found to create checkout session.", "Error occurred in payment, Please login again.");
            if (!user.IsPaymentAdmin)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"User {customerId} not found to create checkout session.", "Error occurred in payment, User not authorized.");
            ClientCompanyDetailSM company = await this._clientCompanyDetailProcess.GetClientCompanyDetailByCompanyCode(companyCode);//check this method
            if (company == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"company Code {companyCode} not found to create checkout session.", "Error occurred in payment, Please login again.");
            //if (!user.IsEmailConfirmed)
            //    throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_NoLog, $"company Code {companyCode} email not confirmed.", "Please verify your email.");
            //if (!company.IsPhoneNumberConfirmed)
            //    throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_NoLog, $"company Code {companyCode} phone number not confirmed.", "Please verify your contact number.");

            var paymentMethodTypes = new List<string>();
            switch (reqData.PaymentMode)
            {
                case ServiceModels.Enums.PaymentModeSM.DebitCard:
                case ServiceModels.Enums.PaymentModeSM.CreditCard:
                    paymentMethodTypes.Add("card");
                    break;
                case ServiceModels.Enums.PaymentModeSM.Wallet:
                    paymentMethodTypes.Add("wallet");
                    break;
                default:
                    paymentMethodTypes.Add("card");
                    break;
            }
            //var options = new Stripe.Checkout.SessionCreateOptions
            //{
            //    PaymentMethodTypes = new List<string> { "card" },
            //    LineItems = new List<SessionLineItemOptions>
            //{
            //    new SessionLineItemOptions
            //    {
            //        //reqdata.priceid
            //        Price = "price_1OpWyFSJgnOZTfXcFplfDZ8E", 
            //        Quantity = 1,    
            //    }
            //},
            //    Mode = "subscription", // Specify the mode as "subscription" for creating a subscription session
            //    SuccessUrl = "https://example.com/success",
            //    CancelUrl = "https://example.com/cancel",
            //};

            var options = new SessionCreateOptions
            {
                SuccessUrl = reqData.SuccessUrl,
                CancelUrl = reqData.FailureUrl,
                PaymentMethodTypes = paymentMethodTypes,
                Mode = "subscription",
                CustomerEmail = company.CompanyContactEmail,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                    //    Price =reqData.PriceId,
                        Price = plans.Data[0].Id,
                        Quantity = 1,
                    },
                },
                //adding extra info  - product id
                Metadata = new Dictionary<string, string>
                {
                    { "productId", reqData.ProductId.ToString() },
                }
            };

            // Redirect the user to the Stripe Checkout page

            try
            {
                var service = new Stripe.Checkout.SessionService();
                var session = service.Create(options);

                return new CheckoutSessionResponseSM() { SessionId = session.Id, };
            }
            catch (StripeException e)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, e.Message, "Some error occurred, please try again.", e);
            }

        }
        #endregion Checkout Session

        #region Customer Portal
        public async Task<CustomerPortalResponseSM> GetCustomerPortalUrl(string returnUrl, int customerId, string companyCode)
        {
            var user = await this._clientUserProcess.GetClientUserById(customerId);//check this method
            if (user == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"User {customerId} not found to create checkout session.", "Error occurred in payment, Please login again.");
            /*if (!user.IsPaymentAdmin)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"User {customerId} not found to create checkout session.", "Error occurred in payment, User not authorized.");*/
            ClientCompanyDetailSM company = await this._clientCompanyDetailProcess.GetClientCompanyDetailByCompanyCode(companyCode);//check this method
            if (company == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"company Code {companyCode} not found to create checkout session.", "Error occurred in payment, Please login again.");

            var userSubscription = await _companyLicenseDetailsProcess.GetUserSubscriptionByCompanyId(company.Id);
            if (userSubscription == null)
                throw new SiffrumPayrollException(ApiErrorTypeSM.NoRecord_Log, $"userSubscription not found company Code {companyCode}.", "Error occurred in payment, Please login again.");

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = userSubscription?.StripeCustomerId,
                ReturnUrl = returnUrl,
            };
            var resp = new CustomerPortalResponseSM();
            try
            {
                var service = new Stripe.BillingPortal.SessionService();
                var session = await service.CreateAsync(options);
                resp.Url = session.Url;
                return resp;
            }
            catch (StripeException e)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, e.Message, "Some error occurred, please try again", e);
            }

        }

        #endregion Customer Portal

        #region Stripe Webhook
        public async Task<bool> RegisterStripeWebhook(string stripeWebhookJson, string stripeSignatureHeader, string webHookSecret)
        {
            try
            {
                //var stripeEvent = EventUtility.ConstructEvent(stripeWebhookJson, stripeSignatureHeader, webHookSecret);
                var stripeEvent = EventUtility.ConstructEvent(
                    stripeWebhookJson,
                    stripeSignatureHeader,
                    webHookSecret,
                    throwOnApiVersionMismatch: false
                    );

                if (stripeEvent.Type == Events.CustomerCreated)
                {
                    var customer = stripeEvent.Data.Object as Customer;
                    await AddUpdateStripeCustomer(customer);
                }
                if (stripeEvent.Type == Events.CustomerUpdated)
                {
                    var customer = stripeEvent.Data.Object as Customer;
                    await AddUpdateStripeCustomer(customer);
                }
                // Handle the event
                if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    await AddUpdateStripeSubscription(subscription);
                }
                if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                    await AddUpdateStripeSubscription(subscription);
                }
                if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                    await AddUpdateStripeSubscription(subscription);
                }
                if (stripeEvent.Type == Events.SubscriptionScheduleCanceled)
                {
                    var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                    await AddUpdateStripeSubscription(subscription);
                }
                if (stripeEvent.Type == Events.SubscriptionScheduleUpdated)
                {
                    var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                    await AddUpdateStripeSubscription(subscription);
                }
                // Handle the payment retry event
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Implement your logic to handle the payment retry event
                    // For example, you can send notifications or update your database
                    //await UpdateInvoiceDetails(paymentIntent);
                }
                //else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                //{
                //    var session = stripeEvent.Data.Object as Session;
                //    // Update Subsription
                //    //await UpdateSubscription(session);
                //}
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    //Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return true;
            }
            catch (StripeException e)
            {
                throw e;
                //log message
            }
        }
        #endregion Stripe Webhook

        #region Private Methods

        private async Task<CompanyLicenseDetailsSM?> AddUpdateStripeCustomer(Customer? customer)
        {
            try
            {
                // recheck this function
                //var user = await _clientUserProcess.GetClientUserByEmail(customer.Email);
                var company = await _clientCompanyDetailProcess.GetClientCompanyByEmail(customer.Email);
                var userSubscriptionSM = new CompanyLicenseDetailsSM()
                {
                    StripeCustomerId = customer.Id,
                };

                //checking if this user subscription already exists in the db for this customer id
                var userSubscriptionExistsInDb = await _companyLicenseDetailsProcess.GetUserSubscriptionByStripeCustomerId(customer.Id);

                if (userSubscriptionExistsInDb != null && company != null)
                    return await _companyLicenseDetailsProcess.UpdateUserSubscription(userSubscriptionExistsInDb.Id, userSubscriptionSM);
                /* else
                     return await _userSubscriptionProcess.AddUserSubscription(userSubscriptionSM);*/
                return null;
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.ModelError_Log, $"{ex.Message}", $"Add or Update failed in user subscription table", ex);
            }
        }

        private async Task<bool?> AddUpdateStripeSubscription(Subscription? subscription)
        {
            try
            {
                //ClientUserSM? user = null;
                ClientCompanyDetailSM? company = null;
                CompanyLicenseDetailsSM? userSubscriptionAddUpdateResponse = null;
                var featureGroup = await _licenseTypeProcess.GetSingleFeatureGroupByStripePriceId(subscription.Items.Data[0].Price.Id);
                var customerService = new CustomerService();
                var customer = customerService.Get(subscription.CustomerId);
                if (customer != null)
                    company = await _clientCompanyDetailProcess.GetClientCompanyByEmail(customer.Email);
                //var priceId = subscription.Items.Data[0].Price.Id;
                //TODO: a user can purchase different products, so we have to allow that as well based on some validation
                //checking if the user is again subscribing to the same product
                //if (priceId != userSubscription.StripePriceId)
                {
                    if (company != null)
                    {
                        CompanyLicenseDetailsSM? userSubscription = await _companyLicenseDetailsProcess.GetUserSubscriptionByCompanyId(company.Id);
                        if (userSubscription != null)
                            userSubscriptionAddUpdateResponse = await _companyLicenseDetailsProcess.UpdateUserSubscription(userSubscription.Id, ConvertStripeSubscriptionToUserSubscriptionSM(subscription, company.Id, featureGroup));
                        else
                            userSubscriptionAddUpdateResponse = await _companyLicenseDetailsProcess.AddUserSubscription(ConvertStripeSubscriptionToUserSubscriptionSM(subscription, company.Id, featureGroup));

                        Invoice invoice = new Invoice();
                        if (subscription.LatestInvoice == null)
                        {
                            var invoiceService = new InvoiceService();
                            invoice = invoiceService.Get(subscription.LatestInvoiceId);
                        }

                        var invoiceDb = await _companyInvoiceProcess.GetSingleInvoiceByStripeInvoiceId(subscription.LatestInvoiceId);
                        if (invoiceDb != null)
                            await _companyInvoiceProcess.UpdateUserInvoice(invoiceDb.Id, ConvertToInvoiceSM(subscription, invoice, company.Id));
                        else
                            await _companyInvoiceProcess.AddUserInvoice(ConvertToInvoiceSM(subscription, invoice, company.Id));
                    }
                    return userSubscriptionAddUpdateResponse != null && userSubscriptionAddUpdateResponse.Id > 0;
                }
            }
            catch (Exception ex)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.ModelError_Log, $"{ex.Message}", $"Add or Update failed in user subscription table", ex);
            }
        }

        #endregion Private Methods

        #region Convert Stripe Subscription to User SubscriptionSM

        /// <summary>
        /// Converts a Stripe subscription object to a UserSubscriptionSM object.
        /// </summary>
        /// <param name="subscription">The Stripe subscription object.</param>
        /// <param name="user">The associated client user.</param>
        /// <returns>A UserSubscriptionSM object containing the relevant subscription details.</returns>
        private CompanyLicenseDetailsSM? ConvertStripeSubscriptionToUserSubscriptionSM(Subscription? subscription, int companyId, LicenseTypeSM licenseType)
        {
            try
            {
                if (subscription != null && subscription.Items != null && subscription.Items.Data.Count > 0)
                {
                    // Retrieve the price details, which includes the associated product
                    var priceItem = subscription.Items.Data[0].Price;
                    if (priceItem != null)
                    {
                        var priceService = new PriceService();
                        var price = priceService.Get(priceItem.Id);

                        if (price != null)
                        {
                            // Create and return the UserSubscriptionSM object
                            var userSubscriptionSM = new CompanyLicenseDetailsSM
                            {
                                ClientCompanyId = companyId,
                                StripeCustomerId = subscription.CustomerId,
                                SubscriptionPlanName = price.Nickname,
                                StripeProductId = priceItem.ProductId,
                                StartDateUTC = subscription.CurrentPeriodStart,
                                ExpiryDateUTC = subscription.CurrentPeriodEnd,
                                IsSuspended = false,
                                ValidityInDays = (int)(subscription.CurrentPeriodEnd - subscription.CurrentPeriodStart).TotalDays,
                                ActualPaidPrice = priceItem.UnitAmountDecimal.Value / 100,
                                Currency = subscription.Currency,
                                StripeSubscriptionId = subscription.Id,
                                Status = subscription.Status,
                                StripePriceId = price.Id,
                                LicenseTypeId = licenseType.Id, //TODO : get it from license type table
                            };
                            if (subscription.CancelAtPeriodEnd || subscription.CanceledAt.HasValue)
                            {
                                userSubscriptionSM.IsCancelled = true;
                                userSubscriptionSM.CancelAt = subscription.CancelAt?.ToUniversalTime();
                                userSubscriptionSM.CancelledOn = subscription.CanceledAt?.ToUniversalTime();
                            }
                            else
                            {
                                userSubscriptionSM.IsCancelled = false;
                                userSubscriptionSM.CancelAt = null;
                                userSubscriptionSM.CancelledOn = null;
                            }
                            return userSubscriptionSM;
                        }
                    }
                }

                // Handle the case where subscription or its related objects are null
                return null;
            }
            catch (StripeException ex)
            {
                throw ex;
            }
        }

        //#region Get Product Name On StripeProductId
        //public string GetProductName(string productId)
        //{
        //    try
        //    {
        //        var productService = new ProductService();
        //        var product = productService.Get(productId);

        //        if (product != null)
        //        {
        //            return product.Name;
        //        }
        //        else
        //        {
        //            throw new Exception("Product not found");
        //        }
        //    }
        //    catch
        //    {
        //        return "Unknown Product";
        //    }
        //}
        //#endregion Get Product Name On StripeProductId

        #endregion Convert Stripe Subscription to User SubscriptionSM

        #region Convert To InvoiceSM
        private CompanyInvoiceSM ConvertToInvoiceSM(Subscription subscription, Invoice invoice, int companyId)
        {
            var userInvoiceSM = new CompanyInvoiceSM
            {
                StripeCustomerId = subscription.CustomerId,
                Currency = invoice.Currency,
                StartDateUTC = subscription.CurrentPeriodStart.ToUniversalTime(),
                ActualPaidPrice = invoice.AmountPaid,
                AmountDue = invoice.AmountDue,
                AmountRemaining = invoice.AmountRemaining,
                ExpiryDateUTC = subscription.CurrentPeriodEnd.ToUniversalTime(),
                DiscountInPercentage = 0,//can be calculated;
                StripeInvoiceId = invoice.Id,
                CreatedBy = _loginUserDetail.LoginId,
                CreatedOnUTC = DateTime.UtcNow,
                CompanyLicenseDetailsId = companyId

            };
            return userInvoiceSM;
        }
        #endregion Convert To InvoiceSM

    }
}
