using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transshipment.Common.Api.v1.Requests.Notification;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/thongbao")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationBLL notificationBLL;

        public NotificationController(INotificationBLL notificationBLL)
        {
            this.notificationBLL = notificationBLL;
        }

        /// <summary>
        /// Gửi thông báo tới các id người dùng được chỉ định.
        /// Với đầu vào là một thông báo bất kỳ bao gồm: Tiêu đề, nội dung thông báo, data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("send-notification")]
        [HttpPost]
        public async Task<ApiObjectResponse<bool>> SendNotification(SendNotificationRequest request)
        {
            return await this.notificationBLL.SendNotification(request);
        }
    }
}
