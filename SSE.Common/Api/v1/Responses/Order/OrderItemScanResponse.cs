using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Order
{
    public class OrderItemScanResponse : CommonResponse
    {
        public OrderItemInfoDTO Data { get; set; }
    }
}