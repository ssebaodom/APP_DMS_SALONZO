using Newtonsoft.Json.Linq;
using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Report
{
    public class ReportExecuteResponse : BaseResponse
    {
        public IEnumerable<ReportHeaderDescDTO> HeaderDesc { get; set; }
        public dynamic Values { get; set; }
    }
}