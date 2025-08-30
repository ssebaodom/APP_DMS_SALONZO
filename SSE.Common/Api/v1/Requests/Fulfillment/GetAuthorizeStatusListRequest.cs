using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.FulfillmentRequest
{
    public class GetAuthorizeStatusListRequest
    {
        public string loai_duyet { get; set; }
        public int Page_Index { get; set; }
        public int Page_count { get; set; }
    }
}