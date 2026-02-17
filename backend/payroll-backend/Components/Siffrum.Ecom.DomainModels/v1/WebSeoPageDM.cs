using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("web_seo_pages")]
    [Index(nameof(PageType), IsUnique = true)]
    public class WebSeoPageDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("meta_title")]
        [MaxLength(191)]
        public string? MetaTitle { get; set; }

        [Column("meta_description")]
        public string? MetaDescription { get; set; }

        [Column("meta_keyword")]
        public string? MetaKeyword { get; set; }

        [Column("schema_markup")]
        public string? SchemaMarkup { get; set; }

        [Required]
        [Column("page_type")]
        [MaxLength(191)]
        public string PageType { get; set; } = string.Empty;

        [Column("og_image")]
        public string? OgImage { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
