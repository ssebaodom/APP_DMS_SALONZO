using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.Api.v1.Responses.Customer;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface ICustomerBLL
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <param name="customerList"></param>
        /// <returns></returns>
        Task<CustomerListResponse> CustomerList(CustomerListResquest customerList);
        Task<CustomerListResponse> SearchCustomerList(CustomerListResquest customerList);
        /// <summary>
        /// Tạo mới khách hàng
        /// </summary>
        /// <param name="customerCreate"></param>
        /// <returns></returns>
        Task<CustomerCreateResponse> CustomerCreate(CustomerCreateResquest customerCreate);
        /// <summary>
        /// Chi tiết khách hàng
        /// </summary>
        /// <param name="customerDetail"></param>
        /// <returns></returns>
        Task<CustomerDetailResponse> CustomerInfo(string codeCustomer);
        Task<ListCustomerCareResponse> ListCustomerCare(string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
        Task<CustomerCreateResponse> CustomerCareCreate(CustomerCareCreateResquest customerCreate);
        Task<DynamicResponse> ListCustomerAction(string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
    }
}