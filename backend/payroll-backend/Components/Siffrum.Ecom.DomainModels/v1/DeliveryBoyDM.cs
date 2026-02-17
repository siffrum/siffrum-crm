using Microsoft.EntityFrameworkCore;
using Siffrum.Ecom.DomainModels.Enums;
using Siffrum.Ecom.DomainModels.Foundation.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("delivery_boys")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class DeliveryBoyDM : SiffrumDomainModelBase<long>
    {      

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; }

        [Column("username")]
        [MaxLength(191)]
        public string? Username { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("mobile")]
        [MaxLength(191)]
        public string? Mobile { get; set; } = null!;

        [Column("order_note")]
        public string? OrderNote { get; set; }

        [Column("address")]
        public string? Address { get; set; } = null!;

        [Column("bonus_type")]
        public int BonusType { get; set; } = 0;

        [Column("bonus_percentage")]
        public double BonusPercentage { get; set; } = 0;

        [Column("bonus_min_amount")]
        public double BonusMinAmount { get; set; } = 0;

        [Column("bonus_max_amount")]
        public double BonusMaxAmount { get; set; } = 0;

        [Column("balance")]
        public double Balance { get; set; } = 0;

        [Column("driving_license")]
        public string? DrivingLicense { get; set; }

        [Column("national_identity_card")]
        public string? NationalIdentityCard { get; set; }

        [Column("dob")]
        public DateTime? DateOfBirth { get; set; }

        [Column("bank_account_number")]
        public string? BankAccountNumber { get; set; }

        [Column("bank_name")]
        public string? BankName { get; set; }

        [Column("account_name")]
        public string? AccountName { get; set; }

        [Column("ifsc_code")]
        public string? IfscCode { get; set; }

        [Column("other_payment_information")]
        public string? OtherPaymentInformation { get; set; }

        [Column("status")]
        public DeliveryBoyStatusDM Status { get; set; }

        [Column("login_status")]
        public LoginStatusDM LoginStatus { get; set; }

        [Column("is_available")]
        public int IsAvailable { get; set; } = 1;

        [Column("fcm_id")]
        [MaxLength(191)]
        public string? FcmId { get; set; }

        [Column("pincode_id")]
        public int? PincodeId { get; set; }

        [Column("cash_received")]
        public double CashReceived { get; set; } = 0;        

        [Column("password")]
        public string? Password { get; set; }

        [Column("remark")]
        public string? Remark { get; set; }        

        [Column("is_email_confirmed")]
        public bool IsEmailConfirmed { get; set; }
        [Column("is_mobile_confirmed")]
        public bool IsMobileConfirmed { get; set; }

        [ForeignKey(nameof(Admin))]
        [Column("admin_id")]
        public long? AdminId { get; set; }
        public virtual AdminDM? Admin { get; set; }

        [ForeignKey(nameof(City))]

        [Column("city_id")]
        public long? CityId { get; set; }
        public virtual CityDM? City { get; set; }
        [Column("role_type")]
        public RoleTypeDM RoleType { get; set; }
        
    }
}
