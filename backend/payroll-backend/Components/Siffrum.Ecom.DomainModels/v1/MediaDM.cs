using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("media")]
    public class MediaDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("extension")]
        [MaxLength(191)]
        public string Extension { get; set; } = null!;

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = null!;

        [Required]
        [Column("sub_directory")]
        public string SubDirectory { get; set; } = null!;

        [Required]
        [Column("size")]
        public string Size { get; set; } = null!;

        [Column("seller_id")]
        public int? SellerId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
