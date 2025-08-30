using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class RefundSaleOutCreateV1Request : CommonRequest
    {
        public RefundSaleOutCreateV1DTO Data { get; set; }
    }
}