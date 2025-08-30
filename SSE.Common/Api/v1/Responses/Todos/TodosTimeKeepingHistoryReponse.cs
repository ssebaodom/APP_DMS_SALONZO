using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Todos
{
    public class TodosTimeKeepingHistoryReponse : BaseResponse
    {
        public string Resurlt { get; set; }
        public IEnumerable<dynamic> Data1 { get; set; }
        public IEnumerable<dynamic> Data2 { get; set; }

    }
}