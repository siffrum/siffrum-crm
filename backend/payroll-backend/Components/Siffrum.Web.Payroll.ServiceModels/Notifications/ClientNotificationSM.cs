using Siffrum.Web.Payroll.ServiceModels.Enums;

namespace Siffrum.Web.Payroll.ServiceModels.v1.Notifications
{
    public class ClientNotificationSM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public NotificationTypeSM NotificationType { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}