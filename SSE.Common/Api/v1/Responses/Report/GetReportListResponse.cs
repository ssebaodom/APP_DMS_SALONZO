using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Report
{
    public class GetReportListResponse : BaseResponse
    {
        public IEnumerable<ReportGroupDTO> Data { get; set; }
    }
}