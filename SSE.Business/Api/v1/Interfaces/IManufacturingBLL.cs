using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Manufacturing;
using SSE.Common.Api.v1.Responses.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IManufacturingBLL
    {
        Task<DynamicResponse> RequestSectionItem(string request, string route, int page_index, int page_count);
        Task<DynamicResponse> GetVoucherTransaction(string vCCode);
        Task<DynamicResponse> GetItemMaterials(string item);
        Task<DynamicResponse> GetSemiProducts(string lsx, string section, string searchValue, int page_index, int page_count);
        Task<CommonsResponse> CreateFactoryTransactionVoucherModify(ManufacturingRequest request);
    }
}