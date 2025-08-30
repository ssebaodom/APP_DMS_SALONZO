using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SSE.Business.Services.v1.Interfaces;
using SSE.Core.Common.Constants;
using SSE.Common.Constants.v1;
using SSE.Core.Common.Logger;
using SSE.Common.Api.v1.Requests.Notification;

namespace SSE.Business.Services.v1.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration configuration;
        private readonly ILoggerManager loggerManager;

        public NotificationService(IConfiguration configuration, ILoggerManager loggerManager)
        {
            this.configuration = configuration;
            this.loggerManager = loggerManager;
        }   

        public async Task<bool> SendNotification(SendNotificationRequest notificationRequest)
        {
            bool isSuccess = false;

            var fb = configuration.GetSection(CONFIGURATION_KEYS.FIRE_BASE);
            string fireBaseUrl = fb.GetSection(CONFIGURATION_KEYS.FIRE_BASE_URL).Value;
            string fireBaseServerKey = fb.GetSection(CONFIGURATION_KEYS.FIRE_BASE_AUTHORIZATION).Value;

            dynamic notificationInformation;

            if (notificationRequest.Type == NOTIFICATION_TYPE.NotificationAndReloadData || notificationRequest.Type == NOTIFICATION_TYPE.NotificationOnly)
            {
                notificationInformation = new
                {
                    registration_ids = notificationRequest.DriverTokens.ToList(),
                    notification = new
                    {
                        title = notificationRequest.Title,
                        body = notificationRequest.Body,
                        sound = "default",
                        priority = "high",
                        content_available = true,
                        mutable_content = true
                    },
                    data = notificationRequest.Data
                };
            }
            else
            {
                notificationInformation = new
                {
                    registration_ids = notificationRequest.DriverTokens.ToList(),
                    data = notificationRequest.Data
                };
            }

            string jsonMessage = JsonConvert.SerializeObject(notificationInformation);
            var request = new HttpRequestMessage(HttpMethod.Post, fireBaseUrl);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + fireBaseServerKey);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage send_result = null;

            using (var client = new HttpClient())
            {
                try
                {
                    send_result = await client.SendAsync(request);
                    isSuccess = send_result.IsSuccessStatusCode;
                    loggerManager.LogInformation("Send message status: " + send_result.IsSuccessStatusCode + " - Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                catch (System.Exception ex)
                {
                    loggerManager.LogError(ex.Message, ex);
                }
            }

            return isSuccess;
        }
    }
}