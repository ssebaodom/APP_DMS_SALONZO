using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using System;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class TimeKeepingHistoryRequest : CommonRequest
    {
        public DateTime dateTime { get; set; }
    }
}