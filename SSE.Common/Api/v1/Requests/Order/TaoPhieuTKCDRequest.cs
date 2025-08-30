using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class TaoPhieuTKCDRequest : CommonRequest
    {
        public TaoPhieuTKCDDTO Data { get; set; }
    }

    public class UpdateTKCDDraftsRequest : CommonRequest
    {
        public TaoPhieuTKCDDTO Data { get; set; }
    }
}