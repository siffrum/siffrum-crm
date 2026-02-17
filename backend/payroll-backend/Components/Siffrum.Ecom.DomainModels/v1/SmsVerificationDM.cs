using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("sms_verifications")]
    public class SmsVerificationDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("phone")]
        [MaxLength(191)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column("otp")]
        [MaxLength(191)]
        public string Otp { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = "pending";

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
