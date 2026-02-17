using static System.Collections.Specialized.BitVector32;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("offers")]
    public class OfferDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("image")]
        [MaxLength(191)]
        public string Image { get; set; } = string.Empty;

        [Required]
        [Column("position")]
        [MaxLength(191)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [Column("section_position")]
        [MaxLength(191)]
        public string SectionPosition { get; set; } = string.Empty;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("type_id")]
        [MaxLength(191)]
        public string TypeId { get; set; } = string.Empty;

        [Column("offer_url")]
        public string? OfferUrl { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // --------------------
        // Relationships
        // --------------------
        public CategoryDM? Category { get; set; }
        public ProductDM? Product { get; set; }
        public SectionDM? Section { get; set; }

       /* // --------------------
        // Computed properties (Laravel $appends)
        // --------------------
        [NotMapped]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? Image ?? "" : $"storage/{Image}";

        [NotMapped]
        public string TypeName =>
            Type == "category" ? Category?.Name ?? "" :
            Type == "product" ? Product?.Name ?? "" : "";

        [NotMapped]
        public string TypeSlug =>
            Type == "category" ? Category?.Slug ?? "" :
            Type == "product" ? Product?.Slug ?? "" : "";

        [NotMapped]
        public string SectionTitle =>
            Position == "below_section"
                ? $"Below {Section?.Title ?? ""}"
                : "";*/
    }
}
