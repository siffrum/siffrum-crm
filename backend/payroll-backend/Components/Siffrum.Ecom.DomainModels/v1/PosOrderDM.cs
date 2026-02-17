using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("pos_orders")]
    public class PosOrderDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("pos_user_id")]
        public int? PosUserId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [Required]
        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; } = 0.00m;

        [Column("discount_percentage")]
        public decimal DiscountPercentage { get; set; } = 0.00m;

        [Required]
        [Column("payment_method")]
        [MaxLength(191)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
