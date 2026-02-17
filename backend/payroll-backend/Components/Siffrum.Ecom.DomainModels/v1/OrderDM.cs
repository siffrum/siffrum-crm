using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("orders")]
    public class OrderDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("delivery_boy_id")]
        public long? DeliveryBoyId { get; set; }

        [Column("delivery_boy_bonus_details")]
        public string? DeliveryBoyBonusDetails { get; set; }

        [Column("delivery_boy_bonus_amount")]
        public double? DeliveryBoyBonusAmount { get; set; }

        [Column("transaction_id")]
        public long? TransactionId { get; set; }

        [Column("orders_id")]
        [MaxLength(191)]
        public string? OrdersId { get; set; }

        [Column("otp")]
        public int? Otp { get; set; }

        [Required]
        [Column("mobile")]
        [MaxLength(191)]
        public string Mobile { get; set; } = string.Empty;

        [Column("order_note")]
        public string? OrderNote { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("delivery_charge")]
        public decimal DeliveryCharge { get; set; }

        [Column("tax_amount")]
        public decimal TaxAmount { get; set; } = 0.00m;

        [Column("tax_percentage")]
        public decimal TaxPercentage { get; set; } = 0.00m;

        [Column("wallet_balance")]
        public decimal WalletBalance { get; set; }

        [Column("discount")]
        public decimal Discount { get; set; } = 0.00m;

        [Column("additional_charges")]
        public string? AdditionalCharges { get; set; }

        [Column("promo_code_id")]
        public int PromoCodeId { get; set; } = 0;

        [Column("promo_code")]
        [MaxLength(191)]
        public string? PromoCode { get; set; }

        [Column("promo_discount")]
        public decimal PromoDiscount { get; set; } = 0.00m;

        [Column("final_total")]
        public decimal? FinalTotal { get; set; }

        [Required]
        [Column("payment_method")]
        [MaxLength(191)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("latitude")]
        [MaxLength(191)]
        public string Latitude { get; set; } = string.Empty;

        [Required]
        [Column("longitude")]
        [MaxLength(191)]
        public string Longitude { get; set; } = string.Empty;

        [Required]
        [Column("delivery_time")]
        [MaxLength(191)]
        public string DeliveryTime { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [Column("active_status")]
        [MaxLength(191)]
        public string ActiveStatus { get; set; } = string.Empty;

        [Required]
        [Column("order_type")]
        public string OrderType { get; set; } = "doorstep"; // enum('doorstep','selfpickup')

        [Column("pickup_address")]
        public string? PickupAddress { get; set; }

        [Column("order_from")]
        public int OrderFrom { get; set; } = 0;

        [Column("pincode_id")]
        public int PincodeId { get; set; } = 0;

        [Column("address_id")]
        public int AddressId { get; set; } = 0;

        [Column("area_id")]
        public int? AreaId { get; set; }

        [Column("remaining_total")]
        public decimal? RemainingTotal { get; set; }

        [Column("subscription_id")]
        public long? SubscriptionId { get; set; }

        [Column("delivery_save_amount")]
        public double DeliverySaveAmount { get; set; } = 0;

        [Column("remaining_final")]
        public decimal? RemainingFinal { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // --------------------
        // Constants
        // --------------------
        /*public static int ActiveType = 1;
        public static int PreviousType = 0;
        public static int PreviousTypeStatus = 0;*/

        // --------------------
        // Relationships
        // --------------------
        public ICollection<OrderItemDM> Items { get; set; } = new List<OrderItemDM>();
        public UserDM? User { get; set; }
        public ICollection<OrderStatusDM> OrderStatuses { get; set; } = new List<OrderStatusDM>();
        public UserSubscriptionDM? Subscription { get; set; }

        // --------------------
        // JSON helpers (Laravel mutator/accessor)
        // --------------------
       /* [NotMapped]
        public object? DeliveryBoyBonusDetailsObject
        {
            get => string.IsNullOrEmpty(DeliveryBoyBonusDetails)
                ? null
                : JsonSerializer.Deserialize<object>(DeliveryBoyBonusDetails);

            set => DeliveryBoyBonusDetails = JsonSerializer.Serialize(value);
        }*/
    }
}
