using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Order
{
    public class OrderItemScanResult : CommonResult
    {
        public OrderItemInfoDTO Result { get; set; }
    }
}