using SSE.Core.Common.Api;

namespace SSE.Common.Api.v1.Requests.User
{
    public class SigninRequest : BaseRequest
    {
        public string HostId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DevideToken { get; set; }
        public string Language { get; set; }
        public string UID { get; set; }
    }
}