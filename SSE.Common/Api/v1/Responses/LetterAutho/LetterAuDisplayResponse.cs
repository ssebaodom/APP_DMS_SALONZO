using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.LetterAutho
{
    public class LetterAuDisplayResponse : CommonResponse
    {
        public IEnumerable<LetterAuDisplayDTO> Data { get; set; }
    }
}