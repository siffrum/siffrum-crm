using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siffrum.Web.Payroll.API.Security;
using Siffrum.Web.Payroll.BAL.License;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.ServiceModels.v1.License;

namespace Siffrum.Web.Payroll.API.Controllers.License
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ApiControllerRoot
    {
        #region Properties
        private readonly PaymentProcess _paymentProcess;
        private readonly APIConfiguration _apiConfiguration;

        #endregion Properties

        #region Constructor
        public PaymentController(PaymentProcess paymentProcess, APIConfiguration apiConfiguration)
        {
            _paymentProcess = paymentProcess;
            _apiConfiguration = apiConfiguration;
        }
        #endregion Constructor

        #region Stripe Methods

        #region Create Checkout Session
        /// <summary>
        /// Create Checkout Session ID for priceId
        /// </summary>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        [HttpPost("mine/checkout")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin, ClientEmployee,SystemAdmin,SuperAdmin")]
        public async Task<ActionResult<ApiResponse<CheckoutSessionResponseSM>>> CreateCheckoutSession([FromBody] ApiRequest<CheckoutSessionRequestSM> apiRequest)
        {
            var innerReq = apiRequest?.ReqData;
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }
            CheckoutSessionResponseSM resp = await _paymentProcess.CheckoutSession(apiRequest.ReqData, currentUserRecordId, companyCode);
            if (resp != null)
            {
                //resp.PublicKey = _apiConfiguration.StripeSettings.PublicKey;
                resp.PublicKey = _apiConfiguration.StripeSettings.PrivateKey;
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Create Checkout Session

        #region Customer Portal
        [HttpPost("mine/StripeCustomerPortal")]
        [Authorize(AuthenticationSchemes = APIBearerTokenAuthHandler.DefaultSchema, Roles = "ClientAdmin")]
        public async Task<ActionResult<ApiResponse<CustomerPortalResponseSM>>> CustomerPortal([FromBody] ApiRequest<CustomerPortalRequestSM> apiRequest)
        {
            var innerReq = apiRequest?.ReqData;
            int currentUserRecordId = User.GetUserRecordIdFromCurrentUserClaims();
            string companyCode = User.GetCompanyCodeFromCurrentUserClaims();
            if (currentUserRecordId <= 0)
            {
                return NotFound(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_IdNotInClaims));
            }
            if (innerReq == null)
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_ReqDataNotFormed, ApiErrorTypeSM.InvalidInputData_NoLog));
            }
            //check permission if user is allowed to see license details
            var resp = await _paymentProcess.GetCustomerPortalUrl(innerReq.ReturnUrl, currentUserRecordId, companyCode);
            if (resp != null)
            {
                //resp.PublicKey = _apiConfiguration.StripeSettings.PublicKey;
                return Ok(ModelConverter.FormNewSuccessResponse(resp));
            }
            else
            {
                return BadRequest(ModelConverter.FormNewErrorResponse(DomainConstantsRoot.DisplayMessagesRoot.Display_PassedDataNotSaved, ApiErrorTypeSM.NoRecord_NoLog));
            }
        }
        #endregion Customer Portal

        #region Stripe Webhooks

        /// <summary>
        /// Listen to stripe webhook events
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("stripewebhook")]
        public async Task<ActionResult<ApiResponse<CheckoutSessionResponseSM>>> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var resp = await this._paymentProcess.RegisterStripeWebhook(json, Request.Headers["Stripe-Signature"], _apiConfiguration.StripeSettings.WHSecret);
            if (resp != null)
            {
                if (resp)
                    return Ok();
                else
                    //log the object in payments logs and that the customer has paid but is not created.
                    return BadRequest();
            }
            else
            {
                //log the object in payments logs and that the customer has paid but is not created.
                return BadRequest();
            }
        }
        #endregion Stripe Webhooks

        #endregion Stripe Methods
    }
}
