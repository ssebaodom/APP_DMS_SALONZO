using Microsoft.AspNetCore.Http;
using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Common
{
    public class CommonRequest
    {
        public long UserId { get; set; }
        public string UnitId { get; set; }
        public string StoreId { get; set; }
        public string Lang { get; set; }
        public int Admin { get; set; }
    }

    public class ApiObjectResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}