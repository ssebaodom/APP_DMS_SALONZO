using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.User
{
    public class GetNotifyTotalResponse : BaseResponse
    {
        public int Total { get; set; }
        public int UnreadTotal { get; set; }
    }
}