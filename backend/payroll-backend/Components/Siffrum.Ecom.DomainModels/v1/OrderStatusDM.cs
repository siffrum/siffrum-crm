using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("order_statuses")]
    public class OrderStatusDM
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }   // INT UNSIGNED

        [Required]
        [Column("order_id")]
        [MaxLength(191)]
        public string OrderId { get; set; } = string.Empty;

        [Column("order_item_id")]
        public int? OrderItemId { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Required]
        [Column("user_type")]
        public int UserType { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /*public static int UserTypeScript = 0;
        public static int UserTypeAdmin = 1;
        public static int UserTypeUser = 2;

        // --------------------
        // Computed
        // --------------------
        [NotMapped]
        public string CreatedByName { get; set; } = "";

        [NotMapped]
        public string DisplayDateTime =>
            TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(CreatedAt, DateTimeKind.Utc),
                TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")
            ).ToString("dd-MM-yyyy hh:mm:ss tt");*/
    }
}
