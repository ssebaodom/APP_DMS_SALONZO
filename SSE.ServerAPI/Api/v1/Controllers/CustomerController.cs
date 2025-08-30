using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.Api.v1.Responses.Customer;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customer")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBLL customerBLL;

        public CustomerController(ICustomerBLL customerBLL)
        {
            this.customerBLL = customerBLL;
        }

        [Route("customer-list")]
        [HttpPost]
        public async Task<CustomerListResponse> CustomerList(CustomerListResquest request)
        {
            return await this.customerBLL.CustomerList(request);
        }

        [Route("search-customer-list-v2")]
        [HttpPost]
        public async Task<CustomerListResponse> SearchCustomerListV2(CustomerListResquest request)
        {
            return await this.customerBLL.SearchCustomerList(request);
        }
        [Route("customer-create")]
        [HttpPost]
        public async Task<CustomerCreateResponse> CustomerCreate(CustomerCreateResquest request)
        {
            return await this.customerBLL.CustomerCreate(request);
        }

        [Route("customer-info")]
        [HttpGet]
        public async Task<CustomerDetailResponse> CustomerInfo(string CustomerCode)
        {
            //CustomerDetailResquest request = new CustomerDetailResquest() { CustomerCode = CustomerCode };
            return await this.customerBLL.CustomerInfo(CustomerCode);
        }
        
        [Route("list-history-customer-care")]
        [HttpGet]
        public async Task<ListCustomerCareResponse> ListCustomerCare(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            return await this.customerBLL.ListCustomerCare(dateForm, dateTo, idCustomer, page_index, page_count);
        }

        [Route("customer-care-create")]
        [HttpPost]
        public async Task<CustomerCreateResponse> CustomerCareCreate([FromForm] CustomerCareCreateResquest request)
        {
            return await this.customerBLL.CustomerCareCreate(request);
        }

        [Route("list-history-customer-action")]
        [HttpGet]
        public async Task<DynamicResponse> ListCustomerAction(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            return await this.customerBLL.ListCustomerAction(dateForm, dateTo, idCustomer, page_index, page_count);
        }
    }
}