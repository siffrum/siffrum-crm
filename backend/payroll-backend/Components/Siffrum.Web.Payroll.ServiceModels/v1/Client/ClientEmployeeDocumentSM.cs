using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class ClientEmployeeDocumentSM : SiffrumPayrollServiceModelBase<int>
    {
        public int ClientUserId { get; set; }
        public int ClientCompanyDetailId { get; set; }
        public string Name { get; set; }
        public string EmployeeDocumentPath { get; set; }
        public EmployeeDocumentTypeSM EmployeeDocumentType { get; set; }
        public string DocumentDescription { get; set; }
        public string Extension { get; set; }

    }
}

