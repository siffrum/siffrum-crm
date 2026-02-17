using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("subscription_plans")]
    public class SubscriptionPlanDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("days")]
        public int Days { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("discounted_price")]
        public decimal? DiscountedPrice { get; set; }

        [Column("free_delivery_above")]
        public decimal FreeDeliveryAbove { get; set; } = 0.00m;

        [Column("ios_product_id")]
        [MaxLength(191)]
        public string? IosProductId { get; set; }

        [Column("status")]
        public short Status { get; set; } = 1; // 0=Inactive, 1=Active

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        public ICollection<UserSubscriptionDM> UserSubscriptions { get; set; }
    }
}
