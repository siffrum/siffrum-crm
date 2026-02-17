namespace Siffrum.Web.Payroll.Config
{
    public class APIConfiguration : APIConfigRoot
    {
        public string ApiDbConnectionString { get; set; }
        public string JwtTokenSigningKey { get; set; }
        public double DefaultTokenValidityDays { get; set; }
        public string JwtIssuerName { get; set; }
        public string AuthTokenEncryptionKey { get; set; }
        public string AuthTokenDecryptionKey { get; set; }
        public int Time { get; set; }
        public int ValidityInDays { get; set; }
        public bool IsTestLicenseUsed { get; set; }

        #region Stripe Settings
        public StripeSettings StripeSettings { get; set; }
        #endregion
    }
}
