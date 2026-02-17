using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("sub_categories")]
    public class SubCategoryDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Column("row_order")]
        public int RowOrder { get; set; } = 0;

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("slug")]
        [MaxLength(191)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [Column("subtitle")]
        public string Subtitle { get; set; } = string.Empty;

        [Required]
        [Column("image")]
        public string Image { get; set; } = string.Empty;

        public CategoryDM Category { get; set; }
        /*[NotMapped]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? Image : $"/storage/{Image}";*/

    }
}
