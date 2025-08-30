using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Home
{
    public class GetDefaultDataResponse : BaseResponse
    {
        public IEnumerable<FilterTimeDTO> FilterTimes { get; set; }
        public IEnumerable<ReportCategoryDTO> ReportCategories { get; set; }
        public IEnumerable<CurrenciesDTO> CurrencyList { get; set; }
        public IEnumerable<StocksDTO> StockList { get; set; }
        public bool IsCallServerCart { get; set; }
        public dynamic NumberFormat{ get; set; }
        public dynamic ReportData { get; set; }
        public dynamic ReportInfo { get; set; }
    }
}