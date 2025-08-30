using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Common
{
    public class DynamicResponse : BaseResponse
    {
        public dynamic Data { get; set; }
        public int TotalPage { get; set; }
    }
}