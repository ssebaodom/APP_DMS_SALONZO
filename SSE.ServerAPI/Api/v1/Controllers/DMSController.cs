using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Api.v1.Responses.Todos;
using SSE.Common.DTO.v1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/todos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DMSController : ControllerBase
    {
        private readonly IDMSBLL todosBLL;
        
        public DMSController(IDMSBLL todosBLL)
        {
            this.todosBLL = todosBLL;
        }

        [Route("create-todo")]
        [HttpPost]
        public async Task<CommonResponse> CreateTodo(TodoCreateRequest request)
        {
            return await this.todosBLL.CreateTodo(request);
        }
        [Route("update-todo")]
        [HttpPost]
        public async Task<CommonResponse> UpdateTodo(TodoUpdateRequest request)
        {
            return await this.todosBLL.UpdateTodo(request);
        }
        [Route("todos-viewer")]
        [HttpGet]
        public async Task<TodosViewerResponse> TodosViewer(string Id)
        {
            return await this.todosBLL.TodosViewer(Id);
        }
        [Route("todos-list")]
        [HttpPost]
        public async Task<TodosListResponse> TodosList(string Code, string Status, [FromQuery] int PageIndex, [FromBody] List<ReportRequestDTO> FilterData)
        {
            return await this.todosBLL.TodosList(Code, Status, PageIndex, FilterData);
        }
        [Route("todos-layout")]
        [HttpGet]
        public async Task<TodosLayoutResponse> TodosLayoutPage()
        {
            return await this.todosBLL.TodosLayoutPage();
        }
        /// <summary>
        /// Check-in DMS 09/2022 Tiennq
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [Route("list-province")]
        [HttpGet]
        public async Task<DynamicResponse> GetListProvince(string idProvince, string idDistrict, int page_index, int page_count, string idArea)
        {
            return await this.todosBLL.GetListProvince(idProvince,idDistrict,page_index, page_count, idArea);
        }
        [Route("list-area")]
        [HttpGet]
        public async Task<DynamicResponse> GetListArea(string searchKey, int page_index, int page_count)
        {
            return await this.todosBLL.GetListArea(searchKey, page_index, page_count);
        }
        [Route("list-type-store")]
        [HttpGet]
        public async Task<DynamicResponse> GetListTypeStore(string searchKey, int page_index, int page_count)
        {
            return await this.todosBLL.GetListTypeStore(searchKey, page_index, page_count);
        }
        [Route("list-store-form")]
        [HttpGet]
        public async Task<DynamicResponse> GetListStoreForm(string searchKey, int page_index, int page_count)
        {
            return await this.todosBLL.GetListStoreForm(searchKey, page_index, page_count);
        }
        [Route("list-type-ticket")]
        [HttpGet]
        public async Task<DynamicResponse> GetListTypeTicket(int page_index, int page_count)
        {
            return await this.todosBLL.GetListTypeTicket(page_index, page_count);
        }
        [Route("create-new-ticket")]
        [HttpPost]
        public async Task<CommonResponse> CreateNewTicket([FromForm] CreatNewTicketRequest request)
        {
            return await this.todosBLL.CreateNewTicket(request);
        }
        [Route("order-create-from-checkin")]
        [HttpPost]
        public async Task<CommonResponse> OrderCreateFromCheckIn(OrderCreateFromCheckInRequest request)
        {
            return await this.todosBLL.OrderCreateFromCheckIn(request);
        }
        [Route("create-request-open-store")]
        [HttpPost]
        public async Task<CommonResponse> CreateRequestOpenStore([FromForm] RequestOpenStoreRequest request)
        {
            return await this.todosBLL.CreateRequestOpenStore(request);
        }
        [Route("update-request-open-store")]
        [HttpPost]
        public async Task<CommonResponse> UpdateRequestOpenStore([FromForm] UpdateRequestOpenStoreRequest request)
        {
            return await this.todosBLL.UpdateRequestOpenStore(request);
        }
        [Route("cancel-request-open-store")]
        [HttpGet]
        public async Task<CommonResponse> CancelRequestOpenStore(string idTour, string idRequestOpenStore)
        {
            return await this.todosBLL.CancelRequestOpenStore(idTour,idRequestOpenStore);
        }
        [Route("list-request-open-store")]
        [HttpGet]
        public async Task<ListRequestOpenStoreResponse> GetListRequestOpenStore(string dateForm, string dateTo, string district, int status,string dateTime, int page_index, int page_count)
        {
            return await this.todosBLL.GetListRequestOpenStore(dateForm,dateTo,district,status,dateTime, page_index, page_count);
        }
        [Route("list-request-open-store-detail")]
        [HttpGet]
        public async Task<DetailRequestOpenStoreResponse> GetListRequestOpenStoreDetail(string idRequestOpenStore)
        {
            return await todosBLL.GetListRequestOpenStoreDetail(idRequestOpenStore);
        }
        [Route("search-list-check-in")]
        [HttpGet]
        public async Task<DynamicResponse> SearchListCheckIn(string dateTime, string searchKey, int page_index, int page_count)
        {
            return await todosBLL.SearchListCheckIn(dateTime,searchKey, page_index, page_count);
        }
        [Route("list-check-in")]
        [HttpPost]
        public async Task<CheckInResponse> GetListCheckin(CheckinRequest request)
        {
            return await this.todosBLL.GetListCheckin(request);
        }
        [Route("detail-check-in")]
        [HttpGet]
        public async Task<DetailCheckInResponse> GetDetailCheckin(string idCheckIn, string idCustomer)
        {
            return await this.todosBLL.GetDetailCheckin(idCheckIn, idCustomer);
        }

        [Route("tour-check-in")]
        [HttpGet]
        public async Task<DynamicResponse> GetTourCheckin(string searchKey, int page_index, int page_count)
        {
            return await this.todosBLL.GetTourCheckin(searchKey, page_index,page_count);
        }

        [Route("list-ticket")]
        [HttpGet]
        public async Task<DetailListTicketResponse> GetListTicket(string ticketType, string idCheckIn,string idCustomer,int page_index, int page_count)
        {
            return await this.todosBLL.GetListTicket(ticketType, idCheckIn,idCustomer, page_index, page_count);
        }

        [Route("list-inventory-check-in")]
        [HttpGet]
        public async Task<DynamicResponse> GetListInventoryCheckIn(string idCustomer,string idCheckIn ,int page_index, int page_count)
        {
            return await this.todosBLL.GetListInventoryCheckIn(idCustomer, idCheckIn ,page_index, page_count);
        }

        [Route("list-album-check-in")]
        [HttpGet]
        public async Task<DynamicResponse> GetListAlbumCheckIn(string idAlbum)
        {
            return await this.todosBLL.GetListAlbumCheckIn(idAlbum);
        }

        [Route("list-image-check-in")]
        [HttpGet]
        public async Task<ListImageCheckInResponse> GetListImageCheckin(string idAlbum, string idCustomer, string idCheckin, int page_index, int page_count)
        {
            return await this.todosBLL.GetListImageCheckin(idAlbum, idCustomer,idCheckin, page_index, page_count);
        }

        [Route("create-ticket")]
        [HttpPost]
        public async Task<CommonResponse> CreateTicket(CreateTicketRequest request)
        {
            return await this.todosBLL.CreateTicket(request);
        }

        [Route("save-inventory-control")]
        [HttpPost]
        public async Task<CommonResponse> InventoryControl(InventoryControlRequest request)
        {
            return await this.todosBLL.InventoryControl(request);
        }
        [Route("update-sale-out")]
        [HttpPost]
        public async Task<CommonResponse> UpdateSaleOut(InventoryControlAndSaleOutRequest request)
        {
            return await this.todosBLL.UpdateSaleOut(request);
        }

        [Route("list-history-sale-out")]
        [HttpGet]
        public async Task<DynamicResponse> GetListHistorySaleOut(string dateFrom, string dateTo, string idCustomer,int idTransaction, int page_index, int page_count)
        {
            return await this.todosBLL.GetListHistorySaleOut(dateFrom, dateTo, idCustomer, idTransaction, page_index, page_count);
        }

        [Route("detail-history-sale-out")]
        [HttpGet]
        public async Task<DynamicResponse> GetDetailHistorySaleOut(string stt_rec, string invoice_date)
        {
            return await this.todosBLL.GetDetailHistorySaleOut(stt_rec, invoice_date);
        }

        [Route("check-out")]
        [HttpPost]
        public async Task<CommonResponse> CheckOut([FromForm] CheckOutRequest request)
        {
            return await this.todosBLL.CheckOut(request);
        }
        [Route("daily-job-offline")]
        [HttpGet]
        public async Task<DailyJobOfflineResponse> DailyJobOffline(string dateTime)
        {
            return await this.todosBLL.DailyJobOffline(dateTime);
        }
                             
        [Route("report-location")]
        [HttpPost]
        public async Task<CommonResponse> ReportLocation(ReportLocationRequest request)
        {
            return await this.todosBLL.ReportLocation(request);
        }
        
        [Route("report-location-v2")]
        [HttpPost]
        public async Task<CommonResponse> ReportLocationV2([FromForm] ReportLocationRequestV2 request)
        {
            return await this.todosBLL.ReportLocationV2(request);
        }
        //[Route("daily1")]
        //[HttpGet]
        //public async Task<DailyJobOfflineResponse> DailyJobOffline()
        //{
        //    return await this.todosBLL.DailyJobOffline("");
        //}
        /// <summary>
        /// Chấm công QR Code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("time-keeping")]
        [HttpPost]
        public async Task<CommonResponse> TimeKeeping(TimeKeepingRequest request)
        {
            return await this.todosBLL.TimeKeepingCreate(request);
        }
        [Route("time-keeping-history")]
        [HttpPost]
        public async Task<ListTimeKeepingHistoryResponse> TimeKeepingHistory(TimeKeepingHistoryRequest request)
        {
            return await this.todosBLL.TimeKeepingHistory(request);
        }
        [Route("checkin-history")]
        [HttpPost]
        public async Task<DynamicResponse> CheckinHistory(CheckinHistoryRequest request)
        {
            return await this.todosBLL.CheckinHistory(request);
        }

        [Route("list-history-action-employee")]
        [HttpGet]
        public async Task<DynamicResponse> GetListHistoryActionEmployee(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            return await this.todosBLL.GetListHistoryActionEmployee(dateForm,  dateTo, idCustomer,page_index, page_count);
        }

        [Route("list-history-ticket")]
        [HttpGet]
        public async Task<DynamicResponse> GetListHistoryTicket(string dateForm, string dateTo, string idCustomer, string employeeCode, int status ,int page_index, int page_count)
        {
            return await this.todosBLL.GetListHistoryTicket(dateForm, dateTo, idCustomer, employeeCode, status, page_index, page_count);
        }
        
        [Route("detail-history-ticket")]
        [HttpGet]
        public async Task<DetailHistoryTicketResponse> GetDetailHistoryTicket(string idTicket)
        {
            return await this.todosBLL.GetDetailHistoryTicket(idTicket);
        }

        [Route("list-kpi-summary")]
        [HttpGet]
        public async Task<DynamicResponse> ListKPISummaryByDay(string dateForm, string dateTo)
        {
            return await this.todosBLL.ListKPISummaryByDay(dateForm, dateTo);
        }
        
        [Route("list-state-open-store")]
        [HttpGet]
        public async Task<DynamicResponse> ListStateOpenStore(string searchKey, int page_index, int page_count)
        {
            return await this.todosBLL.ListStateOpenStore(searchKey, page_index,page_count);
        }
        [Route("list-sale-out-completed")]
        [HttpGet]
        public async Task<DynamicResponse> GetListSaleOutCompleted(string idAgency, string dateForm, string dateTo,  int page_index, int page_count)
        {
            return await this.todosBLL.GetListSaleOutCompleted(idAgency,dateForm, dateTo, page_index, page_count);
        }

        [Route("detail-sale-out-completed")]
        [HttpGet]
        public async Task<DynamicResponse> DetailOrderCompleted(string sct, string invoiceDate)
        {
            return await this.todosBLL.DetailSaleOutCompleted(sct, invoiceDate);
        }

        [Route("create-refund-sale-out-v1")]
        [HttpPost]
        public async Task<CommonResponse> RefundCreateSaleOutV1(RefundSaleOutCreateV1Request request)
        {
            return await this.todosBLL.RefundCreateSaleOutV1(request);
        }

        [Route("create-task-from-customer")]
        [HttpGet]
        public async Task<DynamicResponse> CreateTaskFromCustomer(string idCustomer)
        {
            return await this.todosBLL.CreateTaskFromCustomer(idCustomer);
        }
        [Route("danh-sach-cau-hoi")]
        [HttpGet]
        public async Task<DynamicResponse> DanhSachCauHoi(string searchKey, int page_index, int page_count)
        {
            return await this.todosBLL.DanhSachCauHoi(searchKey, page_index, page_count);
        }
        [Route("danh-sach-cau-tra-loi")]
        [HttpGet]
        public async Task<DynamicResponse> DanhSachCauTraLoi(string stt_rec, string ma_cau_hoi)
        {
            return await this.todosBLL.DanhSachCauTraLoi(stt_rec, ma_cau_hoi);
        }
    }
}