using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("time_slots")]
    public class TimeSlotDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("title")]
        [MaxLength(191)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("from_time")]
        public TimeSpan FromTime { get; set; }

        [Required]
        [Column("to_time")]
        public TimeSpan ToTime { get; set; }

        [Required]
        [Column("last_order_time")]
        public TimeSpan LastOrderTime { get; set; }

        [Column("status")]
        public short Status { get; set; } = 1; // 1-active, 0-deactive

        [Column("is_free_delivery")]
        public short IsFreeDelivery { get; set; } = 0;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
