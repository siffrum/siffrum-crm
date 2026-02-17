using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("product_tag")]
    public class ProductTagDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; } 
        

        [ForeignKey(nameof(ProductVariants))]
        [Required]
        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }
        public virtual ProductVariantDM ProductVariants { get; set; }        

        [ForeignKey(nameof(Tags))]
        [Required]
        [Column("tag_id")]
        public long TagId { get; set; }
        public virtual TagDM Tags { get; set; }

    }
}
