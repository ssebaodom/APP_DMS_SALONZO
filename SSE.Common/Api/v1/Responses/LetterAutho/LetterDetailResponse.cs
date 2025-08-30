using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Results.LetterAutho;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.LetterAutho
{
    public class LetterDetailResponse : CommonResponse
    {
        public IEnumerable<ReportHeaderDescDTO> MainHeaderDesc { get; set; }
        public dynamic MainValues { get; set; }
        public IEnumerable<ReportHeaderDescDTO> DetailHeaderDesc { get; set; }
        public dynamic DetailValues { get; set; }
    }
    public class LetterDetailResponse2 : CommonResponse
    {
        public string Data { get; set; }
        public List<ValuesFile> listValuesFile { get; set; }
    }
}