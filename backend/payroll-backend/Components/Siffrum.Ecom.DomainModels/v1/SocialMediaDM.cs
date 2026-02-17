using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("social_media")]
    public class SocialMediaDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("icon")]
        public string Icon { get; set; } = string.Empty;

        [Required]
        [Column("link")]
        public string Link { get; set; } = string.Empty;
    }
}
