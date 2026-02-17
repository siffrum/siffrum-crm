using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("subscription_items")]
    public class SubscriptionItemDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("subscription_id")]
        public long SubscriptionId { get; set; }

        [Required]
        [Column("stripe_id")]
        [MaxLength(191)]
        public string StripeId { get; set; } = string.Empty;

        [Required]
        [Column("stripe_product")]
        [MaxLength(191)]
        public string StripeProduct { get; set; } = string.Empty;

        [Required]
        [Column("stripe_price")]
        [MaxLength(191)]
        public string StripePrice { get; set; } = string.Empty;

        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
