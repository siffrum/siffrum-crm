using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("jobs")]
    public class JobDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("queue")]
        [MaxLength(191)]
        public string Queue { get; set; } = null!;

        [Required]
        [Column("payload")]
        public string Payload { get; set; } = null!;

        [Column("attempts")]
        public int Attempts { get; set; }

        [Column("reserved_at")]
        public int? ReservedAt { get; set; }

        [Column("available_at")]
        public int AvailableAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
