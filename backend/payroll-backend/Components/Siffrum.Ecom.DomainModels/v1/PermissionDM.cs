using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("permissions")]
    public class PermissionDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("guard_name")]
        [MaxLength(191)]
        public string GuardName { get; set; } = string.Empty;

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /*public static readonly Dictionary<string, string[]> RouteMatch =
            new()
            {
                { "manage_dashboard", new[] { "home" } },
                { "manage_order", new[] { "deliveries.manage" } },
                { "create_order", new[] { "deliveries.create", "deliveries.store" } },
                { "order_details", new[] { "delivery.detail" } },
                { "delete_order", new[] { "delivery.delete" } },
                { "change_order_status", new[] { "change_status" } },
                { "change_region", new[] { "change_bus" } },
                { "change_driver", new[] { "change_driver" } }
            };*/
    }
}
