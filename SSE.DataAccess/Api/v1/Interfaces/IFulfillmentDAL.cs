using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Results.Fulfillment;
using System.Threading.Tasks;
using SSE.Common.Api.v1.Common;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IFulfillmentDAL
    {
        Task<FulfillmentResults> GetListFulfillment(FulfillmentRequest req);
        Task<DetailFulfillmentResults> GetDetailFulfillment(DetailFulfillmentRequest req);

        Task<ConfirmFulfillmentResults> CofirmFulfillment(ConfirmFulfillmentRequest req);
        Task<ConfirmFulfillmentResults> Test(ConfirmFulfillmentRequest req);
        Task<DynamicResult> GetAuthorizeList(GetAuthorizeListRequest request);
        Task<DynamicResult> GetAuthorizeTypeList(GetAuthorizeTypeListRequest request);
        Task<DynamicResult> GetAuthorizeStatusList(GetAuthorizeStatusListRequest request);
        Task<AuthorizeResult> Authorize(AuthorizeRequest request);
        /// <summary>
        /// Kế hoạch giao hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DynamicResult> ListDeliveryPlan(ListDeliveryPlanRequest request);
        Task<DynamicResult> DetailDeliveryPlan(DetailDeliveryPlanRequest request);
        Task<DynamicResult> UpdateDeliveryPlan(DeliveryPlanRequest request);
        Task<DynamicResult> CreateDeliveryPlan(DeliveryPlanRequest request);
    }
}
