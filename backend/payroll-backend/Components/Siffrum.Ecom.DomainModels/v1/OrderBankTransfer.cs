using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("order_bank_transfers")]
    public class OrderBankTransfer
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("attachment")]
        public string Attachment { get; set; } = string.Empty;

        [Column("message")]
        public string? Message { get; set; }

        [Column("status")]
        public short Status { get; set; } = 0;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
