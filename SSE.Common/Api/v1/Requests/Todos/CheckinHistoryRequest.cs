using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using System;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class CheckinHistoryRequest : CommonRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int PageIndex { get; set; }
    }
}