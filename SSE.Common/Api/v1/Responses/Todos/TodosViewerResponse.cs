using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Todos
{
    public class TodosViewerResponse : BaseResponse
    {
        public dynamic Data { get; set; }
        
    }
}