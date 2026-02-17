using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("delivery_boy_notifications")]
    public class DeliveryBoyNotificationDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("delivery_boy_id")]
        public int DeliveryBoyId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = null!;

        [Required]
        [Column("message")]
        public string Message { get; set; } = null!;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = null!;

        [Column("order_item_id")]
        public int? OrderItemId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
