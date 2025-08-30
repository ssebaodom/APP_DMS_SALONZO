using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.LetterAutho
{
    public class LetterDetailResquest : CommonRequest
    {
        public string LetterId { get; set; }
    }
}