using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Notifications
{
    public class CreateNotificationRequestSM
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public NotificationTypeSM NotificationType { get; set; }

        public int? ReferenceId { get; set; }
    }
}