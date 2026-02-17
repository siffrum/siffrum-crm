using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("user_product_requests")]
    public class UserProductRequestDM
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }   // BIGINT UNSIGNED

        [Required]
        [Column("customer_id")]
        public long CustomerId { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("image")]
        [MaxLength(191)]
        public string? Image { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(191)]
        public string Status { get; set; } = "pending"; // enum('pending','accepted','rejected')

        [Column("product_id")]
        public long? ProductId { get; set; }

        [Column("admin_notes")]
        public string? AdminNotes { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public UserDM Customer { get; set; }
        public ProductDM Product { get; set; }
        /*public const string StatusPending = "pending";
        public const string StatusAccepted = "accepted";
        public const string StatusRejected = "rejected";

        [JsonIgnore]


        [NotMapped]
        public string ImageUrl =>
            string.IsNullOrEmpty(Image) ? null : $"/storage/{Image}";*/
    }
}
