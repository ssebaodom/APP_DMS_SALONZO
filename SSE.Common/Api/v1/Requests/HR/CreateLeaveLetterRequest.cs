using SSE.Core.Common.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.Api.v1.Requests.HR
{
    public class CreateLeaveLetterRequest : BaseRequest
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
        public string leaveType { get; set; }
        public string description { get; set; }
        public long userId { get; set; }
        public string unitId { get; set; }
        public string maCong { get; set; }
        public decimal date { get; set; }
    }
}
