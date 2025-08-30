using SSE.Core.Common.Api;

namespace SSE.Common.DTO.v1
{
    public class ReportParamsDTO : BaseRequest
    {
        public string ReportId { get; set; }
        public string TimeId { get; set; }
        public string Lang { get; set; }
        public long UserId { get; set; }
        public int Role { get; set; }
        public string UnitId { get; set; }
        public string StoreId { get; set; }
        public int UoM { get; set; }
        public string ReportType { get; set; }
    }
}