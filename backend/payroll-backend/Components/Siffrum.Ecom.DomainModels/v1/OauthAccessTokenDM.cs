using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("oauth_access_tokens")]
    public class OauthAccessTokenDM
    {
        [Key]
        [Column("id")]
        [MaxLength(100)]
        public string Id { get; set; } = string.Empty;

        [Column("user_id")]
        public long? UserId { get; set; }

        [Required]
        [Column("client_id")]
        public long ClientId { get; set; }

        [Column("name")]
        [MaxLength(191)]
        public string? Name { get; set; }

        [Column("scopes")]
        public string? Scopes { get; set; }

        [Column("revoked")]
        public bool Revoked { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}
