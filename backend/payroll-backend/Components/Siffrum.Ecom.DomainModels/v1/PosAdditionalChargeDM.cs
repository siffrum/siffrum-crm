using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("pos_additional_charges")]
    public class PosAdditionalChargeDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("pos_order_id")]
        public int PosOrderId { get; set; }

        [Required]
        [Column("charge_name")]
        [MaxLength(191)]
        public string ChargeName { get; set; } = string.Empty;

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
