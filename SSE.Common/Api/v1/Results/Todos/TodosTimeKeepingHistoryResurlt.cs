using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Todos
{
    public class TodosTimeKeepingHistoryResurlt : BaseResult
    {
        public string Resurlt { get; set; }
        public IEnumerable<dynamic> Data1 { get; set; }
        public IEnumerable<dynamic> Data2{ get; set; }
    }
}