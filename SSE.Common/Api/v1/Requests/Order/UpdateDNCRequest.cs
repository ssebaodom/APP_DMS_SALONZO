using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class UpdateDNCRequest : CommonRequest
    {
        public string stt_rec { get; set; }
        public UpdateDNCDTO Data { get; set; }
    }
}