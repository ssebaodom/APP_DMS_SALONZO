using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Responses.Fulfillment;
using System.Threading.Tasks;
using static SSE.Common.Api.v1.Responses.Fulfillment.DeliveryPlanResponse;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/fulfillment")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FulfillmentController : ControllerBase
    {
        private readonly IFulfillmentBLL fulfillmentBLL;

        public FulfillmentController(IFulfillmentBLL fulfillmentBLL)
        {
            this.fulfillmentBLL = fulfillmentBLL;
        }
        [Route("getlist")]
        [HttpPost]
        public async Task<FulfillmentResponse> GetDisCount(FulfillmentRequest request)
        {
            return await this.fulfillmentBLL.GetListFulfillment(request);
        }
        [Route("detail")]
        [HttpPost]
        public async Task<DetailFulfillmentResponse> GetDetail(DetailFulfillmentRequest request)
        {
            return await this.fulfillmentBLL.GetFulfillment(request);
        }
        [Route("cofirm")]
        [HttpPost]
        public async Task<ConfirmFulfillmentResponse>Comfirm(ConfirmFulfillmentRequest request)
        {
            return await this.fulfillmentBLL.CofirmFulfillment(request);
        }
        [Route("test")]
        [HttpPost]
        public async Task<ConfirmFulfillmentResponse> Test(ConfirmFulfillmentRequest request)
        {
            return await this.fulfillmentBLL.Test(request);
        }
        /// <summary>
        /// Duyệt phiếu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("authorize_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetAuthorizeList(GetAuthorizeListRequest request)
        {
            return await this.fulfillmentBLL.GetAuthorizeList(request);
        }
        [Route("authorize_type_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetProductList(GetAuthorizeTypeListRequest request)
        {
            return await this.fulfillmentBLL.GetAuthorizeTypeList(request);
        }
        [Route("authorize_status_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetAuthorizeStatusList(GetAuthorizeStatusListRequest request)
        {
            return await this.fulfillmentBLL.GetAuthorizeStatusList(request);
        }
        [Route("authorize")]
        [HttpPost]
        public async Task<AuthorizeResponse> DNCUpdate(AuthorizeRequest request)
        {
            return await this.fulfillmentBLL.Authorize(request);
        }
        /// <summary>
        /// Kế hoạch giao hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("list_delivery_plan")]
        [HttpPost]
        public async Task<DynamicResponse> ListDeliveryPlan(ListDeliveryPlanRequest request)
        {
            return await this.fulfillmentBLL.ListDeliveryPlan(request);
        }
        [Route("detail_delivery_plan")]
        [HttpPost]
        public async Task<DynamicResponse> DetailDeliveryPlan(DetailDeliveryPlanRequest request)
        {
            return await this.fulfillmentBLL.DetailDeliveryPlan(request);
        }
        [Route("update_delivery_plan")]
        [HttpPost]
        public async Task<UpdateDeliveryPlanResponse> UpdateDeliveryPlan(DeliveryPlanRequest request)
        {
            return await this.fulfillmentBLL.UpdateDeliveryPlan(request);
        }
        [Route("create_delivery_plan")]
        [HttpPost]
        public async Task<CreateDeliveryPlanResponse> CreateDeliveryPlan(DeliveryPlanRequest request)
        {
            return await this.fulfillmentBLL.CreateDeliveryPlan(request);
        }
    }
}
