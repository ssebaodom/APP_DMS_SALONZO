using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Order
{
    public class ItemGroupResponse : CommonResponse
    {
        public IEnumerable<ItemGroupDTO> Data { get; set; }
    }
}