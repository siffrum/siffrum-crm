using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("invoice")]
    public class InvoiceDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("invoice_date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("address")]
        public string Address { get; set; } = null!;

        [Required]
        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column("phone_number")]
        [MaxLength(191)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [Column("order_list")]
        public string OrderList { get; set; } = null!;

        [Required]
        [Column("email")]
        [MaxLength(191)]
        public string Email { get; set; } = null!;

        [Required]
        [Column("discount")]
        [MaxLength(191)]
        public string Discount { get; set; } = null!;

        [Required]
        [Column("total_sale")]
        [MaxLength(191)]
        public string TotalSale { get; set; } = null!;

        [Required]
        [Column("shipping_charge")]
        [MaxLength(191)]
        public string ShippingCharge { get; set; } = null!;

        [Required]
        [Column("payment")]
        public string Payment { get; set; } = null!;

        [Required]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
