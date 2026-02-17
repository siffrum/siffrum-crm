using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("order_trackings")]
    public class OrderTrackingDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Required]
        [Column("shiprocket_order_id")]
        public int ShiprocketOrderId { get; set; }

        [Required]
        [Column("shipment_id")]
        public int ShipmentId { get; set; }

        [Column("courier_company_id")]
        public int? CourierCompanyId { get; set; }

        [Column("awb_code")]
        [MaxLength(191)]
        public string? AwbCode { get; set; }

        [Column("tracking_url")]
        [MaxLength(191)]
        public string? TrackingUrl { get; set; }

        [Required]
        [Column("pickup_status")]
        public int PickupStatus { get; set; }

        [Required]
        [Column("pickup_scheduled_date")]
        [MaxLength(191)]
        public string PickupScheduledDate { get; set; } = string.Empty;

        [Required]
        [Column("pickup_token_number")]
        [MaxLength(191)]
        public string PickupTokenNumber { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        public int Status { get; set; }

        [Required]
        [Column("others")]
        [MaxLength(191)]
        public string Others { get; set; } = string.Empty;

        [Required]
        [Column("pickup_generated_date")]
        [MaxLength(191)]
        public string PickupGeneratedDate { get; set; } = string.Empty;

        [Required]
        [Column("data")]
        [MaxLength(191)]
        public string Data { get; set; } = string.Empty;

        [Required]
        [Column("date")]
        [MaxLength(191)]
        public string Date { get; set; } = string.Empty;

        [Required]
        [Column("is_canceled")]
        public int IsCanceled { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
