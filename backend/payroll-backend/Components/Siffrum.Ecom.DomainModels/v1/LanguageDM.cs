using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("languages")]
    public class LanguageDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("supported_language_id")]
        public int SupportedLanguageId { get; set; } = 0;

        [Required]
        [Column("system_type")]
        public int SystemType { get; set; }
        // 1 => Customer App
        // 2 => Seller & Delivery App
        // 3 => Website
        // 4 => Admin Panel

        [Column("json_data", TypeName = "json")]
        public string? JsonData { get; set; }

        [Column("display_name")]
        [MaxLength(191)]
        public string? DisplayName { get; set; }

        [Column("is_default")]
        public int IsDefault { get; set; } = 0; // 0 = No, 1 = Yes

        [Column("status")]
        public int Status { get; set; } = 1; // 0 = Inactive, 1 = Active

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Static constants
        /*public const int SystemTypeCustomerApp = 1;
        public const int SystemTypeSellerAndDeliveryBoyApp = 2;
        public const int SystemTypeWebsite = 3;
        public const int SystemTypeAdminPanel = 4;
*/
        /*// Laravel: $appends
        [NotMapped]
        public string? SystemTypeName
        {
            get
            {
                var systemTypes = GetSystemTypes();
                var match = systemTypes.Find(x => x.Id == SystemType);
                return match?.Name;
            }
        }

        // Helper equivalent
        public static List<SystemTypeItem> GetSystemTypes()
        {
            return new List<SystemTypeItem>
            {
                new() { Id = 1, Name = "Customer App" },
                new() { Id = 2, Name = "Seller & Delivery Boy App" },
                new() { Id = 3, Name = "Website" },
                new() { Id = 4, Name = "Admin Panel" }
            };
        }*/
    }

    /*public class SystemTypeItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }*/
}
