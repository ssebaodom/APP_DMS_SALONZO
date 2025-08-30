using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Report
{
    public class ReportFieldLookupResult : BaseResult
    {
        public List<dynamic> reportFields { get; set; }
        public int TotalCount { get; set; }
    }
}