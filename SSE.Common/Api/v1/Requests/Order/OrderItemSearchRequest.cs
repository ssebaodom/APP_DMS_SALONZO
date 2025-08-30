using SSE.Common.Api.v1.Common;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class OrderItemSearchRequest : CommonRequest
    {
        public string SearchValue { get; set; }
        public string ItemGroup { get; set; }
        public string ItemGroup2 { get; set; }
        public string ItemGroup3 { get; set; }
        public string ItemGroup4 { get; set; }
        public int PageIndex { get; set; }
        public string Currency { get; set; }
    }

    public class OrderItemSearchV2Request : CommonRequest
    {
        public string SearchValue { get; set; }
        public string IdCustomer { get; set; }
        public string ItemGroup { get; set; }
        public string ItemGroup2 { get; set; }
        public string ItemGroup3 { get; set; }
        public string ItemGroup4 { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public string Currency { get; set; }
        public string keyGroup { get; set; }
    }
}