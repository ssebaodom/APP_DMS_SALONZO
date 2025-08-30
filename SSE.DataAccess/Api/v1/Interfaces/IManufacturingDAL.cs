using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Manufacturing;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Results.Order;
using SSE.Core.Common.BaseApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IManufacturingDAL
    {
        Task<DynamicResult> RequestSectionItem( long UserId, string UnitId, string request, string route, int page_index, int page_count);
        Task<DynamicResult> GetVoucherTransaction( long UserId, string UnitId, string vCCode);
        Task<DynamicResult> GetItemMaterials( long UserId, string UnitId, string item);
        Task<DynamicResult> GetSemiProducts(long UserId, string UnitId, string lsx, string section, string searchValue, int page_index, int page_count);
        Task<CommonsResult> CreateFactoryTransactionVoucherModify(ManufacturingRequest request);
    }
}
