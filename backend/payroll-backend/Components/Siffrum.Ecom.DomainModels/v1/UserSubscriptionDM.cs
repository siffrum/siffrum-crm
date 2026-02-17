using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("user_subscriptions")]
    public class UserSubscriptionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("plan_id")]
        public long? PlanId { get; set; }

        [Required]
        [Column("plan_name")]
        [MaxLength(191)]
        public string PlanName { get; set; } = string.Empty;

        [Required]
        [Column("price_paid")]
        public decimal PricePaid { get; set; }

        [Column("discounted_price")]
        public decimal? DiscountedPrice { get; set; }

        [Column("free_delivery_above")]
        public decimal FreeDeliveryAbove { get; set; } = 0.00m;

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = "active"; // enum('active','expired')

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Relationships
        public UserDM? User { get; set; }
        public SubscriptionPlanDM? Plan { get; set; }
    }
}
