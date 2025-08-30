using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Common
{
    public class DynamicResult : BaseResult
    {
        public dynamic Data { get; set; }
        public int TotalPage { get; set; }
    }
}