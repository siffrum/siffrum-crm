using Siffrum.Web.Payroll.ServiceModels.Base;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeCTCDetailSM : SiffrumPayrollServiceModelBase<int>
    {
        public float CtcAmount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime StartDateUTC { get; set; }

        public DateTime? EndDateUTC { get; set; }

        public bool CurrentlyActive { get; set; }

        public int ClientUserId { get; set; }

        //public int ClientCompanyDetailId { get; set; }

        public List<ClientEmployeePayrollComponentSM> ClientEmployeePayrollComponents { get; set; }

    }
}
