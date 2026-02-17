using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("personal_access_tokens")]
    public class PersonalAccessTokenDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("tokenable_type")]
        [MaxLength(191)]
        public string TokenableType { get; set; } = string.Empty;

        [Required]
        [Column("tokenable_id")]
        public long TokenableId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("token")]
        [MaxLength(64)]
        public string Token { get; set; } = string.Empty;

        [Column("abilities")]
        public string? Abilities { get; set; }

        [Column("last_used_at")]
        public DateTime? LastUsedAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
