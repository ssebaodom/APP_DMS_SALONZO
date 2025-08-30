using Newtonsoft.Json;

namespace SSE.Common.DTO.v1
{
    public class CompanyDTO
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }

        [JsonIgnore]
        public string AppDbName { get; set; }
    }
}