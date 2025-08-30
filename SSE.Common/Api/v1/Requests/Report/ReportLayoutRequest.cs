using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Report
{
    public class ReportLayoutRequest : CommonRequest
    {
        public string ReportId { get; set; }
    }
}