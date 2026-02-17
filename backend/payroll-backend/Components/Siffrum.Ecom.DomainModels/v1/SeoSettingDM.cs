using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siffrum.Ecom.DomainModels.v1
{
    [Table("seo_settings")]
    public class SeoSettingDM
    {
        [Key]
        public long Id { get; set; }

        public string PageType { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string SchemaMarkup { get; set; }
        public string OgImage { get; set; }

        /*[NotMapped]
        public string OgImageUrl =>
            string.IsNullOrEmpty(OgImage) ? OgImage : $"/storage/{OgImage}";*/
    }
}
