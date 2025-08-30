using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Manufacturing
{
    public class ManufacturingRequest : CommonRequest
    {
        public ManufacturingDTO Data { get; set; }
    }
}