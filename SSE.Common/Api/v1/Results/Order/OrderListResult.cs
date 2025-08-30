using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Order
{
    public class OrderListResult : CommonResult
    {
        public IEnumerable<ReportHeaderDescDTO> HeaderDesc { get; set; }
        public dynamic Values { get; set; }
        public IEnumerable<LetterStatusDTO> Status { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get { return (int)Math.Ceiling((decimal)TotalCount / 20); } }
    }
    public class OrderDetailResult : CommonResult
    {
        public OrderDetailMasterResultDTO master { get; set; }
        public List<OrderDetailDeResultDTO> listProduct { get; set; }
        public OrderDetailPaymentResultDTO infoPayment { get; set; }
    }
    public class OrderCancelResult : CommonResult
    {
    }

    public class ListVVHDResult : CommonResult
    {
        public List<dynamic> list_vv { get; set; }
        public List<dynamic> list_hd { get; set; }
    }
    
    public class ListStoreAndGroupResult : CommonResult
    {
        public List<dynamic> listStore { get; set; }
        public List<dynamic> listGroup { get; set; }
    }
    public class ListInfoCardResult : CommonResult
    {
        public List<dynamic> master { get; set; }
        public List<dynamic> listItem { get; set; }
        public RuleActionInforCard ruleActionInfoCard { get; set; }
    }
    public class FormatProviderResult : CommonResult
    {
        public FormatProvider formatProvider { get; set; }
    }
    public class ItemHolderDetailResult : CommonResult
    {
        public ItemHolderDetailMasterDTO master { get; set; }
        public List<ItemHolderDTO> listItem { get; set; }
    }
    public class ItemFromBarCodeResult : CommonResult
    {
        public List<ItemFromBarCodeDTO> listItem { get; set; }
    }
    public class StatusResult : CommonResult
    {
        public List<StatusDTO> Data { get; set; }
    }
    public class GetDynamicListResult : CommonResult
    {
        public List<dynamic> listVoucher { get; set; }
        public List<dynamic> listStatus { get; set; }
    }
}