using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("subscription_faqs")]
    public class SubscriptionFaqDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("question")]
        public string Question { get; set; } = string.Empty;

        [Column("answer")]
        public string? Answer { get; set; }

        [Column("sort_order")]
        public int SortOrder { get; set; } = 0;

        [Column("status")]
        public short Status { get; set; } = 1; // 0=Inactive, 1=Active

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
