using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("product_images")]
    public class ProductImagesDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED
           
        [Required]
        [Column("image")]
        [MaxLength(191)]
        public string Image { get; set; } = string.Empty;
        
        [ForeignKey(nameof(ProductVariants))]
        [Required]
        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }
        public virtual ProductVariantDM ProductVariants { get; set; }
    }
}
