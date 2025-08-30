using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Notification;
using SSE.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SSE.Common.Constants.v1;

namespace Transshipment.Business.Api.v1.Implements
{
    public class NotificationBLL : INotificationBLL
    {
        private readonly INotificationService notificationService;
        private readonly IUserBLL userBLL;
        private readonly IHttpContextAccessor httpContextAccessor;
        private UserInfoCache userInfoCache;

        public NotificationBLL(INotificationService notificationService, IUserBLL userBLL, IUserBLLService userBLLService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.notificationService = notificationService;
            this.userBLL = userBLL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiObjectResponse<bool>> SendNotification(SendNotificationRequest request)
        {
            
            // Chuyển từ List Id tài khoản sang list Token.
            //List<string> Ids = new List<string>();
            //foreach (var id in request.IdTaiKhoans)
            //{
            //    var token = await taiKhoanDAL.TokenByIdTaiKhoan(new Guid(id));
            //    if (token != null && !string.IsNullOrWhiteSpace(token.Data))
            //    {
            //        Ids.Add(token.Data);
            //    }
            //}

            var result = await notificationService.SendNotification(new SendNotificationRequest()
            {
                Type = NOTIFICATION_TYPE.NotificationOnly,
                Title = request.Title,
                Body = request.Body,
                Data = request.Data,
                DriverTokens = request.IdTaiKhoans
            });

            if (result)
            {
                return new ApiObjectResponse<bool>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = true
                };
            }

            return new ApiObjectResponse<bool>()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Data = false
            };
        }
    }
}