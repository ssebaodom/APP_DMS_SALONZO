using SSE.Common.Constants.v1;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class ReportLayoutDTO
    {
        public string Field { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public DataType Type { get; set; }
        public string DefaultValue { get; set; }
        public bool IsNull { get; set; }
        public string Controller { get; set; }
        public List<DropDownDTO> DropDownList { get; set; }
    }
}