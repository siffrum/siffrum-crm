using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("email_templates")]
    public class EmailTemplateDM
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

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
