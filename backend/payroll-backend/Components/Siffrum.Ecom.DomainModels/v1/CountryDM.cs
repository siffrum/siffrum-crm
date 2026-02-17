using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("countries")]
    public class CountryDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = null!;

        [Required]
        [Column("dial_code")]
        [MaxLength(191)]
        public string DialCode { get; set; } = null!;

        [Required]
        [Column("code")]
        [MaxLength(191)]
        public string Code { get; set; } = null!;

        [Column("logo")]
        public string? Logo { get; set; }

        [Required]
        [Column("status")]
        public int Status { get; set; } = 1;   // tinyint → int
    }
}
