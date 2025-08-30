using Dapper;
using Newtonsoft.Json.Linq;
using SSE.Common.Api.v1.Requests.Report;
using SSE.Common.Api.v1.Results.Report;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Common.Functions;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class ReportDAL : IReportDAL
    {
        private readonly IDapperService dapperService;
        private readonly ICached cached;
        public ReportDAL(IDapperService dapperService, ICached cached)
        {
            this.dapperService = dapperService;
            this.cached = cached;
        }

        public async Task<GetReportListResult> GetReportList(GetReportListRequest getReportList)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetReportListRequest>(getReportList);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Report_GetList", parameters);

            List<ReportGroupDTO> reportGroup = gridReader.Read<ReportGroupDTO>().ToList();
            List<ReportListDTO> reportList = gridReader.Read<ReportListDTO>().ToList();

            foreach (var element in reportGroup)
            {
                element.ReportList = reportList.Where(x => x.GroupId == element.Id).ToList();
            }

            if (gridReader == null)
            {
                return new GetReportListResult
                {
                    IsSucceeded = false
                };
            }

            return new GetReportListResult
            {
                IsSucceeded = true,
                ReportList = reportGroup
            };
        }

        public async Task<ReportLayoutResult> GetReportLayout(ReportLayoutRequest layoutRequest)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<ReportLayoutRequest>(layoutRequest);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Report_Layout", parameters);

            List<ReportLayoutDTO> reportLayout = gridReader.Read<ReportLayoutDTO>().ToList();
            List<DropDownDTO> dropDowns = gridReader.Read<DropDownDTO>().ToList();

            foreach (var element in reportLayout)
            {
                element.DropDownList = dropDowns.Where(x => x.Field == element.Field).ToList();
            }

            return new ReportLayoutResult
            {
                IsSucceeded = true,
                ReportLayout = reportLayout
            };
        }

        public async Task<ReportFieldLookupResult> GetReportFieldLookup(ReportFieldLookupRequest reportFieldLookup)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<ReportFieldLookupRequest>(reportFieldLookup);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Report_FilterLookup", parameters);

            List<dynamic> reportFieldLookups = gridReader.Read<dynamic>().ToList();
            int CountTotal = gridReader.ReadSingle<int>();

            return new ReportFieldLookupResult
            {
                IsSucceeded = true,
                reportFields = reportFieldLookups,
                TotalCount = CountTotal
            };
        }

        public async Task<ReportExecuteResult> GetReportResult(ReportExecuteRequest executeRequest)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", executeRequest.UserId);
            parameters.Add("@Language", executeRequest.Lang);
            parameters.Add("@Admin", executeRequest.Admin);
            parameters.Add("@Unit", executeRequest.UnitId);

            foreach (var element in executeRequest.Values)
            {
                parameters.Add($"@{element.Field}", element.Value is null ? "" : element.Value);
            }
            List<Report> reports = ListReportCache();
            string ProceName = reports.FirstOrDefault(x => x.ReportId == executeRequest.ReportId).SQLStoreName;

            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);
            List<ReportHeaderDescDTO> reportHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            var data = gridReader.Read<dynamic>().ToList();
           
            return new ReportExecuteResult
            {
                IsSucceeded = true,
                Values = data,
                HeaderDesc = reportHeaders
            };
        }

        public List<Report> ListReportCache()
        {
            string reportListKey = string.Concat(CACHE_KEYS.REPORT_LIST);
            cached.ResetList<Report>(reportListKey);
            // Lấy dữ liệu trong cache.
            List<Report> reportList = cached.GetList<Report>(reportListKey);
            
            // Nếu trong cache chưa có dữ liệu. Đọc trong Db rồi lưu vào cache.
            if (reportList == null || reportList.Count == 0)
            {
#if DEBUG
                reportList = FileReader.LoadFileJson<List<Report>>("/SSE.Common/VariableData", "ReportList.json");
#else
                reportList = FileReader.LoadFileJson<List<Report>>("/VariableData", "ReportList.json");
#endif
                if (reportList is null)
                {
                    return null;
                }
                cached.AddRangeToList(reportListKey, reportList);

                return reportList;
            }

            return reportList;
        }
    }
}