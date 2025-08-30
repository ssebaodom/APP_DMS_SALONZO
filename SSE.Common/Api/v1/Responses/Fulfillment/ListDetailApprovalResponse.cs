using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Fulfillment
{
    public class ListDetailApprovalResponse : CommonResponse
    {
        public IEnumerable<ListDetailApprovalDTO> Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}