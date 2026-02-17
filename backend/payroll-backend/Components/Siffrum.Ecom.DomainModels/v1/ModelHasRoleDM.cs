using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("model_has_roles")]
    public class ModelHasRoleDM
    {
        [Key]
        [Column("role_id")]
        public long RoleId { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("model_type")]
        [MaxLength(191)]
        public string ModelType { get; set; } = string.Empty;

        [Required]
        [Column("model_id")]
        public long ModelId { get; set; }   // BIGINT UNSIGNED
    }
}
