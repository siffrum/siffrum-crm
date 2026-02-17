using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("live_tracking")]
    public class LiveTrackingDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("order_id")]
        public long OrderId { get; set; }

        [Required]
        [Column("latitude")]
        public decimal Latitude { get; set; }   // decimal(10,7)

        [Required]
        [Column("longitude")]
        public decimal Longitude { get; set; }  // decimal(10,7)

        [Column("tracked_at")]
        public DateTime? TrackedAt { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Relationship
        public OrderDM? Order { get; set; }
    }
}
