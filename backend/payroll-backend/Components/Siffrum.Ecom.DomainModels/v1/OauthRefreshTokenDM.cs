using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("oauth_refresh_tokens")]
    public class OauthRefreshTokenDM
    {
        [Key]
        [Column("id")]
        [MaxLength(100)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Column("access_token_id")]
        [MaxLength(100)]
        public string AccessTokenId { get; set; } = string.Empty;

        [Column("revoked")]
        public bool Revoked { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}
