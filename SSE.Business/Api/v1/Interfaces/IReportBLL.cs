using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Report;
using SSE.Common.Api.v1.Responses.Report;
using SSE.Common.Api.v1.Results.Report;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IReportBLL
    {
        /// <summary>
        /// Danh sách báo cáo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetReportListResponse> GetReportList(GetReportListRequest request);
        /// <summary>
        /// Màn hình lọc báo cáo
        /// </summary>
        /// <param name="layoutRequest"></param>
        /// <returns></returns>
        Task<ReportLayoutResponse> GetReportLayout(ReportLayoutRequest layoutRequest);
        /// <summary>
        /// Bấm kính lúp màn hình lọc
        /// </summary>
        /// <param name="reportFieldLookup"></param>
        /// <returns></returns>
        Task<ReportFieldLookupResponse> GetReportFieldLookup(ReportFieldLookupRequest reportFieldLookup);
        /// <summary>
        /// Kết quả báo cáo
        /// </summary>
        /// <param name="executeRequest"></param>
        /// <returns></returns>
        Task<ReportExecuteResponse> GetReportResult(ReportExecuteRequest executeRequest);
        CommonResponse ResetCache(string cachekey);
    }
}