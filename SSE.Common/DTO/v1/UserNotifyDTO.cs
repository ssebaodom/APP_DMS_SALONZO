using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class UserNotifyDTO
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserIds { get; set; }
        public string Tokens { get; set; }
        public string Data { get; set; }
        public int Status { get; set; }
        public string Time { get; set; }
    }
}