using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Responses.Order;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IOrderBLL
    {
        Task<ListDetailApprovalResponse> SearchItem(OrderItemSearchRequest request);
        
        Task<ListDetailApprovalResponse> SearchItemV2(OrderItemSearchV2Request request);

        Task<OrderItemScanResponse> ScanItem(OrderItemScanRequest request);

        Task<OrderCreateResponse> CreateOrder(OrderCreateRequest request);

        Task<OrderCreateResponse> CreateOrderV3(OrderCreateV3Request request);
        
        Task<OrderCreateResponse> RefundCreateOrderV1(RefundOrderCreateV1Request request);

        Task<OrderListResponse> OrderList(OrderListRequest request);
        Task<OrderListResponse> GetHistoryOrder(OrderListRequest request);

        Task<OrderCountResponse> OrderCount();

        Task<OrderCartResponse> OrderCart(OrderCartRequest request);

        Task<ItemDetailResponse> ItemDetail(string ItemCode, string Currency);


        Task<ItemGroupResponse> ItemGroup(int GroupType, int Level);
        Task<BannerAdResponse> BannerAdd();
        Task<OrderDetailResponse> OrderDetail(string stt_rec);

        Task<OrderUpdateResponse> CreateUpdate(OrderUpdateRequest request);
        Task<OrderCancelResponse> OrderCancel(string stt_rec);
        Task<DynamicResponse> GetProductList(GetProductListRequest request);
        Task<DynamicResponse> GetProduceOrderList(GetProduceOrderListRequest request);
        Task<DynamicResponse> GetCommandList(GetCommandListRequest request);
        Task<DynamicResponse> GetCommandDetail(string stt_rec, string ma_kh);
        Task<OrderCreateResponse> TaoPhieuTKCD(TaoPhieuTKCDRequest request);
        Task<OrderCreateResponse> UpdateTKCDDrafts(UpdateTKCDDraftsRequest request);
        Task<DNCCreateResponse> TaoPhieuDNC(TaoPhieuDNCRequest request);
        Task<DynamicResponse> GetDNCList(GetDNCListRequest request);
        Task<DNCDetailResponse> DNCDetail(string stt_rec);
        Task<DNCUpdateResponse> UpdateDNC(UpdateDNCRequest request);
        Task<DNCAuthorizeResponse> AuthorizeDNC(DNCAuthorizeRequest request);
        Task<DynamicResponse> GetSiteList(GetSiteListRequest request);

        Task<DynamicResponse> GetListDNCHistory(GetDNCListHistoryRequest request);

        Task<ListVVHDResponse> GetListVVHD(string idCustomer);
        Task<ListStoreAndGroupResponse> GetListStoreAndGroup(string codeProduct, string listKeyGroup, int checkGroup, int checkStock, int checkStockEmployee);
        Task<DynamicResponse> FindProductAllStore(string codeProduct);
        Task<DynamicResponse> GetListTax();
        Task<DynamicResponse> GetListOrderCompleted(string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
        Task<DynamicResponse> GetListRefundOrder(string status,string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
        Task<DynamicResponse> DetailOrderCompleted(string sct, string invoiceDate);
        Task<DynamicResponse> DetailRefundOrder(string sct, string invoiceDate);
        Task<DynamicResponse> GetListStatusOrder(string vcCode);
        Task<DynamicResponse> GetListApproveOrder(string dateForm, string dateTo, int page_index, int page_count);
        Task<ItemFromBarCodeResponse> GetInforItemForBarCode(string barcode, string palet);
        Task<GetItemHolderDetailResponse> GetItemHolderDetail(string stt_rec);
        Task<ListInfoCardResponse> GetListInfoCard(string stt_rec, string key);
        Task<DynamicResponse> ApproveOrder(string sctRec);
        Task<CommonsResponse> UpdateQuantityWarehouseDelivery(UpdateQuantityWarehouseDeliveryRequest request);
        Task<CommonsResponse> CreateDeliveryCard(CreateDeliveryRequest request);
        Task<CommonsResponse> UpdateItemBarcode(UpdateItemBarcodeRequest request);
        Task<CommonsResponse> StockTransferConfirm(UpdateItemBarcodeRequest request);
        Task<CommonsResponse> UpdatePostPNF(UpdateQuantityPostPNFRequest request);
        Task<CommonsResponse> CreateRefundBarcodeHistory(UpdateQuantityPostPNFRequest request);
        Task<CommonsResponse> ItemLocaionModify(ItemLocaionModifyRequest request);
        Task<CommonsResponse> CreateItemHolder(CreateItemHolderRequest request);
        Task<CommonsResponse> DeleteItemHolder(string stt_rec);
        Task<GetDynamicListResponse> GetDynamicList(string voucher_code,string status, string dateFrom, string dateTo);
        Task<DynamicResponse> GetHistoryDNNK(string stt_rec);
        Task<DynamicResponse> GetBarcodeValue(string barcode);
    }
}