using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class TodoUpdateRequest : CommonRequest
    {
        public int Status { get; set; }
        public string Id { get; set; }
        public decimal PercentComplete { get; set; }
        public string Detail { get; set; }
        public string ShareNames { get; set; }

    }
}