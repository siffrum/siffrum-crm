using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("delivery_boy_transactions")]
    public class DeliveryBoyTransactionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("delivery_boy_id")]
        public int? DeliveryBoyId { get; set; }

        [Column("type")]
        public string? Type { get; set; }

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

        /*// ===== Static Constants =====
        public static string StatusSuccess = "success";
        public static string StatusFailed = "failed";

        public static string PaymentTypeCod = "COD";*/
    }
}
