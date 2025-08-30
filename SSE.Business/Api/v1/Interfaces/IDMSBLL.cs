using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Api.v1.Responses.Todos;
using SSE.Common.DTO.v1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IDMSBLL
    {
        Task<TodosLayoutResponse> TodosLayoutPage();
        Task<CommonResponse> CreateTodo(TodoCreateRequest request);
        Task<TodosViewerResponse> TodosViewer(string TodoId);
        Task<TodosListResponse> TodosList(string Code, string Status, int PageIndex, List<ReportRequestDTO> FilterData);
        Task<CommonResponse> UpdateTodo(TodoUpdateRequest request);
        /// <summary>
        /// Checkin 09/2022 tiennq
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// 
        Task<DynamicResponse> SearchListCheckIn(string dateTime, string searchKey, int page_index, int page_count);
        Task<DetailRequestOpenStoreResponse> GetListRequestOpenStoreDetail(string idRequestOpenStore);
        Task<DynamicResponse> GetListArea(string searchKey, int page_index, int page_count); 
        Task<DynamicResponse> GetListProvince(string province, string dictrict, int page_index, int page_count, string idArea);
        Task<DynamicResponse> GetListTypeStore(string searchKey, int page_index, int page_count);
        Task<DynamicResponse> GetListStoreForm(string searchKey, int page_index, int page_count);
        Task<CommonResponse> OrderCreateFromCheckIn(OrderCreateFromCheckInRequest values);
        Task<CommonResponse> CreateNewTicket(CreatNewTicketRequest values);
        Task<CommonResponse> CreateRequestOpenStore(RequestOpenStoreRequest values);
        Task<CommonResponse> UpdateRequestOpenStore(UpdateRequestOpenStoreRequest values);
        Task<CommonResponse> CancelRequestOpenStore(string idTour, string idRequestOpenStore);

        Task<ListRequestOpenStoreResponse> GetListRequestOpenStore(string dateForm, string dateTo, string district, int status,string dateTime, int page_index, int page_count);
        Task<CheckInResponse> GetListCheckin(CheckinRequest values);
        Task<DetailCheckInResponse> GetDetailCheckin(string idCheckIn, string idCustomer);
        Task<DynamicResponse> GetTourCheckin(string searchKey, int page_index, int page_count);
        Task<DynamicResponse> GetListTypeTicket(int page_index, int page_count);
        Task<DetailListTicketResponse> GetListTicket(string ticketType, string idCheckIn, string idCustomer, int page_index, int page_count);
        Task<DynamicResponse> GetListInventoryCheckIn(string idCustomer,string idCheckIn,int page_index, int page_count);
        Task<DynamicResponse> GetListAlbumCheckIn(string idAlbum);
        Task<DynamicResponse> GetListHistoryActionEmployee(string dateFrom, string dateTo, string idCustomer, int page_index, int page_count);
        Task<ListImageCheckInResponse> GetListImageCheckin(string idAlbum, string idCustomer,string idCheckIn, int page_index, int page_count);
        Task<CommonResponse> CreateTicket(CreateTicketRequest values);
        Task<CommonResponse> InventoryControl(InventoryControlRequest values);
        Task<CommonResponse> UpdateSaleOut(InventoryControlAndSaleOutRequest values);
        Task<CommonResponse> CheckOut(CheckOutRequest values);
        Task<CommonResponse> ReportLocation(ReportLocationRequest values);
        Task<CommonResponse> ReportLocationV2(ReportLocationRequestV2 values);
        Task<CommonResponse> TimeKeepingCreate(TimeKeepingRequest request);
        Task<ListTimeKeepingHistoryResponse> TimeKeepingHistory(TimeKeepingHistoryRequest request);
        Task<DynamicResponse> CheckinHistory(CheckinHistoryRequest values);
        Task<DailyJobOfflineResponse> DailyJobOffline(string dateTime);
        Task<DynamicResponse> GetListHistorySaleOut(string dateFrom, string dateTo, string idCustomer, int idTransaction, int page_index, int page_count);
        Task<DynamicResponse> GetDetailHistorySaleOut(string stt_rec, string invoice_date);
        Task<DynamicResponse> GetListHistoryTicket(string dateFrom, string dateTo, string idCustomer, string employeeCode, int status, int page_index, int page_count);
        Task<DetailHistoryTicketResponse> GetDetailHistoryTicket(string idTicket);
        Task<DynamicResponse> ListKPISummaryByDay(string dateFrom, string dateTo);
        Task<DynamicResponse> ListStateOpenStore(string keySearch, int page_index, int page_count);

        Task<DynamicResponse> GetListSaleOutCompleted(string idAgency,string dateForm, string dateTo, int page_index, int page_count);
        Task<DynamicResponse> DetailSaleOutCompleted(string sct, string invoiceDate);
        Task<CommonResponse> RefundCreateSaleOutV1(RefundSaleOutCreateV1Request request);
        Task<DynamicResponse> CreateTaskFromCustomer(string idCustomer);
        Task<DynamicResponse> DanhSachCauHoi(string keySearch, int page_index, int page_count);
        Task<DynamicResponse> DanhSachCauTraLoi(string stt_rec, string ma_cau_hoi);
    }
}