using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("blog_categories")]
    public class BlogCategory
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED → long

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = null!;

        [Required]
        [Column("slug")]
        [MaxLength(191)]
        public string Slug { get; set; } = null!;

        [Column("meta_title")]
        [MaxLength(191)]
        public string? MetaTitle { get; set; }

        [Column("meta_keywords")]
        public string? MetaKeywords { get; set; }

        [Column("meta_description")]
        public string? MetaDescription { get; set; }

        [Required]
        [Column("status")]
        public int Status { get; set; } = 1;   // 1 = Active, 0 = Inactive

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<BlogDM> Blogs { get; set; }
       
    }
}
