using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class ReportGroupDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public List<ReportListDTO> ReportList { get; set; }
    }
}