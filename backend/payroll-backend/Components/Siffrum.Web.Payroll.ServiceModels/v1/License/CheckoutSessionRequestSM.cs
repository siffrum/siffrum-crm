using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.License
{
    public class CheckoutSessionRequestSM
    {
        public string ProductId { get; set; }
        public string PriceId { get; set; }
        public string SuccessUrl { get; set; }
        public string FailureUrl { get; set; }
        public PaymentModeSM PaymentMode { get; set; }
    }
}
