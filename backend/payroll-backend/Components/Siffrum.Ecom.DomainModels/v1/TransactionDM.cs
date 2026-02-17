using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("transactions")]
    public class TransactionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("order_id")]
        [MaxLength(191)]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("txn_id")]
        [MaxLength(191)]
        public string TxnId { get; set; } = string.Empty;

        [Column("payu_txn_id")]
        [MaxLength(191)]
        public string? PayuTxnId { get; set; }

        [Required]
        [Column("amount")]
        public double Amount { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Column("message")]
        [MaxLength(191)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }


        /*public const string StatusSuccess = "success";
        public const string StatusFailed = "failed";

        
        public const string PaymentTypeCod = "COD";
        public const string PaymentTypeStripe = "Stripe";
        public const string PaymentTypeRazorpay = "Razorpay";
        public const string PaymentTypePaystack = "Paystack";
        public const string PaymentTypePaytm = "Paytm";
        public const string PaymentTypePaypal = "Paypal";
        public const string PaymentTypeWallet = "Wallet";
        public const string PaymentTypeMidtrans = "Midtrans";
        public const string PaymentTypePhonepe = "Phonepe";
        public const string PaymentTypeCashfree = "Cashfree";
        public const string PaymentTypePaytabs = "Paytabs";
        public const string PaymentTypeInAppPurchase = "InAppPurchase";

        [NotMapped]
        public float AmountAsFloat => (float)Amount;*/
    }
}
