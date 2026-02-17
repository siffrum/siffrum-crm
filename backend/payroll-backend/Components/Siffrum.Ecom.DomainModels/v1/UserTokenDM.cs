using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("user_tokens")]
    public class UserTokenDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("fcm_token")]
        [MaxLength(255)]
        public string FcmToken { get; set; } = string.Empty;

        [Required]
        [Column("platform")]
        [MaxLength(191)]
        public string Platform { get; set; } = "web";
    }
}
