using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class GetCommandListRequest
    {
        public string To { get; set; }
        public string UnitId { get; set; }
        public int Page_Index { get; set; }
        public int Page_count { get; set; }
    }
}