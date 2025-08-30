using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;
using static Dapper.SqlMapper;

namespace SSE.Common.Api.v1.Results.User
{
    public class GetSettingOptionsResult : BaseResult
    {
        public dynamic inStockCheck { get; set; }
        public dynamic distanceLocationCheckIn { get; set; }
        public IEnumerable<dynamic> listTransaction { get; set; }
    }

    public class GetSettingOptionsV2Result : BaseResult
    {
        public AppSettingsDTO masterAppSettings { get; set; }
        public IEnumerable<dynamic> listTransaction { get; set; }
        public IEnumerable<dynamic> listAgency { get; set; }
        public IEnumerable<dynamic> listTypePayment { get; set; }
        public IEnumerable<dynamic> listFunctionQrCode { get; set; }
        public IEnumerable<dynamic> listTypeDelivery { get; set; }
        public IEnumerable<dynamic> listTypeVoucher { get; set; }
    }
}