using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("sliders")]
    public class SliderDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("type_id")]
        [MaxLength(191)]
        public string TypeId { get; set; } = string.Empty;

        [Required]
        [Column("image")]
        [MaxLength(191)]
        public string Image { get; set; } = string.Empty;

        [Column("slider_url")]
        public string? SliderUrl { get; set; }

        [Column("status")]
        public short Status { get; set; } = 1; // 1-active, 0-deactive

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public CategoryDM Category { get; set; }
        public ProductDM Product { get; set; }

        /*[NotMapped]
        public string TypeName =>
            Type == "category" ? Category?.Name :
            Type == "product" ? Product?.Name :
            Type == "offer_url" ? OfferUrl : string.Empty;

        [NotMapped]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? Image : $"/storage/{Image}";*/
    }
}
