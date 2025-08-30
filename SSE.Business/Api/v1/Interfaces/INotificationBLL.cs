using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Notification;
using System;
using System.Threading.Tasks;
using Transshipment.Common.Api.v1.Requests.Notification;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface INotificationBLL
    {
        Task<ApiObjectResponse<bool>> SendNotification(SendNotificationRequest request);
    }
}