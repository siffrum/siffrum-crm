using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("oauth_personal_access_clients")]
    public class OauthPersonalAccessClientDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("client_id")]
        public long ClientId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
