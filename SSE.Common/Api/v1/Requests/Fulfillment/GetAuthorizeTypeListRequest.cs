using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.FulfillmentRequest
{
    public class GetAuthorizeTypeListRequest
    {
        public long UserID { get; set; }
        public int Page_Index { get; set; }
        public int Page_count { get; set; }
        public string Unit { get; set; }
    }
}