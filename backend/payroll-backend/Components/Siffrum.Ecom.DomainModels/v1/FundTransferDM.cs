using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("fund_transfers")]
    public class FundTransferDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("delivery_boy_id")]
        public int DeliveryBoyId { get; set; }

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = null!; // credit | debit

        [Column("opening_balance")]
        public double OpeningBalance { get; set; }

        [Column("closing_balance")]
        public double ClosingBalance { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = null!;

        [Required]
        [Column("message")]
        [MaxLength(191)]
        public string Message { get; set; } = null!;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /*public static readonly string TypeDebit = "debit";
        public static readonly string TypeCredit = "credit";*/
    }
}
