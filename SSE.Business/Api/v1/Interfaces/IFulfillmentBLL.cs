
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Responses.Fulfillment;
using System.Threading.Tasks;
using static SSE.Common.Api.v1.Responses.Fulfillment.DeliveryPlanResponse;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IFulfillmentBLL
    {
        Task<FulfillmentResponse> GetListFulfillment(FulfillmentRequest fulfillmenRequest);
        Task<DetailFulfillmentResponse> GetFulfillment(DetailFulfillmentRequest fulfillmenRequest);
        Task<ConfirmFulfillmentResponse> CofirmFulfillment(ConfirmFulfillmentRequest fulfillmenRequest);
        Task<ConfirmFulfillmentResponse> Test(ConfirmFulfillmentRequest fulfillmenRequest);
        Task<DynamicResponse> GetAuthorizeList(GetAuthorizeListRequest request);
        Task<DynamicResponse> GetAuthorizeTypeList(GetAuthorizeTypeListRequest request);
        Task<DynamicResponse> GetAuthorizeStatusList(GetAuthorizeStatusListRequest request);
        Task<AuthorizeResponse> Authorize(AuthorizeRequest request);
        /// <summary>
        /// Kế hoạch giao hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DynamicResponse> ListDeliveryPlan(ListDeliveryPlanRequest request);
        Task<DynamicResponse> DetailDeliveryPlan(DetailDeliveryPlanRequest request);
        Task<UpdateDeliveryPlanResponse> UpdateDeliveryPlan(DeliveryPlanRequest request);
        Task<CreateDeliveryPlanResponse> CreateDeliveryPlan(DeliveryPlanRequest request);
    }
}
