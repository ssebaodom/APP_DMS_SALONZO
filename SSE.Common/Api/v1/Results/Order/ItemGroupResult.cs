using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Order
{
    public class ItemGroupResult : CommonResult
    {
        public IEnumerable<ItemGroupDTO> Data { get; set; }
    }
}