using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("seller_transactions")]
    public class SellerTransactionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("seller_id")]
        public int? SellerId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("order_item_id")]
        public int? OrderItemId { get; set; }

        [Column("type")]
        public string? Type { get; set; }

        [Column("txn_id")]
        public string? TxnId { get; set; }

        [Column("amount")]
        public double Amount { get; set; } = 0;

        [Column("status")]
        public string? Status { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
