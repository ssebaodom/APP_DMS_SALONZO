using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Customer
{
    public class CustomerListResquest : CommonRequest
    {
        public int Type { get; set; }
        public int PageIndex { get; set; }
        public string SearchValue { get; set; }
        public string TypeName { get; set; }
    }
}