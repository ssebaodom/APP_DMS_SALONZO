using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.FulfillmentRequest
{
    public class FulfillmentRequest : CommonRequest
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public string CusTomer { get; set; }
        public string Itemcode { get; set; }
    }
    public class DetailFulfillmentRequest : CommonRequest
    {
        public string stt_rec { get; set; }
    }
    public class ConfirmFulfillmentRequest : CommonRequest
    {
        public string desc { get; set; }
        public int TypePayment { get; set; }
        public int status { get; set; }
        public List<LineConfirmFulfillmentRequest> ds_line { get; set; }
    }
    public class LineConfirmFulfillmentRequest
    {
        public string stt_rec;
        public string stt_rec0;
        public decimal so_luong;
    }
    /// <summary>
    /// Kế hoạch giao hàng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public class ListDeliveryPlanRequest
    {
        public long UserID { get; set; }
        public string UnitID { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public int Page_Index { get; set; }
        public int Page_count { get; set; }
    }
    
    public class DetailDeliveryPlanRequest
    {
        public string stt_rec { get; set; }
        public string ngay_giao { get; set; }
        public string ma_kh { get; set; }
        public string ma_vc { get; set; }
        public string nguoi_giao { get; set; }
    }
    
    public class DeliveryPlanRequest : CommonRequest
    {
        public PlanDeliveryDTO Data { get; set; }
    }
}
