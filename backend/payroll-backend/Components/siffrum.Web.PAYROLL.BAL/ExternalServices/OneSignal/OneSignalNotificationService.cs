using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;

namespace Siffrum.Web.Payroll.API.Services.Notifications
{
    public class OneSignalNotificationService : INotificationDeliveryService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public OneSignalNotificationService(
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<bool> SendAsync(string playerId, string title, string message)
        {
            var appId = _configuration["OneSignal:AppId"];
            var apiKey = _configuration["OneSignal:ApiKey"];

            var payload = new
            {
                app_id = appId,
                include_player_ids = new[] { playerId },
                headings = new { en = title },
                contents = new { en = message }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://onesignal.com/api/v1/notifications");

            request.Headers.Add("Authorization", $"Basic {apiKey}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
