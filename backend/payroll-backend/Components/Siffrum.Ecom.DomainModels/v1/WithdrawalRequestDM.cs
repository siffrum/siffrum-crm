using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("withdrawal_requests")]
    public class WithdrawalRequestDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("type")]
        [MaxLength(191)]
        public string Type { get; set; } = string.Empty; // 'user', 'seller', 'delivery_boy'

        [Required]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Required]
        [Column("amount")]
        public double Amount { get; set; }

        [Required]
        [Column("message")]
        public string Message { get; set; } = string.Empty;

        [Column("status")]
        public short Status { get; set; } = 0;

        [Column("remark")]
        public string? Remark { get; set; }

        [Column("receipt_image")]
        [MaxLength(191)]
        public string? ReceiptImage { get; set; }

        [Column("device_type")]
        [MaxLength(191)]
        public string? DeviceType { get; set; } // '0=>Web, 1=>Android, 2=>IOS'

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

       /* // Types
        public static readonly string TypeUser = "user";
        public static readonly string TypeSeller = "seller";
        public static readonly string TypeDeliveryBoy = "delivery_boy";

        // Status
        public static readonly int StatusPending = 0;
        public static readonly int StatusApproved = 1;
        public static readonly int StatusRejected = 2;

        public static readonly string Pending = "Pending";
        public static readonly string Approved = "Approved";
        public static readonly string Rejected = "Rejected";

        // Device
        public static readonly int DeviceWeb = 0;
        public static readonly int DeviceAndroid = 1;
        public static readonly int DeviceIos = 2;

        public static readonly string Web = "Web";
        public static readonly string Android = "Android";
        public static readonly string Ios = "IOS";*/
       /*

        [NotMapped]
        public string? ReceiptImageUrl =>
            string.IsNullOrEmpty(ReceiptImage)
                ? ReceiptImage
                : $"storage/{ReceiptImage}";

        [NotMapped]
        public string DeviceTypeText =>
            DeviceType switch
            {
                0 => Web,
                1 => Android,
                _ => Ios
            };

        [NotMapped]
        public string DisplayType =>
            System.Globalization.CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(Type.Replace("_", " "));

        [NotMapped]
        public string OrigionalType =>
            DisplayType.ToLower().Replace(" ", "_");*/
    }
}
