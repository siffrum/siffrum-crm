using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("emails")]
    public class EmailDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("title")]
        [MaxLength(191)]
        public string Title { get; set; } = null!;

        [Required]
        [Column("message")]
        public string Message { get; set; } = null!;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = null!;

        [Required]
        [Column("type_id")]
        [MaxLength(191)]
        public string TypeId { get; set; } = null!;

        [Column("image")]
        [MaxLength(191)]
        public string? Image { get; set; }

        [Column("date_sent")]
        public DateTime DateSent { get; set; }
    }
}
