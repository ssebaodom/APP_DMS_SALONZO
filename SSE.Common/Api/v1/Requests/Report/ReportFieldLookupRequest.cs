using SSE.Common.Api.v1.Common;
using System.ComponentModel.DataAnnotations;

namespace SSE.Common.Api.v1.Requests.Report
{
    public class ReportFieldLookupRequest : CommonRequest
    {
        [Required]
        public string Controller { get; set; }

        public string FilterValueName { get; set; }
        public string FilterValueCode { get; set; }

        [Required]
        public int PageIndex { get; set; }
    }
}