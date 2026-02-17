namespace Siffrum.Web.Payroll.ServiceModels.v1.License
{
    public class CheckoutSessionResponseSM
    {
        public string SessionId { get; set; }
        public string PublicKey { get; set; }
        public bool IsNewSubscription { get; set; }
    }
}
