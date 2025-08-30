using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class OrderItemScanRequest : CommonRequest
    {
        public string ItemCode { get; set; }
        public string Currency { get; set; }
    }
}