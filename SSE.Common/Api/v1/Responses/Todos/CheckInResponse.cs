using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Todos
{
    public class CheckInResponse : BaseResponse
    {
        public IEnumerable<dynamic> ListCheckInToDay { get; set; }
        //public IEnumerable<dynamic> ListCheckInOther { get; set; }
        public int TotalPage { get; set; }
    }

    public class ListImageCheckInResponse : BaseResponse
    {
        public List<CheckinImageDTO> ListImage { get; set; }
        public IEnumerable<dynamic> ListAlbum { get; set; }
    }

    public class DetailCheckInResponse : BaseResponse
    {
        public List<dynamic> Master { get; set; }
        public List<dynamic> ListAlbum { get; set; }
        public List<dynamic> ListTicket { get; set; }
    }

    public class DetailRequestOpenStoreResponse : BaseResponse
    {
        public MasterRequestOpenStoreDetailDTO DetailRequestOpenStore { get; set; }
        public RoleUpdateRequestOpenStore Roles { get; set; }
      
    }

    public class ListRequestOpenStoreResponse : BaseResponse
    {
        public DataRequestOpentStoreDTO Data { get; set; }
        public List<Khuvuc> listKhuVuc { get; set; }
    }

    public class DetailListTicketResponse : BaseResponse
    {
        public List<MasterTicketDTO> Data { get; set; }
    }
    
    public class ListTimeKeepingHistoryResponse : BaseResponse
    {
        public MasterTimeKeeping Master { get; set; }
        public List<dynamic> listTimeKeepingHistory { get; set; }
    }

    public class DailyJobOfflineResponse : BaseResponse
    {
        public List<dynamic> ListCustomer { get; set; }
        public List<dynamic> ListAlbum { get; set; }
        public List<dynamic> ListTicket { get; set; }
    }
    
    public class DetailHistoryTicketResponse : BaseResponse
    {
        public MasterHistoryDetailTicketDTO Data { get; set; }
    }
}