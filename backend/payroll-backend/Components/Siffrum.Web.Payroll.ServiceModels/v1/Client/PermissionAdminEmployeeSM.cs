using System.Collections.Generic;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Client
{
    public class PermissionAdminEmployeeSM
    {
        public List<PermissionSM> permissionAdmin { get; set; }
        public List<PermissionSM> permissionEmployee { get; set; }
    }
}
