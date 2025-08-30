using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Requests.Report
{
    public class ReportExecuteRequest : CommonRequest
    {
        public string ReportId { get; set; }
        public List<ReportRequestDTO> Values { get; set; }
    }
}