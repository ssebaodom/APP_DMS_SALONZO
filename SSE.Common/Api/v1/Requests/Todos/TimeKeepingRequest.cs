using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using System;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class TimeKeepingRequest : CommonRequest
    {
        public DateTime Datetime { get; set; }
        public string LatLong { get; set; }
        public string Location { get; set; }
        public string Descript { get; set; }
        public string Note { get; set; }
        public string UId { get; set; }
        public string QRCode { get; set; }
    }
}