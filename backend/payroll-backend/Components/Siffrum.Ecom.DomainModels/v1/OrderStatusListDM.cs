using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("order_status_lists")]
    public class OrderStatusListDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = string.Empty;
    }
}
