using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("sessions")]
    public class SessionDM
    {
        [Key]
        [Column("id")]
        [MaxLength(191)]
        public string Id { get; set; } = string.Empty;

        [Column("user_id")]
        public long? UserId { get; set; }

        [Column("ip_address")]
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        [Column("user_agent")]
        public string? UserAgent { get; set; }

        [Required]
        [Column("payload")]
        public string Payload { get; set; } = string.Empty;

        [Required]
        [Column("last_activity")]
        public int LastActivity { get; set; }
    }
}
