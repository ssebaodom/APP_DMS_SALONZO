using Microsoft.AspNetCore.Http;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Responses.Fulfillment;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Entities;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Threading.Tasks;
using static SSE.Common.Api.v1.Responses.Fulfillment.DeliveryPlanResponse;

namespace SSE.Business.Api.v1.Implements
{
    internal class FulfillmentBLL : IFulfillmentBLL
    {
        private readonly IFulfillmentDAL fulfillmentDAL;
        private UserInfoCache userInfoCache;

        public FulfillmentBLL(IFulfillmentDAL fulfillmentDAL,
                       IUserBLLService userBLLService,
                       IHttpContextAccessor httpContextAccessor)
        {
            this.fulfillmentDAL = fulfillmentDAL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }
        public async Task<FulfillmentResponse> GetListFulfillment(FulfillmentRequest request)
        {
            FulfillmentResponse re = new FulfillmentResponse();

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.fulfillmentDAL.GetListFulfillment(request);
            if (result.IsSucceeded == true)
            {
                re.data = result.data;
                re.StatusCode = StatusCodes.Status200OK;
                re.Message = result.Message;
                return re;
            }
            else
                return new FulfillmentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<DetailFulfillmentResponse> GetFulfillment(DetailFulfillmentRequest request)
        {
            DetailFulfillmentResponse re = new DetailFulfillmentResponse();

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId; 

            var result = await this.fulfillmentDAL.GetDetailFulfillment(request);
            if (result.IsSucceeded == true)
            {
                re.data = result.data;
                re.StatusCode = StatusCodes.Status200OK;
                re.Message = result.Message;
                return re;
            }
            else
                return new DetailFulfillmentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<ConfirmFulfillmentResponse> CofirmFulfillment(ConfirmFulfillmentRequest request)
        {
            ConfirmFulfillmentResponse re = new ConfirmFulfillmentResponse();

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.fulfillmentDAL.CofirmFulfillment(request);
            if (result.IsSucceeded == true)
            {
                re.data = result.data;
                re.StatusCode = StatusCodes.Status200OK;
                re.Message = result.data;
                return re;
            }
            else
                return new ConfirmFulfillmentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<ConfirmFulfillmentResponse> Test(ConfirmFulfillmentRequest request)
        {
            ConfirmFulfillmentResponse re = new ConfirmFulfillmentResponse();

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.fulfillmentDAL.CofirmFulfillment(request);
            if (result.IsSucceeded == true)
            {
                re.data = result.data;
                re.StatusCode = StatusCodes.Status200OK;
                re.Message = result.data;
                return re;
            }
            else
                return new ConfirmFulfillmentResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<DynamicResponse> GetAuthorizeList(GetAuthorizeListRequest request)
        {
            request.UserID = userInfoCache.UserId;
            request.Unit = userInfoCache.UnitId;
            var result = await this.fulfillmentDAL.GetAuthorizeList(request);

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
        public async Task<DynamicResponse> GetAuthorizeTypeList(GetAuthorizeTypeListRequest request)
        {
            request.UserID = userInfoCache.UserId;
            request.Unit = userInfoCache.UnitId;
            var result = await this.fulfillmentDAL.GetAuthorizeTypeList(request);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
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
        public async Task<DynamicResponse> GetAuthorizeStatusList(GetAuthorizeStatusListRequest request)
        {
            var result = await this.fulfillmentDAL.GetAuthorizeStatusList(request);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
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
        public async Task<AuthorizeResponse> Authorize(AuthorizeRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.fulfillmentDAL.Authorize(request);

            if (result.IsSucceeded == true)
                return new AuthorizeResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new AuthorizeResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        /// <summary>
        /// Kế hoạch giao hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DynamicResponse> ListDeliveryPlan(ListDeliveryPlanRequest request)
        {
            request.UserID = userInfoCache.UserId;
            request.UnitID = userInfoCache.UnitId;


            var result = await this.fulfillmentDAL.ListDeliveryPlan(request);

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

        public async Task<DynamicResponse> DetailDeliveryPlan(DetailDeliveryPlanRequest request)
        {
            var result = await this.fulfillmentDAL.DetailDeliveryPlan(request);

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

        public async Task<UpdateDeliveryPlanResponse> UpdateDeliveryPlan(DeliveryPlanRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.fulfillmentDAL.UpdateDeliveryPlan(request);

            if (result.IsSucceeded == true)
                return new UpdateDeliveryPlanResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new UpdateDeliveryPlanResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<CreateDeliveryPlanResponse> CreateDeliveryPlan(DeliveryPlanRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.fulfillmentDAL.CreateDeliveryPlan(request);

            if (result.IsSucceeded == true)
                return new CreateDeliveryPlanResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CreateDeliveryPlanResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
    }
}
