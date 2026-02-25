using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siffrum.Web.Payroll.DomainModels.Enums
{
    public enum NotificationTypeDM
    {
        General = 1,
        PayslipGenerated = 2,
        LeaveApproved = 3,
        LeaveRejected = 4,
        PayrollProcessed = 5,
        LicenseExpiryReminder = 6,
        AdminAnnouncement = 7
    }
}
