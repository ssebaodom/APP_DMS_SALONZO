using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class DNCAuthorizeRequest : CommonRequest
    {
        public string Action { get; set; }
        public string Cap_duyet { get; set; }
        public string stt_rec { get; set; }
    }
}