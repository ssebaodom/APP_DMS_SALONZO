using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Responses.Order;
using SSE.Core.Services.Dapper;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/order")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderBLL orderBLL;

        public OrderController(IOrderBLL orderBLL, IDapperService dapperService)
        {
            this.orderBLL = orderBLL;
        }

        [Route("search-item")]
        [HttpPost]
        public async Task<ListDetailApprovalResponse> SearchItem(OrderItemSearchRequest request)
        {
            return await this.orderBLL.SearchItem(request);
        }
        [Route("search-item-v2")]
        [HttpPost]
        public async Task<ListDetailApprovalResponse> SearchItemV2(OrderItemSearchV2Request request)
        {
            return await this.orderBLL.SearchItemV2(request);
        }
        [Route("scan-item")]
        [HttpGet]
        public async Task<OrderItemScanResponse> ScanItem(string ItemCode, string Currency)
        {
            OrderItemScanRequest request = new OrderItemScanRequest() { ItemCode = ItemCode, Currency = Currency };
            return await this.orderBLL.ScanItem(request);
        }

        [Route("create-order")]
        [HttpPost]
        public async Task<OrderCreateResponse> CreateOrder(OrderCreateRequest request)
        {
            return await this.orderBLL.CreateOrder(request);
        }

        [Route("create-order-v3")]
        [HttpPost]
        public async Task<OrderCreateResponse> CreateOrderV3(OrderCreateV3Request request)
        {
            return await this.orderBLL.CreateOrderV3(request);
        }

        [Route("create-refund-order-v1")]
        [HttpPost]
        public async Task<OrderCreateResponse> RefundCreateOrderV1(RefundOrderCreateV1Request request)
        {
            return await this.orderBLL.RefundCreateOrderV1(request);
        }

        [Route("order-cart")]
        [HttpPost]
        public async Task<OrderCartResponse> OrderCart(OrderCartRequest request)
        {
            return await this.orderBLL.OrderCart(request);
        }

        [Route("item-detail")]
        [HttpGet]
        public async Task<ItemDetailResponse> ItemDetail(string ItemCode, string Currency)
        {
            return await this.orderBLL.ItemDetail(ItemCode, Currency);
        }

        [Route("item-main-group")]
        [HttpGet]
        public async Task<ItemGroupResponse> ItemMainGroup()
        {
            return await this.orderBLL.ItemGroup(0, 0);
        }

        [Route("item-group")]
        [HttpGet]
        public async Task<ItemGroupResponse> ItemGroup(int GroupType, int Level)
        {
            return await this.orderBLL.ItemGroup(GroupType, Level);
        }
        [Route("banner-advertise")]
        [HttpGet]
        public async Task<BannerAdResponse> BannerAdd()
        {
            return await this.orderBLL.BannerAdd();
        }
        [Route("order-count")]
        [HttpPost]
        public async Task<OrderCountResponse> OrderCount()
        {
            return await this.orderBLL.OrderCount();
        }
        [Route("order-list")]
        [HttpPost]
        public async Task<OrderListResponse> OrderList(OrderListRequest request)
        {
            return await this.orderBLL.OrderList(request);
        }
        
        [Route("get-history-order")]
        [HttpPost]
        public async Task<OrderListResponse> GetHistoryOrder(OrderListRequest request)
        {
            return await this.orderBLL.GetHistoryOrder(request);
        }

        [Route("order-detail")]
        [HttpPost]
        public async Task<OrderDetailResponse> OrderDetail(string stt_rec)
        {
            return await this.orderBLL.OrderDetail(stt_rec);
        }
        [Route("update-order-v2")]
        [HttpPost]
        public async Task<OrderUpdateResponse> CreateUpdate(OrderUpdateRequest request)
        {
            return await this.orderBLL.CreateUpdate(request);
        }
        [Route("order-cancel")]
        [HttpPost]
        public async Task<OrderCancelResponse> OrderCancel(string stt_rec)
        {
            return await this.orderBLL.OrderCancel(stt_rec);
        }
        [Route("find-product-all-store")]
        [HttpGet]
        public async Task<DynamicResponse> FindProductAllStore(string codeProduct)
        {
            return await this.orderBLL.FindProductAllStore(codeProduct);
        }
        
        [Route("get-list-store-and-group")]
        [HttpGet]
        public async Task<ListStoreAndGroupResponse> GetListStoreAndGroup(string codeProduct, string listKeyGroup, int checkGroup, int checkStock, int checkStockEmployee)
        {
            return await this.orderBLL.GetListStoreAndGroup(codeProduct, listKeyGroup, checkGroup, checkStock, checkStockEmployee);
        }

        [Route("get-list-tax")]
        [HttpGet]
        public async Task<DynamicResponse> GetListTax()
        {
            return await this.orderBLL.GetListTax();
        }
        /// <summary>
        /// Update Phan cong doan sx
        /// tiennq
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("product_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetProductList(GetProductListRequest request)
        {
            return await this.orderBLL.GetProductList(request);
        }
        [Route("produce_order_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetProduceOrderList(GetProduceOrderListRequest request)
        {
            return await this.orderBLL.GetProduceOrderList(request);
        }
        [Route("command_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetCommandList(GetCommandListRequest request)
        {
            return await this.orderBLL.GetCommandList(request);
        }
        [Route("command_detail")]
        [HttpGet]
        public async Task<DynamicResponse> GetCommandDetail(string stt_rec,string ma_kh)
        {
            return await this.orderBLL.GetCommandDetail(stt_rec,ma_kh);
        }
        [Route("TaoPhieuTKCD")]
        [HttpPost]
        public async Task<OrderCreateResponse> TaoPhieuTKCD(TaoPhieuTKCDRequest request)
        {
            return await this.orderBLL.TaoPhieuTKCD(request);
        }
        [Route("UpdateTKCDDrafts")]
        [HttpPost]
        public async Task<OrderCreateResponse> UpdateTKCDDrafts(UpdateTKCDDraftsRequest request)
        {
            return await this.orderBLL.UpdateTKCDDrafts(request);
        }
        [Route("TaoPhieuDNC")]
        [HttpPost]
        public async Task<DNCCreateResponse> TaoPhieuDNC(TaoPhieuDNCRequest request)
        {
            return await this.orderBLL.TaoPhieuDNC(request);
        }
        [Route("DNC_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetDNCList(GetDNCListRequest request)
        {
            return await this.orderBLL.GetDNCList(request);
        }
        [Route("DNC_list_history")]
        [HttpPost]
        public async Task<DynamicResponse> GetDNCListHistory(GetDNCListHistoryRequest request)
        {
            return await this.orderBLL.GetListDNCHistory(request);
        }
        [Route("DNC-detail")]
        [HttpPost]
        public async Task<DNCDetailResponse> GetDNCDetail(string stt_rec)
        {
            return await this.orderBLL.DNCDetail(stt_rec);
        }
        [Route("DNC-update")]
        [HttpPost]
        public async Task<DNCUpdateResponse> DNCUpdate(UpdateDNCRequest request)
        {
            return await this.orderBLL.UpdateDNC(request);
        }
        [Route("DNC-authorize")]
        [HttpPost]
        public async Task<DNCAuthorizeResponse> DNCUpdate(DNCAuthorizeRequest request)
        {
            return await this.orderBLL.AuthorizeDNC(request);
        }
        [Route("site_list")]
        [HttpPost]
        public async Task<DynamicResponse> GetSiteList(GetSiteListRequest request)
        {
            return await this.orderBLL.GetSiteList(request);
        }
        [Route("list-vvhd")]
        [HttpGet]
        public async Task<ListVVHDResponse> GetListVVHD(string idCustomer)
        {
            return await this.orderBLL.GetListVVHD(idCustomer);
        }

        [Route("list-order-completed")]
        [HttpGet]
        public async Task<DynamicResponse> GetListOrderCompleted(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            return await this.orderBLL.GetListOrderCompleted(dateForm, dateTo, idCustomer, page_index, page_count);
        }

        [Route("detail-order-completed")]
        [HttpGet]
        public async Task<DynamicResponse> DetailOrderCompleted(string sct, string invoiceDate)
        {
            return await this.orderBLL.DetailOrderCompleted(sct, invoiceDate);
        }

        [Route("list-history-refund-order")]
        [HttpGet]
        public async Task<DynamicResponse> GetListRefundOrder(string status, string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            return await this.orderBLL.GetListRefundOrder(status,dateForm, dateTo, idCustomer, page_index, page_count);
        }

        [Route("detail-history-refund-order")]
        [HttpGet]
        public async Task<DynamicResponse> DetailRefundOrder(string sct, string invoiceDate)
        {
            return await this.orderBLL.DetailRefundOrder(sct, invoiceDate);
        }
        [Route("list-status-order")]
        [HttpGet]
        public async Task<DynamicResponse> GetListStatusOrder(string vcCode)
        {
            return await this.orderBLL.GetListStatusOrder(vcCode);
        }
        [Route("list-approve-order")]
        [HttpGet]
        public async Task<DynamicResponse> GetListApproveOrder(string dateForm, string dateTo, int page_index, int page_count)
        {
            return await this.orderBLL.GetListApproveOrder(dateForm, dateTo, page_index, page_count);
        }
        [Route("approve-order")]
        [HttpPost]
        public async Task<DynamicResponse> ApproveOrder(string stcRec)
        {
            return await this.orderBLL.ApproveOrder(stcRec);
        }
        [Route("get-list-info-card")]
        [HttpGet]
        public async Task<ListInfoCardResponse> GetListInfoCard(string stt_rec, string key)
        {
            return await this.orderBLL.GetListInfoCard(stt_rec, key);
        }
        [Route("update-quantity-warehouse-delivery")]
        [HttpPost]
        public async Task<CommonsResponse> UpdateQuantityWarehouseDelivery(UpdateQuantityWarehouseDeliveryRequest request)
        {
            return await this.orderBLL.UpdateQuantityWarehouseDelivery(request);
        }
        [Route("create-delivery-card")]
        [HttpPost]
        public async Task<CommonsResponse> CreateDeliveryCard(CreateDeliveryRequest request)
        {
            return await this.orderBLL.CreateDeliveryCard(request);
        }
        [Route("update-item-barcode")]
        [HttpPost]
        public async Task<CommonsResponse> UpdateItemBarcode(UpdateItemBarcodeRequest request)
        {
            return await this.orderBLL.UpdateItemBarcode(request);
        }
        [Route("update-post-pnf")]
        [HttpPost]
        public async Task<CommonsResponse> UpdatePostPNF(UpdateQuantityPostPNFRequest request)
        {
            return await this.orderBLL.UpdatePostPNF(request);
        }
        [Route("create-refund-barcode-his")]
        [HttpPost]
        public async Task<CommonsResponse> CreateRefundBarcodeHistory(UpdateQuantityPostPNFRequest request)
        {
            return await this.orderBLL.CreateRefundBarcodeHistory(request);
        }
        [Route("get-info-item-for-barcode")]
        [HttpGet]
        public async Task<ItemFromBarCodeResponse> GetInforItemForBarCode(string barcode, string palet)
        {
            return await this.orderBLL.GetInforItemForBarCode(barcode,palet);
        }
        [Route("get-item-holer-detail")]
        [HttpGet]
        public async Task<GetItemHolderDetailResponse> GetItemHolderDetail(string stt_rec)
        {
            return await this.orderBLL.GetItemHolderDetail(stt_rec);
        }
        [Route("item-location-modify")]
        [HttpPost]
        public async Task<CommonsResponse> ItemLocaionModify(ItemLocaionModifyRequest request)
        {
            return await this.orderBLL.ItemLocaionModify(request);
        }
        [Route("create-item-holder")]
        [HttpPost]
        public async Task<CommonsResponse> CreateItemHolder(CreateItemHolderRequest request)
        {
            return await this.orderBLL.CreateItemHolder(request);
        }
        /// <summary>
        /// Xác nhận phiếu xuất điều chuyển
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("stock-transfer-confirm")]
        [HttpPost]
        public async Task<CommonsResponse> StockTransferConfirm(UpdateItemBarcodeRequest request)
        {
            return await this.orderBLL.StockTransferConfirm(request);
        }
        [Route("delete-item-holder")]
        [HttpPost]
        public async Task<CommonsResponse> DeleteItemHolder(string stt_rec)
        {
            return await this.orderBLL.DeleteItemHolder(stt_rec);
        }
        [Route("get-dynamic-list")]
        [HttpGet]
        public async Task<GetDynamicListResponse> GetDynamicList(string voucher_code,string status, string dateFrom, string dateTo)
        {
            return await this.orderBLL.GetDynamicList(voucher_code, status, dateFrom,  dateTo);
        }
        [Route("get-history-dnnk")]
        [HttpGet]
        public async Task<DynamicResponse> GetHistoryDNNK(string stt_rec)
        {
            return await this.orderBLL.GetHistoryDNNK(stt_rec);
        }
        [Route("get-barcode-value")]
        [HttpGet]
        public async Task<DynamicResponse> GetBarcodeValue(string barcode)
        {
            return await this.orderBLL.GetBarcodeValue(barcode);
        }
    }
   
}