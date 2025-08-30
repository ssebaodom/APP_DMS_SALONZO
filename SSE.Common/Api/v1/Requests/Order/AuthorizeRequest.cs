using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.FulfillmentRequest
{
    public class AuthorizeRequest : CommonRequest
    {
        public string loai_duyet { get; set; }
        public string Action { get; set; }       
        public string stt_rec { get; set; }
        public string Note { get; set; }
    }
}