using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Order
{
    public class DNCListResponse : CommonResponse
    {
        public dynamic Values { get; set; }
        public int PageIndex { get; set; }
    }
    public class DNCCountResponse : CommonResponse
    {
        public int Data { get; set; }
    }
    public class DNCDetailResponse : CommonResponse
    {
        public DNCDetailResponseData Data { get; set; }
    }
    public class DNCDetailResponseData
    {
        public DNCMasterResultDTO master { get; set; }
        public List<DNCDetailResultDTO> detail { get; set; }
    }
    public class DNCCancelResponse : CommonResponse
    {
    }
}