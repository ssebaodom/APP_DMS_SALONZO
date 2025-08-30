using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SSE.Core.Common.BaseApi
{
    public abstract class BaseResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
        public string Message { get; set; }
    }

    public class ApiListResponse<T> : BaseResponse
    {
        public IEnumerable<T> Data { get; set; }
    }
}