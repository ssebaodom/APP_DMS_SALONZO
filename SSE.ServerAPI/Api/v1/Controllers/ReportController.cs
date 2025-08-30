using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Report;
using SSE.Common.Api.v1.Responses.Report;
using SSE.Common.Api.v1.Results.Report;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/report")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReportController : ControllerBase
    {
        private readonly IReportBLL reportBLL;

        public ReportController(IReportBLL reportBLL)
        {
            this.reportBLL = reportBLL;
        }

        [Route("report-list")]
        [HttpGet]
        public async Task<GetReportListResponse> GetReportList()
        {
            GetReportListRequest request = new GetReportListRequest();
            return await this.reportBLL.GetReportList(request);
        }

        [Route("report-layout")]
        [HttpGet]
        public async Task<ReportLayoutResponse> GetReportLayout(string ReportId)
        {
            ReportLayoutRequest request = new ReportLayoutRequest() { ReportId = ReportId };
            return await this.reportBLL.GetReportLayout(request);
        }

        [Route("report-field-lookup")]
        [HttpPost]
        public async Task<ReportFieldLookupResponse> GetReportFieldLookup(ReportFieldLookupRequest request)
        {
            return await this.reportBLL.GetReportFieldLookup(request);
        }

        [Route("report-result")]
        [HttpPost]
        public async Task<ReportExecuteResponse> GetReportResult(ReportExecuteRequest request)
        {
            return await this.reportBLL.GetReportResult(request);
        }
        [Route("reset-cache")]
        [HttpPost]
        public CommonResponse ResetCache(string key)
        {
            return this.reportBLL.ResetCache(key);
        }
    }
}