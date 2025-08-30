using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.User
{
    public class GetNotifyTotalResult : BaseResult
    {
        public int Total { get; set; }
        public int UnreadTotal { get; set; }
    }
}