using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("promo_codes")]
    public class PromoCodeDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("promo_code")]
        [MaxLength(191)]
        public string PromoCodeValue { get; set; } = string.Empty;

        [Required]
        [Column("message")]
        [MaxLength(191)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("no_of_users")]
        public int NoOfUsers { get; set; }

        [Required]
        [Column("minimum_order_amount")]
        public int MinimumOrderAmount { get; set; }

        [Required]
        [Column("discount")]
        public int Discount { get; set; }

        [Required]
        [Column("discount_type")]
        [MaxLength(191)]
        public string DiscountType { get; set; } = string.Empty;

        [Required]
        [Column("max_discount_amount")]
        public int MaxDiscountAmount { get; set; }

        [Column("repeat_usage")]
        public short RepeatUsage { get; set; } // 1-allowed, 0-Not Allowed

        [Column("no_of_repeat_usage")]
        public int NoOfRepeatUsage { get; set; } = 0;

        [Column("status")]
        public short Status { get; set; } = 1; // 1-active, 0-deactive

        [Required]
        [Column("image")]
        [MaxLength(191)]
        public string Image { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /*[NotMapped]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? "" : $"storage/{Image}";

        [NotMapped]
        public int IsApplicable =>
            DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate ? 1 : 0;

        [NotMapped]
        public string Validity =>
            IsApplicable == 1 ? "Acceptable" : "Expired";*/
    }
}
