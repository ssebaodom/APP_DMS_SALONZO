using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Home
{
    public class GetReportDataResponse : BaseResponse
    {
        public dynamic ReportData { get; set; }
        public dynamic ReportInfo { get; set; }
        public IEnumerable<ReportHeaderDescDTO> HeaderDesc { get; set; }
    }

    public class GetListSliderImageResponse : BaseResponse
    {
        public List<dynamic> listSliderImageActive { get; set; }
        public List<dynamic> listSliderImageDisable { get; set; }  
    }
}