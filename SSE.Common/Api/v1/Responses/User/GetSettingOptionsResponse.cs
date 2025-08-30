using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.User
{
    public class GetSettingOptionsResponse : BaseResponse
    {
        public dynamic inStockCheck { get; set; }
        public dynamic distanceLocationCheckIn { get; set; }
        public dynamic listTransaction { get; set; }
    }

    public class GetSettingOptionsV2Response : BaseResponse
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