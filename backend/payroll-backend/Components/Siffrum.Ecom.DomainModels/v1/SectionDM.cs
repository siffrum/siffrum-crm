using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("sections")]
    public class SectionDM
    {
        [Key]
        [Column("id")]
        public ulong Id { get; set; }

        [Required]
        [Column("title")]
        [MaxLength(191)]
        public string Title { get; set; } = null!;

        [Required]
        [Column("short_description")]
        [MaxLength(191)]
        public string ShortDescription { get; set; } = null!;

        [Required]
        [Column("product_type")]
        [MaxLength(191)]
        public string ProductType { get; set; } = null!;

        [Column("product_ids", TypeName = "text")]
        public string? ProductIds { get; set; }

        [Column("category_ids", TypeName = "text")]
        public string? CategoryIds { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Required]
        [Column("position")]
        [MaxLength(191)]
        public string Position { get; set; } = null!;

        [Required]
        [Column("style_app")]
        [MaxLength(191)]
        public string StyleApp { get; set; } = null!;

        [Column("banner_app")]
        [MaxLength(191)]
        public string? BannerApp { get; set; }

        [Required]
        [Column("style_web")]
        [MaxLength(191)]
        public string StyleWeb { get; set; } = null!;

        [Column("banner_web")]
        [MaxLength(191)]
        public string? BannerWeb { get; set; }

        [Required]
        [Column("background_color_for_light_theme")]
        [MaxLength(191)]
        public string BackgroundColorForLightTheme { get; set; } = null!;

        [Required]
        [Column("background_color_for_dark_theme")]
        [MaxLength(191)]
        public string BackgroundColorForDarkTheme { get; set; } = null!;
    }
}
