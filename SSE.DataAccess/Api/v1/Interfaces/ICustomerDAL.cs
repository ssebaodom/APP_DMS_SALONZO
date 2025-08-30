using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.Api.v1.Results.Customer;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface ICustomerDAL
    {
        Task<CustomerListResult> CustomerList(CustomerListResquest customerList);
        Task<CustomerListResult> SearchCustomerList(CustomerListResquest customerList);

        Task<CustomerCreateResult> CustomerCreate(CustomerCreateResquest customerCreate);

        Task<CustomerDetailResult> CustomerInfo(string codeCustomer, long UserId, string unitID, string storeID, string lang, int admin);

        Task<ListCustomerCareResult> ListCustomerCare(long UserId,string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count);

        Task<CommonResult> CustomerCareCreate(CustomerCareCreateResquest customerCreate);

        Task<DynamicResult> ListCustomerAction(long UserId, string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count);
    }
}