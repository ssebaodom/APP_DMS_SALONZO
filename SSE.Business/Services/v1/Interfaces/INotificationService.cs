using System.Threading.Tasks;
using SSE.Common.Api.v1.Requests.Notification;

namespace SSE.Business.Services.v1.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotification(SendNotificationRequest notificationRequest);
    }
}