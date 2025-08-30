using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Responses.Customer
{
    public class CustomerDetailResponse : CommonResponse
    {
        public CustomerInfoDTO Data { get; set; }
    }
}