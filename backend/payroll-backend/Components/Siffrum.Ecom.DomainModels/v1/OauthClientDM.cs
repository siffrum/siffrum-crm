using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("oauth_clients")]
    public class OauthClientDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long? UserId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Column("secret")]
        [MaxLength(100)]
        public string? Secret { get; set; }

        [Column("provider")]
        [MaxLength(191)]
        public string? Provider { get; set; }

        [Required]
        [Column("redirect")]
        public string Redirect { get; set; } = string.Empty;

        [Column("personal_access_client")]
        public bool PersonalAccessClient { get; set; }

        [Column("password_client")]
        public bool PasswordClient { get; set; }

        [Column("revoked")]
        public bool Revoked { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
