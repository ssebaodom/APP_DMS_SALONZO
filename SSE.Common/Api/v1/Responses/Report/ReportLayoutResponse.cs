using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Report
{
    public class ReportLayoutResponse : BaseResponse
    {
        public IEnumerable<ReportLayoutDTO> Data { get; set; }
    }
}