using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("mail_settings")]
    public class MailSettingDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("user_type")]
        public int UserType { get; set; } // 0 = User, 1 = Admin

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("order_status_id")]
        public int OrderStatusId { get; set; }

        [Required]
        [Column("mail_status")]
        public int MailStatus { get; set; } // 0 = false, 1 = true

        [Required]
        [Column("mobile_status")]
        public int MobileStatus { get; set; } // 0 = false, 1 = true

        [Required]
        [Column("sms_status")]
        public bool SmsStatus { get; set; } = false;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
