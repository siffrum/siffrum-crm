using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("panel_notifications")]
    public class PanelNotificationDM
    {
        [Key]
        [Column("id")]
        [MaxLength(36)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("notifiable_type")]
        [MaxLength(191)]
        public string NotifiableType { get; set; } = string.Empty;

        [Required]
        [Column("notifiable_id")]
        public long NotifiableId { get; set; }

        [Required]
        [Column("data")]
        public string Data { get; set; } = string.Empty;

        [Column("read_at")]
        public DateTime? ReadAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
