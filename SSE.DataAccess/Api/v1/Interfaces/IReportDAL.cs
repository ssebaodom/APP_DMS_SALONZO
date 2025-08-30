using SSE.Common.Api.v1.Requests.Report;
using SSE.Common.Api.v1.Results.Report;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IReportDAL
    {
        Task<GetReportListResult> GetReportList(GetReportListRequest getReportList);

        Task<ReportLayoutResult> GetReportLayout(ReportLayoutRequest layoutRequest);

        Task<ReportFieldLookupResult> GetReportFieldLookup(ReportFieldLookupRequest reportFieldLookup);

        Task<ReportExecuteResult> GetReportResult(ReportExecuteRequest executeRequest);
    }
}