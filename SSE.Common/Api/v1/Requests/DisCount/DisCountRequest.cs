using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.DisCountRequest
{
    public class DisCountRequest : CommonRequest
    {
        public string ma_kh { get; set; }
        public List<LineOrder> line_item { get; set; }
        public string ma_kho { get; set; }
    }
    public class LineOrder
    {
        public string code { get; set; }
        public decimal count { get; set; }
        public decimal price { get; set; }
    }
    public class DisCountWhenUpdateRequest : CommonRequest
    {
        public string stt_rec { get; set; }
        public string ma_kh { get; set; }
        public List<LineOrder> line_item { get; set; }
        public string ma_kho { get; set; }
    }
    public class DisCountItemRequest : CommonRequest
    {
        public string List_ckvt { get; set; }
        public string List_promo { get; set; }
        public string List_item { get; set; }
        public string List_qty { get; set; }
        public string List_price { get; set; }
        public string List_money { get; set; }
        public string Warehouse_id { get; set; }
        public string Customer_id { get; set; }
        
        public string cLstItemRe { get; set; }
        public string cLstQtyRe { get; set; }
        public string cLstPriceRe { get; set; }
        public string cLstMoneyRe { get; set; }
    }
}
