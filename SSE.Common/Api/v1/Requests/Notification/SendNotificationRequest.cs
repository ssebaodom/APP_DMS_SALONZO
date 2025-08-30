using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Requests.Notification
{
    public class SendNotificationRequest
    {
        public int Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public JObject Data { get; set; }
        public IEnumerable<string> DriverTokens { get; set; }
        public IEnumerable<string> IdTaiKhoans { get; set; }
    }
}