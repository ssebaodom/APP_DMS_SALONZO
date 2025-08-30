using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.LetterAutho
{
    public class LetterAuDisplayResult : CommonResult
    {
        public IEnumerable<LetterAuDisplayDTO> Data { get; set; }
    }
}