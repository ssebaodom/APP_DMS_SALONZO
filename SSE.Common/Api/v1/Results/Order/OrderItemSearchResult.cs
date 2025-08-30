using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Order
{
    public class OrderItemSearchResult : CommonResult
    {
        public IEnumerable<OrderItemInfoDTO> Result { get; set; }
        public IEnumerable<DescriptFieldDTO> DescriptField { get; set; }
        public int TotalCount { get; set; }
    }
}