using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("order_items")]
    public class OrderItemDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("orders_id")]
        [MaxLength(191)]
        public string OrdersId { get; set; } = string.Empty;

        [Column("product_name")]
        public string? ProductName { get; set; }

        [Column("variant_name")]
        public string? VariantName { get; set; }

        [Required]
        [Column("product_variant_id")]
        public int ProductVariantId { get; set; }

        [Column("delivery_boy_id")]
        public int DeliveryBoyId { get; set; } = 0;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("discounted_price")]
        public double DiscountedPrice { get; set; }

        [Column("tax_amount")]
        public decimal TaxAmount { get; set; } = 0.00m;

        [Column("tax_percentage")]
        public decimal TaxPercentage { get; set; } = 0.00m;

        [Column("discount")]
        public decimal Discount { get; set; } = 0.00m;

        [Required]
        [Column("sub_total")]
        public decimal SubTotal { get; set; }

        [Column("refund_amount")]
        public decimal? RefundAmount { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Column("active_status")]
        [MaxLength(191)]
        public string ActiveStatus { get; set; } = string.Empty;

        [Column("cancellation_reason")]
        public string? CancellationReason { get; set; }

        [Column("canceled_at")]
        public DateTime? CanceledAt { get; set; }

        [Required]
        [Column("seller_id")]
        public int SellerId { get; set; }

        [Column("is_credited")]
        public short IsCredited { get; set; } = 0;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // --------------------
        // Relationships
        // --------------------
        public ICollection<ProductImagesDM> Images { get; set; } = new List<ProductImagesDM>();
        public SellerDM? Seller { get; set; }
        public ProductVariantDM? ProductVariant { get; set; }
        public UserDM? User { get; set; }

      /*  // --------------------
        // Computed
        // --------------------
        [NotMapped]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? Image ?? "" : $"storage/{Image}";*/
    }
}
