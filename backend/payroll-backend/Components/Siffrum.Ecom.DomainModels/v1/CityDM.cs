using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("cities")]
    public class CityDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = null!;

        [Required]
        [Column("zone")]
        [MaxLength(191)]
        public string Zone { get; set; } = null!;

        [Required]
        [Column("state")]
        [MaxLength(191)]
        public string State { get; set; } = null!;

        [Required]
        [Column("formatted_address")]
        [MaxLength(191)]
        public string FormattedAddress { get; set; } = null!;

        [Column("latitude")]
        [MaxLength(191)]
        public string? Latitude { get; set; }

        [Column("longitude")]
        [MaxLength(191)]
        public string? Longitude { get; set; }

        [Column("min_amount_for_free_delivery")]
        [MaxLength(191)]
        public string? MinAmountForFreeDelivery { get; set; }

        [Column("delivery_charge_method")]
        [MaxLength(191)]
        public string? DeliveryChargeMethod { get; set; }

        [Column("fixed_charge")]
        public decimal FixedCharge { get; set; } = 0.00m;

        [Column("per_km_charge")]
        public decimal PerKmCharge { get; set; } = 0.00m;

        [Column("range_wise_charges")]
        public string? RangeWiseCharges { get; set; }

        [Column("time_to_travel")]
        public int TimeToTravel { get; set; } = 0;

        [Column("geolocation_type")]
        [MaxLength(191)]
        public string? GeolocationType { get; set; }

        [Column("radius")]
        [MaxLength(191)]
        public string Radius { get; set; } = "0";

        [Column("boundary_points")]
        public string? BoundaryPoints { get; set; }

        [Column("max_deliverable_distance")]
        public int MaxDeliverableDistance { get; set; } = 0;

        //public HashSet<SellerDM> Seller { get; set; }
    }
}
