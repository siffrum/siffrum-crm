using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("area")]
    public class AreaDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED → long

        [Column("city_id")]
        public int CityId { get; set; } = 0;

        [Column("pincode_id")]
        public int? PincodeId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("minimum_free_delivery_order_amount")]
        public int MinimumFreeDeliveryOrderAmount { get; set; } = 0;

        [Required]
        [Column("delivery_charges")]
        public int DeliveryCharges { get; set; } = 0;
    }
}
