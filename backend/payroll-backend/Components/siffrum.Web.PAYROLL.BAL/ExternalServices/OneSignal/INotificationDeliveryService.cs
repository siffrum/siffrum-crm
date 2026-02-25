namespace Siffrum.Web.Payroll.API.Services.Notifications
{
    public interface INotificationDeliveryService
    {
        Task<bool> SendAsync(string playerId, string title, string message);
    }
}
