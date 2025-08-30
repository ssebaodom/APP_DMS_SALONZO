using Microsoft.AspNetCore.Http;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.DisCountRequest;
using SSE.Common.Api.v1.Responses.DisCount;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Entities;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    internal class DisCountBLL : IDisCountBLL
    {
        private readonly IDisCountDAL disCountDAL;
        private UserInfoCache userInfoCache;

        public DisCountBLL(IDisCountDAL disCountDAL,
                       IUserBLLService userBLLService,
                       IHttpContextAccessor httpContextAccessor)
        {
            this.disCountDAL = disCountDAL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }
        public async Task<DisCountResponse> GetDisCount(DisCountRequest request)
        {
            DisCountResponse re = new DisCountResponse();

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.disCountDAL.GetDisCount(request);
            if (result.IsSucceeded == true)
            {
                re.data = result.data;
                re.StatusCode = StatusCodes.Status200OK;
                re.Message = result.Message;
                return re;
            }
            else
                return new DisCountResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<DisCountResponse> GetDisCountWhenUpdate(DisCountWhenUpdateRequest request)
        {
            DisCountResponse re = new DisCountResponse();

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.disCountDAL.GetDisCountWhenUpdate(request);
            if (result.IsSucceeded == true)
            {
                re.data = result.data;
                re.StatusCode = StatusCodes.Status200OK;
                re.Message = result.Message;
                return re;
            }
            else
                return new DisCountResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        /// <summary>
        /// 19/12/2022 
        /// V2 Tổng quát chương trình khuyến mại
        /// </summary>
        /// <creater name="tiennq"></creater>
        /// <returns>list discount</returns>

        public async Task<DisCountApplyResponse> ApplyDiscount(DisCountItemRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.disCountDAL.ApplyDiscount(request);

            if (result.IsSucceeded == true)
            {
                return new DisCountApplyResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    list_ck_tong_don = result.list_ck_tong_don,
                    list_ck = result.list_ck,
                    list_ck_mat_hang = result.list_ck_mat_hang,
                    totalMoneyDiscount = result.totalMoneyDiscount
                };
            }
            else
            {
                return new DisCountApplyResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
    }
}
