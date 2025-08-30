using SSE.Core.Common.Api;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Requests.User
{
    public class PushNotifyRequest : BaseRequest
    {
        public int Type { get; set; }
        public string Tittle { get; set; }
        public string Body { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> DevideToken { get; set; }
        public dynamic Data { get; set; }
    }
}