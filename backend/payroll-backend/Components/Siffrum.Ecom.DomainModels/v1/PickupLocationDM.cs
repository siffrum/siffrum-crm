using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("pickup_locations")]
    public class PickupLocationDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("seller_id")]
        public int SellerId { get; set; }

        [Required]
        [Column("pickup_location")]
        [MaxLength(191)]
        public string PickupLocationName { get; set; } = string.Empty;

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [MaxLength(191)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("phone")]
        [MaxLength(191)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("address_2")]
        public string Address2 { get; set; } = string.Empty;

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

        [Required]
        [Column("pin_code")]
        [MaxLength(191)]
        public string PinCode { get; set; } = string.Empty;

        [Required]
        [Column("latitude")]
        [MaxLength(191)]
        public string Latitude { get; set; } = string.Empty;

        [Required]
        [Column("longitude")]
        [MaxLength(191)]
        public string Longitude { get; set; } = string.Empty;

        [Column("verified")]
        public short Verified { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
