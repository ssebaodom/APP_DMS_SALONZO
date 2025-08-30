using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class UserNotifySendDTO
    {
        public int Type { get; set; }
        public string Tittle { get; set; }
        public string Body { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> Tokens { get; set; }
        public string Data { get; set; }
    }
}