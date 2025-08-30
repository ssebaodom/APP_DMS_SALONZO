using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class TodoCreateRequest : CommonRequest
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Descript { get; set; }
        public int Level { get; set; }
        public string AssignedName { get; set; }
        public string ShareName { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string ContactPerson { get; set; }
        public string Customer { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public int Status { get; set; }
    }
}