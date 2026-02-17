using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("oauth_auth_codes")]
    public class OauthAuthCodeDM
    {
        [Key]
        [Column("id")]
        [MaxLength(100)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("client_id")]
        public long ClientId { get; set; }

        [Column("scopes")]
        public string? Scopes { get; set; }

        [Column("revoked")]
        public bool Revoked { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}
