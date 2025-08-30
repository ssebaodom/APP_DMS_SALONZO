using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Results.Order;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IUserMember
    {
        Task<OrderItemSearchResult> GetPermissionUser(OrderItemSearchRequest orderItemSearch);
        Task<OrderItemScanResult> ScanItem(OrderItemScanRequest request);
    }
}