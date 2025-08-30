using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Results.Order;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IOrderDAL
    {
        Task<OrderItemSearchResult> SearchItem(OrderItemSearchRequest orderItemSearch);
        Task<OrderItemSearchResult> SearchItemV2(OrderItemSearchV2Request orderItemSearch);
        Task<OrderItemScanResult> ScanItem(OrderItemScanRequest request);
        Task<OrderCreateResult> OrderCreate(OrderCreateRequest request);
        Task<OrderCreateResult> CreateOrderV3(OrderCreateV3Request request);

        Task<OrderCreateResult> RefundCreateOrderV1(RefundOrderCreateV1Request request);

        Task<OrderListResult> OrderList(OrderListRequest request);
        Task<OrderListResult> GetHistoryOrder(OrderListRequest request);
        Task<int> OrderCount(long UserId, string UnitId, string Lang, int Admin);
        Task<OrderCartResult> OrderCart(OrderCartRequest request);
        Task<ItemDetailResult> ItemDetail(string ItemCode, string Currency, long UserId, string UnitId, string Lang, int Admin);
       
        Task<ItemGroupResult> ItemGroup(int GroupType, int Level, long UserId, string UnitId, string Lang, int Admin);
        Task<BannerAdResult> GetBannerAd(long UserId, string UnitId, string Lang, int Admin);
        Task<OrderDetailResult> OrderDetail(long UserId, string UnitId, int Admin, string stt_rec);
        Task<OrderUpdateResult> OrderUpdate(OrderUpdateRequest request);
        Task<OrderCancelResult> OrderCancel(long UserId, string UnitId, int Admin, string stt_rec);
        Task<DynamicResult> GetProductList(GetProductListRequest request);
        Task<DynamicResult> GetProduceOrderList(GetProduceOrderListRequest request);
        Task<DynamicResult> GetCommandList(GetCommandListRequest request);
        Task<DynamicResult> GetCommandDetail(string stt_rec, string ma_kh);
        Task<OrderCreateResult> TaoPhieuTKCD(TaoPhieuTKCDRequest request);
        Task<OrderCreateResult> UpdateTKCDDraftsTKCD(UpdateTKCDDraftsRequest request);
        Task<DNCCreateResult> TaoPhieuDNC(TaoPhieuDNCRequest request);
        Task<DynamicResult> GetDNCList(GetDNCListRequest request);
        Task<DNCResult> GetDNCDetail(string stt_rec);
        Task<DNCUpdateResult> UpdateDNC(UpdateDNCRequest request);
        Task<DNCAuthorizeResult> AuthorizeDNC(DNCAuthorizeRequest request);
        Task<DynamicResult> GetSiteList(GetSiteListRequest request);
        Task<ListVVHDResult> GetListVVHD(string idCustomer,long UserId, string UnitId, string Lang, int Admin);

        Task<DynamicResult> GetListDNCHistory(GetDNCListHistoryRequest request);
        Task<DynamicResult> FindProductAllStore(string codeProduct, long UserId, string UnitId, string Lang, int Admin);
        Task<ListStoreAndGroupResult> GetListStoreAndGroup(string codeProduct, long UserId, string UnitId, string Lang, int Admin, string listKeyGroup, int checkGroup, int checkStock, int checkStockEmployee);
        Task<DynamicResult> GetListTax(long UserId, string UnitId, string Lang, int Admin);

        Task<DynamicResult> GetListOrderCompleted(long UserId, string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
        Task<DynamicResult> GetListRefundOrder(string status,long UserId, string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
        Task<DynamicResult> DetailOrderCompleted(long UserId, string unitID, string sct, string InvoiceDate);
        Task<DynamicResult> DetailRefundOrder(long UserId, string unitID, string sct, string InvoiceDate);
        Task<DynamicResult> GetListStatusOrder(long UserId, string vcCode);
        Task<DynamicResult> GetListApproveOrder(long UserId, string unitID, string dateForm, string dateTo, int page_index, int page_count);
        Task<ItemFromBarCodeResult> GetInforItemForBarCode(long UserId, string unitID, string barcode, string palet);
        Task<ItemHolderDetailResult> GetItemHolderDetail(long UserId, string unitID, string stt_rec);
        Task<ListInfoCardResult> GetListInfoCard(long UserId, string unitID, string stt_rec, string key);
        Task<DynamicResult> DeleteItemHolder(long UserId, string unitID, string stt_rec);
        Task<FormatProviderResult> GetFormatProvider(long UserId, string unitID, string stt_rec);
        Task<DynamicResult> ApproveOrder(long UserId, string unitID, string sctRec);
        Task<CommonsResult> UpdateQuantityWarehouseDelivery(UpdateQuantityWarehouseDeliveryRequest request);
        Task<CommonsResult> CreateDeliveryCard(CreateDeliveryRequest request);
        Task<CommonsResult> UpdateItemBarcode(UpdateItemBarcodeRequest request);
        Task<CommonsResult> StockTransferConfirm(UpdateItemBarcodeRequest request);
        Task<CommonsResult> UpdatePostPNF(UpdateQuantityPostPNFRequest request);
        Task<CommonsResult> CreateRefundBarcodeHistory(UpdateQuantityPostPNFRequest request);
        Task<CommonsResult> ItemLocaionModify(ItemLocaionModifyRequest request);
        Task<CommonsResult> CreateItemHolder(CreateItemHolderRequest request);
        Task<GetDynamicListResult> GetDynamicList(long UserId, string unitID, string voucher_code, string status, string dateFrom, string dateTo);
        Task<DynamicResult> GetHistoryDNNK(long UserId, string unitID, string stt_rec);
        Task<DynamicResult> GetBarcodeValue(long UserId, string unitID, string barcode);
    }
}