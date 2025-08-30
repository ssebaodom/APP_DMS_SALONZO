using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Todos
{
    public class CheckInResult : BaseResult
    {
        public IEnumerable<dynamic> ListCheckInToDay { get; set; }
        //public IEnumerable<dynamic> ListCheckInOther { get; set; }
        public int TotalPage { get; set; }
    }

    public class ListImageCheckInResult : BaseResult
    {
        public List<CheckinImageDTO> ListImage { get; set; }
        public IEnumerable<dynamic> ListAlbum { get; set; }
    }
    
    public class TimeKeepingHistoryResult : BaseResult
    {
        public MasterTimeKeeping Master { get; set; }
        public List<dynamic> listTimeKeepingHistory { get; set; }
    }
    
    public class DetailCheckInResult : BaseResult
    {
        public List<dynamic> Master { get; set; }
        public List<dynamic> ListAlbum { get; set; }
        public List<dynamic> ListTicket { get; set; }
    }
    
    public class DataRequestOpenStoreResult : BaseResult
    { 
        public DataRequestOpentStoreDTO Data { get; set; }
        public List<Khuvuc> listKhuVuc { get; set; }
    }

    public class DataRequestOpenStoreResultRe : BaseResult
    {
        public MasterRequestOpenStore Master { get; set; }
        public List<ImageListRequestOpentStore> imageListRequestOpenStore { get; set; }
    }

    public class DetailRequestOpenStoreResult : BaseResult
    {
        public MasterRequestOpenStoreDetailDTO DetailRequestOpenStore { get; set; }
        public RoleUpdateRequestOpenStore Roles { get; set; }

    }

    public class DataListTicketResult : BaseResult
    {
        public List<MasterTicketDTO> Data { get; set; }
    }

    public class DailyJobOfflineResult : BaseResult
    {
        public List<dynamic> ListCustomer { get; set; }
        public List<dynamic> ListAlbum { get; set; }
        public List<dynamic> ListTicket { get; set; }
    }

    public class DetailHistoryTicketResult : BaseResult
    {
        public MasterHistoryDetailTicketDTO Data { get; set; }
    }
}