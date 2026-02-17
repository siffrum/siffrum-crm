using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("seller_wallet_transactions")]
    public class SellerWalletTransactionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("order_item_id")]
        public int? OrderItemId { get; set; }

        [Column("seller_id")]
        public int? SellerId { get; set; }

        [Column("type")]
        [MaxLength(191)]
        public string? Type { get; set; }

        [Column("amount")]
        public double Amount { get; set; } = 0;

        [Column("message")]
        public string? Message { get; set; }

        [Column("status")]
        public short? Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
