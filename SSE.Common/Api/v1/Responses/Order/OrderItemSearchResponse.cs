using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Order
{
    public class ListDetailApprovalResponse : CommonResponse
    {
        public IEnumerable<OrderItemInfoDTO> Data { get; set; }
        public IEnumerable<DescriptFieldDTO> DescriptField { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}