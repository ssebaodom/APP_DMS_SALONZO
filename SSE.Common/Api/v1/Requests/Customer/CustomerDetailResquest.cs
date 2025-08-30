using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Customer
{
    public class CustomerDetailResquest : CommonRequest
    {
        public string CustomerCode { get; set; }
    }
}