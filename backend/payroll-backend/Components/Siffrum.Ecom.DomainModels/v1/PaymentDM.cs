using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("payments")]
    public class PaymentDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("txnid")]
        [MaxLength(191)]
        public string TxnId { get; set; } = string.Empty;

        [Required]
        [Column("payment_amount")]
        public decimal PaymentAmount { get; set; }

        [Required]
        [Column("payment_status")]
        [MaxLength(191)]
        public string PaymentStatus { get; set; } = string.Empty;

        [Required]
        [Column("itemid")]
        [MaxLength(191)]
        public string ItemId { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
