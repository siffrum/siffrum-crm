using Siffrum.Ecom.DomainModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("blogs")]
    public class BlogDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED → long

        [Required]
        [Column("title")]
        [MaxLength(191)]
        public string Title { get; set; } = null!;

        [Required]
        [Column("slug")]
        [MaxLength(191)]
        public string Slug { get; set; } = null!;

          // BIGINT UNSIGNED → long

        [Column("image")]
        [MaxLength(191)]
        public string? Image { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("short_description")]
        public string? ShortDescription { get; set; }

        [Column("tags")]
        public string? Tags { get; set; }

        [Column("meta_title")]
        [MaxLength(191)]
        public string? MetaTitle { get; set; }

        [Column("meta_keywords")]
        public string? MetaKeywords { get; set; }

        [Column("meta_description")]
        public string? MetaDescription { get; set; }

        [Required]
        [Column("views_count")]
        public int ViewsCount { get; set; } = 0;

        [Required]
        [Column("status")]
        public StatusDM Status { get; set; } // 1 = Active, 0 = Inactive

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(BlogCategory))]
        [Required]
        [Column("category_id")]
        public long BlogCategoryId { get; set; }
        public virtual BlogCategory BlogCategory { get; set; }
        public virtual ICollection<BlogViewDM> Views { get; set; } = new HashSet<BlogViewDM>();


    }
}
