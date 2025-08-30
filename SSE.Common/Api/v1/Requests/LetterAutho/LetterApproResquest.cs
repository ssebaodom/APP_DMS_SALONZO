using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;

namespace SSE.Common.Api.v1.Requests.LetterAutho
{
    public class LetterApproResquest : CommonRequest
    {
        public LetterAuthorityAction ActionType { get; set; }
        public string LetterId { get; set; }
    }
}