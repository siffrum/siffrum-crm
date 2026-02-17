using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("failed_jobs")]
    public class FailedJobDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("uuid")]
        [MaxLength(191)]
        public string Uuid { get; set; } = null!;

        [Required]
        [Column("connection")]
        public string Connection { get; set; } = null!;

        [Required]
        [Column("queue")]
        public string Queue { get; set; } = null!;

        [Required]
        [Column("payload")]
        public string Payload { get; set; } = null!;

        [Required]
        [Column("exception")]
        public string Exception { get; set; } = null!;

        [Column("failed_at")]
        public DateTime FailedAt { get; set; }
    }
}
