using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("app_usages")]
    public class AppUsageDM
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }   // INT UNSIGNED

        [Required]
        [Column("order_id")]
        [MaxLength(191)]
        public string OrderId { get; set; } = null!;

        [Required]
        [Column("device_type")]
        [MaxLength(191)]
        public string DeviceType { get; set; } = null!;

        [Required]
        [Column("app_version")]
        [MaxLength(191)]
        public string AppVersion { get; set; } = null!;

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
