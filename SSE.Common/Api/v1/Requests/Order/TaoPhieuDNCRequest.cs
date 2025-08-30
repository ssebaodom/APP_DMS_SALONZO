using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class TaoPhieuDNCRequest : CommonRequest
    {
        public TaoPhieuDNCDTO Data { get; set; }
        public string Encode { get; set; }
        public string Ticket { get; set; }
        public string DeptId { get; set; }
    }
}