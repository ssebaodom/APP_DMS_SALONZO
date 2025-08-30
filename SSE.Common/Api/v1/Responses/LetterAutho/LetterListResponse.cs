using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.LetterAutho
{
    public class LetterListResponse : CommonResponse
    {
        public IEnumerable<ReportHeaderDescDTO> HeaderDesc { get; set; }
        public dynamic Values { get; set; }
        public IEnumerable<LetterStatusDTO> Status { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get { return (int)Math.Ceiling((decimal)TotalCount / 20); } }
    }
}