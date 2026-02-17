using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("role_has_permissions")]
    public class RoleHasPermissionDM
    {
        [Key]
        [Column("permission_id")]
        public long PermissionId { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("role_id")]
        public long RoleId { get; set; }   // BIGINT UNSIGNED
    }
}
