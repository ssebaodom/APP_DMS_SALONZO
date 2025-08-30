using Microsoft.AspNetCore.Http;
using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class CheckinRequest : CommonRequest
    {
        public DateTime datetime { get; set; }
        public string userId { get; set; }
        public int page_index { get; set; }
        public int page_count { get; set; }
    }

    public class CreateTicketRequest
    {
        public string customerID { get; set; }
        public string comment { get; set; }
        public long userID { get; set; }
    }

    public class InventoryControlAndSaleOutRequest : CommonRequest
    {
        public InventoryControlAndSaleOutDTO Data { get; set; }
    }
    
    public class InventoryControlRequest : CommonRequest
    {
        public InventoryControlDTO Data { get; set; }
    }

    public class CheckOutRequest : CommonRequest
    {
        public CheckinDTO Data { get; set; }
        public string CustomerID { get; set; }
        public string LatLong { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public string IdAlbum { get; set; }
        public string IdCheckIn { get; set; }
        public DateTime TimeStartCheckIn { get; set; }
        public string TimeCheckOut { get; set; }
        public int OpenStore { get; set; }
        public int Status { get; set; }
        public string AddressDifferent { get; set; }
        public double LatDifferent { get; set; }
        public double LongDifferent { get; set; }
        public List<IFormFile> ListFile { get; set; }

    }
    
    public class RequestOpenStoreRequest : CommonRequest
    {
        public List<CheckinListImageDTO> Image { get; set; }
        
        public string StoreName { get; set; }
        public string StorePhone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string GPS { get; set; }
        public string IdTour { get; set; }
        public string Note { get; set; }
        public string IdArea { get; set; }
        public string IdTypeStore { get; set; }
        public string IdStoreForm { get; set; }
        public string MST { get; set; }
        public string Desc { get; set; }
        public string IdCommune { get; set; }
        public string IdState { get; set; }
        public string Location { get; set; }

        public List<IFormFile> ListFile { get; set; }
    }

    public class UpdateRequestOpenStoreRequest : CommonRequest
    {
        public List<CheckinListImageDTO> Image { get; set; }
        public string idRequestOpenStore { get; set; }
        public string StoreName { get; set; }
        public string StorePhone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string GPS { get; set; }
        public string IdTour { get; set; }
        public string Note { get; set; }
        public string IdArea { get; set; }
        public string IdTypeStore { get; set; }
        public string IdStoreForm { get; set; }
        public string MST { get; set; }
        public string Desc { get; set; }
        public string IdCommune { get; set; }
        public string IdState { get; set; }

        public List<IFormFile> ListFile { get; set; }
    }

    public class OrderCreateFromCheckInRequest : CommonRequest
    {
        public OrderCreateFromCheckInDTO Data { get; set; }
        public List<CheckinListImageDTO> Image { get; set; }
        //public List<IFormFile> ListFile { get; set; }
    }

    public class CreatNewTicketRequest : CommonRequest
    {
        public List<CheckinListImageDTO> Image { get; set; }
        public string CustomerCode { get; set; }
        public string TicketType { get; set; }
        public string TaskId { get; set; }
        public string Comment { get; set; }
        public List<IFormFile> ListFile { get; set; }
    }
}