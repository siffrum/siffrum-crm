using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeAdditionalReimbursementLogSM : SiffrumPayrollServiceModelBase<int>
    {
        public int? ClientUserId { get; set; }
        public ReimbursementTypeSM ReimbursementType { get; set; }
        public string ReimburseDocumentName { get; set; }
        public string Extension { get; set; }
        public string ReimbursementDescription { get; set; }
        public decimal ReimbursementAmount { get; set; }
        public DateTime ReimbursementDate { get; set; }
        public string ReimbursementDocumentPath { get; set; }
        public int ClientCompanyDetailId { get; set; }
    }
}
