using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Todos
{
    public class TodosLayoutResponse : BaseResponse
    {
        public IEnumerable<TodoTypeListDTO> TodoType { get; set; }
        public IEnumerable<TodoReportDTO> TodoReport { get; set; }
        public dynamic TodoStatus { get; set; }
        public dynamic TodoStatusSelection { get; set; }
        public dynamic TodoChart { get; set; }
    }
}