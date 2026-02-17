using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("taxes")]
    public class TaxDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("title")]
        public string? Title { get; set; }

        [Required]
        [Column("percentage")]
        public double Percentage { get; set; } = 0;

        [Required]
        [Column("status")]
        public short Status { get; set; }
    }

}
