using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Todos
{
    public class TodosCreateResult : BaseResult
    {
        public UserNotifyDTO userNotify { get; set; }
       
    }
}