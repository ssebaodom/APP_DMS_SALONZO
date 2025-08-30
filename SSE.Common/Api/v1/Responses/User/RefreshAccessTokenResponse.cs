using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Responses.User
{
    public class RefreshAccessTokenResponse : BaseResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}