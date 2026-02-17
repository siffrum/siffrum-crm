using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("recently_visited_products")]
    public class RecentlyVisitedProductDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }

        [Required]
        [Column("visited_at")]
        public DateTime VisitedAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public UserDM? User { get; set; }
        public ProductDM? Product { get; set; }
    }
}
