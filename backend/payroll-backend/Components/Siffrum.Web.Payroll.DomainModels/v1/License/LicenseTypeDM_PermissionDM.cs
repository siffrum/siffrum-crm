using Siffrum.Web.Payroll.DomainModels.v1.Client;

namespace Siffrum.Web.Payroll.DomainModels.v1.License
{
    public class LicenseTypeDM_PermissionDM
    {
        public int? LicenseTypeId { get; set; }
        public virtual LicenseTypeDM LicenseType { get; set; }

        public int? PermissionId { get; set; }
        public virtual PermissionDM Permission { get; set; }
    }
}
