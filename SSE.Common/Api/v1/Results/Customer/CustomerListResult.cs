using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Customer
{
    public class CustomerListResult : CommonResult
    {
        public IEnumerable<CustomerListDTO> Data { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
    }

    public class ListCustomerCareResult : CommonResult
    {
        public List<CareCustomerListDTO> ListCustomerCare { get; set; }
        public int TotalPage { get; set; }
    }
}