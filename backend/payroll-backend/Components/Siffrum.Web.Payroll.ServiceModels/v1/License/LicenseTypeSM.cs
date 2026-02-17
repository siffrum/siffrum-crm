using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.License
{
    public class LicenseTypeSM : SiffrumPayrollServiceModelBase<int>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int ValidityInDays { get; set; }
        public double Amount { get; set; }
        public string LicenseTypeCode { get; set; }
        public string StripePriceId { get; set; }
        public bool IsPredefined { get; set; }
        public RoleTypeSM ValidFor { get; set; }
        public List<int>? PermissionIds { get; set; }
        public List<PermissionSM>? Permissions { get; set; }
    }
}
