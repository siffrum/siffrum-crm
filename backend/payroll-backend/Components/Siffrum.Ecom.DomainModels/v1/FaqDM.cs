using Siffrum.Ecom.DomainModels.Foundation.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("faqs")]
    public class FaqDM : SiffrumDomainModelBase<long>
    {
        [Required]
        [Column("question")]
        public string Question { get; set; }

        [Required]
        [Column("answer")]
        public string Answer { get; set; }

        [Column("status")]
        [MaxLength(191)]
        public bool Status { get; set; }        

        [ForeignKey(nameof(Seller))]
        [Column("seller_id")]
        public long? SellerId { get; set; }
        public virtual SellerDM? Seller { get; set; }
    }
}
