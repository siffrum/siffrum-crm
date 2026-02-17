using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Siffrum.Ecom.DomainModels.Enums;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("product_variants")]
    public class ProductVariantDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Column("total_allowed_quantity")]
        public int TotalAllowedQuantity { get; set; }

        [Column("is_tax_included_in_price")]
        public bool IsTaxIncludedInPrice { get; set; }

        [Column("status")]
        public ProductStatusDM Status { get; set; }

        [Column("return_policy")]
        public ProductReturnPolicyDM ReturnPolicy { get; set; }


        [Required]
        [Column("measurement")]
        public decimal Measurement { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("discounted_price")]
        public decimal DiscountedPrice { get; set; } = 0.00m;

        [Column("stock")]
        public decimal Stock { get; set; } = 0.00m;

        [Column("stock_unit_id")]
        public int StockUnitId { get; set; } = 0;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(Product))]
        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }
        public ProductDM Product { get; set; }
        public ICollection<ProductImagesDM> Images { get; set; }
        public ICollection<ProductTagDM> ProductTags { get; set; }
        public ICollection<ProductFaqDM> ProductFaqs { get; set; }
        public ICollection<ProductRatingDM> Ratings { get; set; }

    }
}
