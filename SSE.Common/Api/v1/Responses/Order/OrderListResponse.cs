using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Order
{
    public class OrderListResponse : CommonResponse
    {
        public dynamic Values { get; set; }
        public int PageIndex { get; set; }
    }
    public class OrderCountResponse : CommonResponse
    {
        public int Data { get; set; }
    }
    public class OrderDetailResponse : CommonResponse
    {
        public OrderDetailResponseData Data { get; set; }
    }
    public class OrderDetailResponseData
    {
        public OrderDetailMasterResultDTO master { get; set; }
        public List<OrderDetailDeResultDTO> line_items { get; set; }
        public OrderDetailPaymentResultDTO infoPayment { get; set; }
    }
    public class OrderCancelResponse : CommonResponse
    {
    }

    public class ListVVHDResponse : CommonResponse
    {
        public List<dynamic> list_vv { get; set; }
        public List<dynamic> list_hd { get; set; }
    }
    
    public class ListStoreAndGroupResponse : CommonResponse
    {
        public List<dynamic> listStore { get; set; }
        public List<dynamic> listGroup { get; set; }
    }
    
    public class ListInfoCardResponse : CommonResponse
    {
        public List<dynamic> master { get; set; }
        public List<dynamic> listItem { get; set; }
        public RuleActionInforCard ruleActionInfoCard { get; set; }
        public FormatProvider formatProvider { get; set; }
    }
    public class GetItemHolderDetailResponse : CommonResponse
    {
        public ItemHolderDetailMasterDTO master { get; set; }
        public List<ItemHolderDTO> listItem { get; set; }
       
    } 
    public class ItemFromBarCodeResponse : CommonResponse
    {
        public List<ItemFromBarCodeDTO> listItem { get; set; }
    }
    public class ListStatusResponse : CommonResponse
    {
        public List<StatusDTO> Data { get; set; }
    } 
    public class GetDynamicListResponse : CommonResponse
    {
        public List<dynamic> listVoucher { get; set; }
        public List<dynamic> listStatus { get; set; }
    }
}