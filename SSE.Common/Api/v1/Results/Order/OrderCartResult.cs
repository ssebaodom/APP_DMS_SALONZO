using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Order
{
    public class OrderCartResult : CommonResult
    {
        public IEnumerable<OrderItemInfoDTO> Items { get; set; }
        public OrderCartTotalDTO Total { get; set; }
    }
}