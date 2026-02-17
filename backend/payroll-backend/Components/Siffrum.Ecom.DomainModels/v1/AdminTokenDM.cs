using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("admin_tokens")]
    public class AdminTokenDM
    {
        [Key]
        [Column("id")]
        public ulong Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = null!;

        [Required]
        [Column("fcm_token")]
        [MaxLength(255)]
        public string FcmToken { get; set; } = null!;

        [Required]
        [Column("platform")]
        [MaxLength(191)]
        public string Platform { get; set; } = "web";
    }
}
