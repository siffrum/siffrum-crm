using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("notifications")]
    public class NotificationDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("title")]
        [MaxLength(191)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("message")]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Column("type_link")]
        [MaxLength(191)]
        public string? TypeLink { get; set; }

        [Column("image")]
        [MaxLength(191)]
        public string? Image { get; set; }

        [Required]
        [Column("date_sent")]
        public DateTime DateSent { get; set; }
    }
}
