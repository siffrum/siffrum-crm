using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("payment_requests")]
    public class PaymentRequestDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("payment_type")]
        [MaxLength(191)]
        public string PaymentType { get; set; } = string.Empty;

        [Required]
        [Column("payment_address")]
        [MaxLength(191)]
        public string PaymentAddress { get; set; } = string.Empty;

        [Required]
        [Column("amount_requested")]
        public int AmountRequested { get; set; }

        [Required]
        [Column("remarks")]
        [MaxLength(191)]
        public string Remarks { get; set; } = string.Empty;

        [Column("status")]
        public short Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
