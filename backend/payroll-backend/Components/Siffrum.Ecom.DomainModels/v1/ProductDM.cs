using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Siffrum.Ecom.DomainModels.Enums;
using Siffrum.Ecom.DomainModels.Foundation.Base;
using Microsoft.EntityFrameworkCore;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("products")]
    [Index(nameof(SellerId), nameof(Slug), IsUnique = true)]
    public class ProductDM : SiffrumDomainModelBase<long>
    {       

        [Required]
        [Column("row_order")]
        public int RowOrder { get; set; } = 0;

        [Required]
        [Column("name")]
        [MaxLength(191)]
        public string Name { get; set; }                 

        [Required]
        [Column("slug")]
        [MaxLength(191)]
        public string Slug { get; set; }       

        [Column("indicator")]
        public ProductIndicatorDM Indicator { get; set; }

        [Column("manufacturer")]
        [MaxLength(191)]
        public string? Manufacturer { get; set; }

        [Column("made_in")]
        [MaxLength(191)]
        public string? MadeIn { get; set; }

        [Column("is_cancelable")]
        public bool IsCancelable { get; set; }

        [Column("image")]
        public string? Image { get; set; } 
/*
        [Column("other_images")]
        [MaxLength(191)]
        public string? OtherImages { get; set; }*/

        [Required]
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("status")]
        public ProductStatusDM Status { get; set; } = 0;

        [Column("is_approved")]
        public bool IsApproved { get; set; }

        [Column("return_days")]
        public int ReturnDays { get; set; } 

        [Column("type")]
        public string? Type { get; set; }

        [Column("is_unlimited_stock")]
        public bool IsUnlimitedStock { get; set; } 

        [Column("is_cod_allowed")]
        public bool IsCodAllowed { get; set; }        

        [Required]
        [Column("fssai_lic_no")]
        [MaxLength(191)]
        public string FssaiLicNo { get; set; } = string.Empty;

        [Column("barcode")]
        public string? Barcode { get; set; }

        [Column("meta_title")]
        [MaxLength(191)]
        public string? MetaTitle { get; set; }

        [Column("meta_keywords")]
        public string? MetaKeywords { get; set; }

        [Column("schema_markup")]
        public string? SchemaMarkup { get; set; }

        [Column("meta_description")]
        public string? MetaDescription { get; set; }

        // Relationships
        [ForeignKey(nameof(Seller))]
        [Required]
        [Column("seller_id")]
        public long SellerId { get; set; }
        public virtual SellerDM? Seller { get; set; }
        [ForeignKey(nameof(Category))]
        [Required]
        [Column("category_id")]
        public long CategoryId { get; set; }
        public virtual CategoryDM? Category { get; set; }
        [ForeignKey(nameof(Brand))]
        [Column("brand_id")]
        public long? BrandId { get; set; }
        public virtual BrandDM? Brand { get; set; }
        [ForeignKey(nameof(Tax))]
        [Column("tax_id")]
        public long? TaxId { get; set; }
        public virtual TaxDM? Tax { get; set; }
        public ICollection<ProductVariantDM> Variants { get; set; } 

    }
}
