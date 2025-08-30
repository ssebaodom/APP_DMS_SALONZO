using Dapper;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Home;
using SSE.Common.Api.v1.Results.Home;
using SSE.Common.DTO.v1;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class HomeDAL : IHomeDAL
    {
        private readonly IDapperService dapperService;

        public HomeDAL(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }

        public async Task<GetFilterTimeResult> GetFilterTime(string lang = "v")
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@lang", lang);

            IEnumerable<FilterTimeDTO> filterTimes = await dapperService.QueryAsync<FilterTimeDTO>("app_get_times", dynamicParameter);

            if (filterTimes == null || filterTimes.AsList<FilterTimeDTO>().Count == 0)
            {
                return new GetFilterTimeResult
                {
                    IsSucceeded = false
                };
            }

            return new GetFilterTimeResult
            {
                IsSucceeded = true,
                FilterTimes = filterTimes
            };
        }

        public async Task<GetReportCategoriesResult> GetReportCategories(long userId, string lang = "v")
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@user_id", userId);
            dynamicParameter.Add("@lang", lang);

            IEnumerable<ReportCategoryDTO> reports = await dapperService.QueryAsync<ReportCategoryDTO>("app_get_reports", dynamicParameter);

            if (reports == null || reports.AsList<ReportCategoryDTO>().Count == 0)
            {
                return new GetReportCategoriesResult
                {
                    IsSucceeded = false
                };
            }

            return new GetReportCategoriesResult
            {
                IsSucceeded = true,
                ReportCategories = reports
            };
        }

        public async Task<GetReportDataResult> GetReportData(RepostDataRequest request)
        {
            //DynamicParameterMap dynamicParameterMap = new DynamicParameterMap();
            //DynamicParameters dynamicParameter = dynamicParameterMap.MapUnderScore<Discount>(request);

            DynamicParameters parameters = dapperService.CreateDynamicParameters<RepostDataRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_report_data", parameters);

            if (gridReader == null)
            {
                return new GetReportDataResult
                {
                    IsSucceeded = false
                };
            }

            var data = gridReader.Read<dynamic>().ToList();
            var info = gridReader.Read<dynamic>().FirstOrDefault();
            IEnumerable<ReportHeaderDescDTO> header = null;

            if (info.dataType == "G")
            {
                header = gridReader.Read<ReportHeaderDescDTO>();
            }

            return new GetReportDataResult
            {
                IsSucceeded = true,
                ReportData = data,
                ReportInfo = info,
                HeaderDesc = header
            };
        }

        public async Task<SettingValuesResult> GetSettingValues(string unitId,long userId, int Role, string lang = "v")
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@UserId", userId);
            dynamicParameter.Add("@Lang", lang);
            dynamicParameter.Add("@Admin", Role);
            dynamicParameter.Add("@UnitId ", unitId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_home_SettingValues", dynamicParameter);

            IEnumerable<CurrenciesDTO> currencies = gridReader.Read<CurrenciesDTO>().ToList();
            IEnumerable<StocksDTO> stocks = gridReader.Read<StocksDTO>().ToList();
            bool IsCallServerCart = gridReader.Read<bool>().First();
            dynamic numberFormat = gridReader.Read<dynamic>().ToList();
            if (currencies == null || currencies.AsList<CurrenciesDTO>().Count == 0)
            {
                return new SettingValuesResult
                {
                    IsSucceeded = false
                };
            }

            return new SettingValuesResult
            {
                IsSucceeded = true,
                CurrencyList = currencies,
                StockList = stocks,
                IsCallServerCart = IsCallServerCart,
                NumberFormat = numberFormat
            };
        }
        public async Task<GetListSliderImageResult> GetSliderImages(long UserId, string UnitId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@UserId", UserId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_slider_images", parameters);
            List<dynamic> listSliderImageActive = gridReader.Read<dynamic>().ToList();
            List<dynamic> listSliderImageDisable = gridReader.Read<dynamic>().ToList();
            if (gridReader == null)
            {
                return new GetListSliderImageResult
                {
                    IsSucceeded = false
                };
            }

            return new GetListSliderImageResult
            {
                IsSucceeded = true,
                listSliderImageActive = listSliderImageActive,
                listSliderImageDisable = listSliderImageDisable
            };
        }
    }
}