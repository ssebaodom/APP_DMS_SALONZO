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
using SSE.Common.Api.v1.Requests.Manufacturing;
using SSE.Common.Api.v1.Requests.Notification;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Api.v1.Responses.Order;
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
    public class ManufacturingBLL : IManufacturingBLL
    {
        private readonly IManufacturingDAL manufacturingDAL;
        //private readonly IFileService fileService;
        private UserInfoCache userInfoCache;
        private readonly IManufacturingBLL manufacturingBLL;
        private readonly IConfiguration configuration;
        private readonly INotificationService notificationService;

        public ManufacturingBLL(IManufacturingDAL manufacturingDAL,
                      IUserBLLService userBLLService,
                      IHttpContextAccessor httpContextAccessor)
        {
            this.manufacturingDAL = manufacturingDAL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }

        public async Task<DynamicResponse> RequestSectionItem(string request, string route, int page_index, int page_count)
        {

            var result = await this.manufacturingDAL.RequestSectionItem(userInfoCache.UserId, userInfoCache.UnitId, request, route, page_index, page_count);

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
        public async Task<DynamicResponse> GetVoucherTransaction(string vCCode)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.manufacturingDAL.GetVoucherTransaction(userInfoCache.UserId, userInfoCache.UnitId, vCCode);

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
        public async Task<DynamicResponse> GetItemMaterials(string item)
        {

            var result = await this.manufacturingDAL.GetItemMaterials(userInfoCache.UserId, userInfoCache.UnitId, item);

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
        public async Task<DynamicResponse> GetSemiProducts(string lsx, string section, string searchValue, int page_index, int page_count)
        {
            var result = await this.manufacturingDAL.GetSemiProducts(userInfoCache.UserId, userInfoCache.UnitId, lsx, section, searchValue, page_index, page_count);

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
        public async Task<CommonsResponse> CreateFactoryTransactionVoucherModify(ManufacturingRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.manufacturingDAL.CreateFactoryTransactionVoucherModify(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
    }
}
