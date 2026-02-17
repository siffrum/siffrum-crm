using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("wallet_transactions")]
    public class WalletTransactionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("order_item_id")]
        public int? OrderItemId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("amount")]
        public double Amount { get; set; }

        [Column("txn_id")]
        [MaxLength(191)]
        public string? TxnId { get; set; }

        [Column("payment_type")]
        [MaxLength(191)]
        public string? PaymentType { get; set; }

        [Required]
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column("message")]
        [MaxLength(191)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        /*public static readonly string StatusSuccess = "success";
        public static readonly string StatusFailed = "failed";

        public static readonly string PaymentTypeCod = "COD";
        public static readonly string PaymentTypeStripe = "Stripe";
        public static readonly string PaymentTypeRazorpay = "Razorpay";
        public static readonly string PaymentTypePaystack = "Paystack";
        public static readonly string PaymentTypePaytm = "Paytm";
        public static readonly string PaymentTypePaypal = "Paypal";*/

        /*// Laravel accessor: getAmountAttribute()
        [NotMapped]
        public double AmountAsFloat => Convert.ToDouble(Amount);*/
    }
}
