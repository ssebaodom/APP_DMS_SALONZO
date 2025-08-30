using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Home
{
    public class SettingValuesResult : BaseResult
    {
        public IEnumerable<CurrenciesDTO> CurrencyList { get; set; }
        public IEnumerable<StocksDTO> StockList { get; set; }
        public bool IsCallServerCart { get; set; }
        public dynamic NumberFormat { get; set; }
    }

    public class GetListSliderImageResult : BaseResult
    {
        public List<dynamic> listSliderImageActive { get; set; }
        public List<dynamic> listSliderImageDisable { get; set; }
    }
}