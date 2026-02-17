using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{

    [Table("blog_views")]
    public class BlogViewDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("ip_address")]
        [MaxLength(191)]
        public string IpAddress { get; set; } = null!;

        [Column("blog_id")]
        public long BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public virtual BlogDM Blog { get; set; } = null!;
    }

}
