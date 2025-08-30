using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Fulfillment
{
    public class ListDetailApprovalResult : CommonResult
    {
        public IEnumerable<ListDetailApprovalDTO> Result { get; set; }
        public int TotalCount { get; set; }
    }
}