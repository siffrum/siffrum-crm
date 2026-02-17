using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("password_resets")]
    public class PasswordResetDM
    {
        [Key]
        [Column("email")]
        [MaxLength(191)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("token")]
        [MaxLength(191)]
        public string Token { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
