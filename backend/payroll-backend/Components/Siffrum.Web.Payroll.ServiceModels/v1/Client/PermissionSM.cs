using Siffrum.Web.Payroll.ServiceModels.Base;
using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class PermissionSM : SiffrumPayrollServiceModelBase<int>
    {
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool IsEnabledForClient { get; set; }
        public ModuleNameSM ModuleName { get; set; }
        public RoleTypeSM RoleType { get; set; }
        public int CompanyModulesId { get; set; }
        public int ClientCompanyDetailId { get; set; }
        public int? ClientUserId { get; set; }
        public int? LicenseTypeId { get; set; }
        public bool IsStandAlone { get; set; }
    }
}
