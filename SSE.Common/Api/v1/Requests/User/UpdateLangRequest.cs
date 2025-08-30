using SSE.Core.Common.Api;

namespace SSE.Common.Api.v1.Requests.User
{
    public class UpdateLangRequest : BaseRequest
    {
        public string Lang { get; set; }
    }
}