using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    [Index(nameof(CompanyCode), IsUnique = true)]
    [Index(nameof(CompanyContactNumber), IsUnique = true)]
    [Index(nameof(CompanyContactEmail), IsUnique = true)]
    public class ClientCompanyDetailDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        public ClientCompanyDetailDM()
        {
        }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string CompanyContactEmail { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]
        public string CompanyContactNumber { get; set; }

        [Url]
        public string CompanyWebsite { get; set; }

        public string CompanyLogoPath { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CompanyDateOfEstablishment { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsTrialUsed { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TrailLastDate { get; set; }


        //[ForeignKey(nameof(ClientCompanyAddress))]
        //public int? ClientCompanyAddressId { get; set; }
        //public virtual ClientCompanyAddressDM? ClientCompanyAddress { get; set; }


        public virtual HashSet<ClientUserDM> ClientEmployeeUsers { get; set; }


    }
}
