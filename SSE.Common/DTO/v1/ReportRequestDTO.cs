using SSE.Common.Constants.v1;

namespace SSE.Common.DTO.v1
{
    public class ReportRequestDTO
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
    }
}