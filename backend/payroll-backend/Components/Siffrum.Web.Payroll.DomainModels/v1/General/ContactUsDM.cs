namespace Siffrum.Web.Payroll.DomainModels.v1.General
{
    public class ContactUsDM : Base.SiffrumPayrollDomainModelBase<int>
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string EmailId { get; set; }

        [StringLength(500, MinimumLength = 3)]
        [Required]
        public string Message { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
