using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.FulfillmentRequest
{
    public class GetAuthorizeListRequest
    {
        public string loai_duyet { get; set; }
        public string status { get; set; }
        public string option { get; set; }
        public long UserID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int Page_Index { get; set; }
        public int Page_count { get; set; }
        public string Unit { get; set; }
    }
}