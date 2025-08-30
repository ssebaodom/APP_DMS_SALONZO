using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Api.v1.Results.Todos;
using SSE.Common.DTO.v1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IDMSDAL
    {
        Task<TodosLayoutResult> TodosLayoutPage(long UserId, string UnitId, string Lang, int Admin);
        Task<TodosCreateResult> TodosCreate(TodoCreateRequest values);
        Task<TodosViewerResult> TodosViewer(string TodoId, long UserId, string UnitId, string Lang, int Admin);
        Task<TodosListResult> TodosList(string Code, string Status, int PageIndex, long UserId, string UnitId, string Lang, int Admin, List<ReportRequestDTO> FilterData);
        Task<TodosCreateResult> TodoUpdate(TodoUpdateRequest values);
        /// <summary>
        /// Check-in 09/2022 tiennq
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// 

        Task<DynamicResult> SearchListCheckIn(long UserId, string dateTime, string searchKey, int page_index, int page_count);
        Task<DetailRequestOpenStoreResult> GetListRequestOpenStoreDetail(string idRequestOpenStore, long UserId);  
        Task<DynamicResult> GetListArea(string searchKey, int page_index, int page_count, long UserId);    
        Task<DynamicResult> GetListProvince(string province, string dictrict, int page_index, int page_count, string idArea);
        Task<DynamicResult> GetListTypeStore(string searchKey, int page_index, int page_count);
        Task<DynamicResult> GetListStoreForm(string searchKey, int page_index, int page_count);
        Task<CommonResult> OrderCreateFromCheckIn(OrderCreateFromCheckInRequest values);

        Task<CommonResult> CancelRequestOpenStore(string idTour, string idRequestOpenStore);
        Task<CommonResult> CreateRequestOpenStore(RequestOpenStoreRequest values);
        Task<CommonResult> UpdateRequestOpenStore(UpdateRequestOpenStoreRequest values);
        Task<CommonResult> CreateNewTicket(CreatNewTicketRequest values);
        Task<CheckInResult> GetListCheckin(CheckinRequest values);
        Task<DetailCheckInResult> GetDetailCheckin(string idCheckIn, string idCustomer);
        Task<DynamicResult> GetTourCheckin(string searchKey, int page_index, int page_count, string unitId, long idUser);
        Task<DynamicResult> GetListTypeTicket(int page_index, int page_count);
        Task<DataListTicketResult> GetListTicket(string ticketType, string idCheckIn, string idCustomer,long idUser ,int page_index, int page_count);
        Task<DynamicResult> GetListInventoryCheckIn(string idCustomer,string idCheckIn, int page_index, int page_count);
        Task<DataRequestOpenStoreResult> GetListRequestOpenStore(string dateForm, string dateTo, string district, int status, string dateTime,long UserId, int page_index, int page_count);
        Task<DynamicResult> GetListAlbumCheckIn(string idAlbum);
        Task<DynamicResult> GetListHistoryActionEmployee(string dateFrom, string dateTo, string idCustomer, long idUser, string unitId, int page_index, int page_count);
        Task<ListImageCheckInResult> GetListImageCheckin(string idAlbum, string idCustomer, string idCheckIn, int page_index, int page_count, long UserId, string UnitId);
        Task<CommonResult> CreateTicket(CreateTicketRequest values);
        Task<CommonResult> InventoryControl(InventoryControlRequest values);
        Task<CommonResult> UpdateSaleOut(InventoryControlAndSaleOutRequest values);
        Task<CommonResult> CheckOut(CheckOutRequest values);
        Task<CommonResult> ReportLocation(ReportLocationRequest values);
        Task<CommonResult> ReportLocationV2(ReportLocationRequestV2 values);
        Task<CommonResult> TimeKeepingCreate(TimeKeepingRequest values);
        Task<DynamicResult> CheckinHistory(CheckinHistoryRequest values);
        Task<TimeKeepingHistoryResult> TimeKeepingHistory(TimeKeepingHistoryRequest values);
        Task<DailyJobOfflineResult> DailyJobOffline(string dateTime, long idUser);
        Task<DynamicResult> GetListHistorySaleOut(string dateFrom, string dateTo, string idCustomer, int idTransaction, long idUser, string unitId, int page_index, int page_count);
        Task<DynamicResult> GetDetailHistorySaleOut(string stt_rec, string invoice_date, long idUser, string unitId);
        Task<DynamicResult> GetListHistoryTicket(string dateFrom, string dateTo, string idCustomer,string employeeCode, int status, long idUser, string unitId, int page_index, int page_count);
        Task<DetailHistoryTicketResult> GetDetailHistoryTicket(string idTicket, long idUser, string unitId);
        Task<DynamicResult> ListKPISummaryByDay(string dateFrom, string dateTo, long idUser, string unitId, string sotreId);
        Task<DynamicResult> ListStateOpenStore(string keySearch, int page_index, int page_count);

        Task<DynamicResult> GetListSaleOutCompleted(string idAgency,long UserId, string unitID, string dateForm, string dateTo, int page_index, int page_count);
        Task<DynamicResult> DetailSaleOutCompleted(long UserId, string unitID, string sct, string InvoiceDate);
        Task<CommonResult> RefundCreateSaleOutV1(RefundSaleOutCreateV1Request request);
        Task<DynamicResult> CreateTaskFromCustomer(string idCustomer, long UserId);
        Task<DynamicResult> DanhSachCauHoi(string searchKey, int page_index, int page_count);
        Task<DynamicResult> DanhSachCauTraLoi(string stt_rec, string ma_cau_hoi);
    }
}