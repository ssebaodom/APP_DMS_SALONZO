using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Results.Customer
{
    public class CustomerDetailResult : CommonResult
    {
        public CustomerInfoDTO Data { get; set; }
    }
}