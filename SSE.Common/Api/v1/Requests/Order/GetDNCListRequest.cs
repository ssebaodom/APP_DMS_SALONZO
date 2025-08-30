using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class GetDNCListRequest : CommonRequest
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Type { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }
}