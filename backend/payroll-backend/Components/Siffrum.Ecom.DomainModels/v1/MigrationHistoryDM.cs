using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("migrations")]
    public class MigrationHistoryDM
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("migration")]
        [MaxLength(191)]
        public string Migration { get; set; } = null!;

        [Required]
        [Column("batch")]
        public int Batch { get; set; }
    }
}
