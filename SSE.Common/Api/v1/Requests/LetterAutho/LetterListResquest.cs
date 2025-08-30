using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.LetterAutho
{
    public class LetterListResquest : CommonRequest
    {
        public string LetterTypeId { get; set; }
        public int PageIndex { get; set; }
        public string Status { get; set; }
        public string TimeFilter { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int LastPage { get; set; }
        public string FirstElement { get; set; }
        public string LastElement { get; set; }
        public int TotalRec { get; set; }
    }
}