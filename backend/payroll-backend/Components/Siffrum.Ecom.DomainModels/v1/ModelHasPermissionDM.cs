using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("model_has_permissions")]
    public class ModelHasPermissionDM
    {
        [Key]
        [Column("permission_id")]
        public long PermissionId { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("model_type")]
        [MaxLength(191)]
        public string ModelType { get; set; } = string.Empty;

        [Required]
        [Column("model_id")]
        public long ModelId { get; set; }   // BIGINT UNSIGNED
    }
}
