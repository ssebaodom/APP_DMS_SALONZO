using SSE.Common.Api.v1.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class GetDNCListHistoryRequest
    {
        public long UserID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Status { get; set; }
        public string UnitID { get; set; }
        public int Admin { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
