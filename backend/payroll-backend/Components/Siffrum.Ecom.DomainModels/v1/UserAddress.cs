using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("user_addresses")]
    public class UserAddress
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("mobile")]
        [MaxLength(191)]
        public string Mobile { get; set; } = string.Empty;

        [Column("alternate_mobile")]
        [MaxLength(191)]
        public string? AlternateMobile { get; set; }

        [Required]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("landmark")]
        public string Landmark { get; set; } = string.Empty;

        [Required]
        [Column("area")]
        [MaxLength(191)]
        public string Area { get; set; } = string.Empty;

        [Required]
        [Column("pincode")]
        [MaxLength(191)]
        public string Pincode { get; set; } = string.Empty;

        [Required]
        [Column("city_id")]
        [MaxLength(191)]
        public string CityId { get; set; } = string.Empty;

        [Required]
        [Column("city")]
        [MaxLength(191)]
        public string City { get; set; } = string.Empty;

        [Required]
        [Column("state")]
        [MaxLength(191)]
        public string State { get; set; } = string.Empty;

        [Required]
        [Column("country")]
        [MaxLength(191)]
        public string Country { get; set; } = string.Empty;

        [Column("is_default")]
        public short IsDefault { get; set; } = 0;

        [Column("latitude")]
        [MaxLength(191)]
        public string? Latitude { get; set; }

        [Column("longitude")]
        [MaxLength(191)]
        public string? Longitude { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }


        public UserDM User { get; set; }
    }
}
