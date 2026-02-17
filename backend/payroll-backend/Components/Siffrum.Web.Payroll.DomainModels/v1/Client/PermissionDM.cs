using Siffrum.Web.Payroll.DomainModels.Base;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.License;
using System.Collections.Generic;

namespace Siffrum.Web.Payroll.DomainModels.v1.Client
{
    public class PermissionDM : SiffrumPayrollDomainModelBase<int>
    {
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool IsEnabledForClient { get; set; }

        public RoleTypeDM RoleType { get; set; }

        [ForeignKey(nameof(CompanyModules))]
        public int? CompanyModulesId { get; set; }
        public virtual CompanyModulesDM CompanyModules { get; set; }

        [ForeignKey(nameof(ClientCompanyDetail))]
        public int? ClientCompanyDetailId { get; set; }
        public virtual ClientCompanyDetailDM ClientCompanyDetail { get; set; }

        [ForeignKey(nameof(ClientUser))]
        public int? ClientUserId { get; set; }
        public virtual ClientUserDM ClientUser { get; set; }

        [ForeignKey(nameof(LicenseType))]
        public int? LicenseTypeId { get; set; }
        public virtual LicenseTypeDM LicenseType { get; set; }

        public virtual ICollection<LicenseTypeDM_PermissionDM> LicenseTypeDM_PermissionDMs { get; set; }

    }
}
