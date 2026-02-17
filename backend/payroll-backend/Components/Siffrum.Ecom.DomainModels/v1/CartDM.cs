using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("carts")]
    public class CartDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("product_variant_id")]
        public int ProductVariantId { get; set; }

        [Required]
        [Column("qty")]
        public int Qty { get; set; }

        [Required]
        [Column("save_for_later")]
        public int SaveForLater { get; set; } = 0; // tinyint → int

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        // Relations
        public ProductDM Product { get; set; }
        public ProductVariantDM Variant { get; set; }

        public ICollection<ProductImagesDM> Images { get; set; }

        
    }
}
