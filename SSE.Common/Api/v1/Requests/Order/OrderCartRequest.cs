using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class OrderCartRequest : CommonRequest
    {
        public string Currency { get; set; }
        public List<OrderItemCartDTO> Items { get; set; }
    }
}