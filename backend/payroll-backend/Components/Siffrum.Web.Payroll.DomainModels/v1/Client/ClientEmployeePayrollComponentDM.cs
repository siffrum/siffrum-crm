namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientEmployeePayrollComponentDM : BaseGenericPayrollComponentDM
    {
        [Required]
        [MaxLength(15)]
        public float AmountYearly { get; set; }

        [ForeignKey(nameof(ClientEmployeeCTCDetail))]
        public int ClientEmployeeCTCDetailId { get; set; }

        public virtual ClientEmployeeCTCDetailDM ClientEmployeeCTCDetail { get; set; }
    }
}
