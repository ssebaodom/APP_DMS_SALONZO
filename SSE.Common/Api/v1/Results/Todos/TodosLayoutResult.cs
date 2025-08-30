using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Todos
{
    public class TodosLayoutResult : BaseResult
    {
        public IEnumerable<TodoTypeListDTO> TodoType { get; set; }
        public IEnumerable<TodoReportDTO> TodoReport { get; set; }
        public dynamic TodoStatus { get; set; }
        public dynamic TodoStatusSelection { get; set; }
        public dynamic TodoChart { get; set; }
    }
}