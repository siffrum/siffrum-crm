using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("api_call_tracking")]
    public class ApiCallTrackingDM
    {
        [Key]
        [Column("id")]
        public ulong Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("api_name")]
        [MaxLength(191)]
        public string ApiName { get; set; } = null!;

        [Required]
        [Column("source")]
        [MaxLength(191)]
        public string Source { get; set; } = null!;

        [Required]
        [Column("count")]
        public ulong Count { get; set; } = 0; // BIGINT UNSIGNED DEFAULT 0

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
