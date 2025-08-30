using SSE.Core.Common.Api;

namespace SSE.Common.Api.v1.Requests.Home
{
    public class RepostDataRequest : BaseRequest
    {
        public string report_id { get; set; }
        public string time_id { get; set; }
        public string lang { get; set; }
        public long user_id { get; set; }
        public int role { get; set; }
        public string unit_id { get; set; }
        public string store_id { get; set; }
        public string unit_of_measure { get; set; }
    }
}