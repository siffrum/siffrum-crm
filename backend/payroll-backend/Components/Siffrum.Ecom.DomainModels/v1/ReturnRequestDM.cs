using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Siffrum.Ecom.DomainModels.v1.Helpers;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("return_requests")]
    public class ReturnRequestDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("product_variant_id")]
        public int ProductVariantId { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Required]
        [Column("reason")]
        public string Reason { get; set; } = string.Empty;

        [Column("status")]
        public short Status { get; set; } = 0;

        [Column("remarks")]
        public string? Remarks { get; set; }

        [Required]
        [Column("delivery_boy_id")]
        public int DeliveryBoyId { get; set; }

        [Column("cancellation_reason")]
        public string? CancellationReason { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public UserDM? User { get; set; }
        public OrderDM? Order { get; set; }
        public OrderItemDM? OrderItem { get; set; }
        public ProductVariantDM? ProductVariant { get; set; }
        public DeliveryBoyDM? DeliveryBoy { get; set; }

        /*[NotMapped]
        public string StatusName =>
            ReturnStatusList.GetStatusName(Status);*/
    }
}
