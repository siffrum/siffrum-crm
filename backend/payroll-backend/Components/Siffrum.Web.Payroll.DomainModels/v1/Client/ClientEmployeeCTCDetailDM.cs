using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class ClientEmployeeCTCDetailDM : SiffrumPayrollDomainModelBase<int>
    {
        [Required]
        [MaxLength(18)]
        public float CTCAmount { get; set; }

        [Required]
        [MaxLength(4)]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Start Date of Payroll is Required")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateUtc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDateUtc { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int ClientUserId { get; set; }

        public bool CurrentlyActive { get; set; }

        public virtual ClientUserDM ClientUser { get; set; }

        //[ForeignKey(nameof(ClientCompanyDetail))]
        //public int ClientCompanyDetailId { get; set; }

        //public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        public virtual HashSet<ClientEmployeePayrollComponentDM> ClientEmployeePayrollComponents { get; set; }
    }
}
