using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("seller_commissions")]
    public class SellerCommission
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("seller_id")]
        public int SellerId { get; set; }

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("commission")]
        public int Commission { get; set; } = 0;
    }
}
