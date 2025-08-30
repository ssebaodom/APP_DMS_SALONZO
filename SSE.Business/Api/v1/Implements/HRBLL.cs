using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Config;
using SSE.Business.Services.v1.Implements;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.HR;
using SSE.Common.Api.v1.Requests.Notification;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Files;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    public class HRBLL : IHRBLL
    {
        private readonly IHRDAL hrDAL;
        //private readonly IFileService fileService;
        private UserInfoCache userInfoCache;
        private readonly IUserBLL userBLL; 
        private readonly IConfiguration configuration;
        private readonly INotificationService notificationService;

        public HRBLL(INotificationService notificationService, IHRDAL hrDAL, IUserBLL userBLL, IUserBLLService userBLLService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) // , IFileService fileService
        {
            this.hrDAL = hrDAL;
            this.notificationService = notificationService;
            //this.fileService = fileService;
            this.userBLL = userBLL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            this.configuration = configuration;
        }

        //public HRBLL(IHRDAL hrDAL,
        //              IUserBLL userBLL,
        //              IUserBLLService userBLLService,
        //              IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IFileService fileService)
        //{
        //    this.hrDAL = hrDAL;
        //    //this.fileService = fileService;
        //    this.userBLL = userBLL;
        //    userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        //    this.configuration = configuration;
        //}

        public async Task<DynamicResponse> GetListLeaveLetter(string status, string dateFrom, string dateTo, int page_index, int page_count)
        {

            var result = await this.hrDAL.GetListLeaveLetter(status, userInfoCache.UserId, userInfoCache.UnitId, userInfoCache.Lang, dateFrom, dateTo, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    TotalPage = result.TotalPage
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> CreateLeaveLetter(CreateLeaveLetterRequest request)
        {
            request.userId = userInfoCache.UserId;
            request.unitId = userInfoCache.UnitId;

            var result = await this.hrDAL.CreateLeaveLetter(request);

            if (result.IsSucceeded == true)
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        
        public async Task<CommonResponse> CancelLeaveLetter(string sctRec, string stt)
        {

            var result = await this.hrDAL.CancelLeaveLetter(sctRec, stt, userInfoCache.UserId, userInfoCache.UnitId);

            if (result.IsSucceeded == true)
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<ApiObjectResponse<bool>> GetListCustomerBirthday()
        {

            var listUserToken = await this.hrDAL.GetListCustomerBirthday();

            List<string> list = new List<string>();
            list.Add("faGYmO6sSheLsnJJwXhSju:APA91bGA2Jn6AewRZqaN22CXR9GJY8HY7O1vA4LwlL-ydvB0OyygXCxOdDj-biEO1KBCQBsERXhAbEFed6vjjYW5ZIC8CUb55yRFWwrc4CaEBB8nCxof4snLp32qBjmb2xviVWGydeWn");

            if (listUserToken != null && listUserToken.Data != null)
            {
                var data = new JObject();
                data["EVENT"] = NOTIFICATION_EVENT.BIRTH_DAY;
                var resultSendNotification = await notificationService.SendNotification(new SendNotificationRequest()
                {
                    Title = "Chúc mừng sinh nhật",
                    Body = "Gửi bạn 1 tình yêu to bự ❤❤❤",
                    Type = NOTIFICATION_TYPE.NotificationOnly,
                    Data = data,
                    DriverTokens = list
                });;

                return new ApiObjectResponse<bool>()
                {
                    Data = resultSendNotification,
                    StatusCode = StatusCodes.Status200OK,   
                };
            }

            return new ApiObjectResponse<bool>()
            {
                Data = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = listUserToken.Message
            };
        }
    }
}
