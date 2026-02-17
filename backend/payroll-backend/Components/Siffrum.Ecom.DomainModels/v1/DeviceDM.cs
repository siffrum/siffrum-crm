using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("devices")]
    public class DeviceDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Required]
        [Column("fcm_id")]
        [MaxLength(191)]
        public string FcmId { get; set; } = null!;

        [Column("seller_id")]
        public int? SellerId { get; set; }
    }
}
