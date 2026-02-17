namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientCompanyDetailSM : Base.SiffrumPayrollServiceModelBase<int>
    {
        public ClientCompanyDetailSM()
        {
        }

        public string CompanyCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompanyContactEmail { get; set; }
        //public string ContactEmail { get; set; }
        public string CompanyMobileNumber { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyLogoPath { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsTrialUsed { get; set; }
        public DateTime? TrailLastDate { get; set; }

        //[ConvertFilePathToUri(SourcePropertyName = nameof(CompanyLogoPath))]
        public DateTime CompanyDateOfEstablishment { get; set; }
        //public int ClientCompanyAddressId { get; set; }
    }
}
