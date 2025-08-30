using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Implements;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Manufacturing;
using SSE.Common.Api.v1.Responses.Order;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/manufacturing")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ManufacturingController : Controller
    {
        private readonly IManufacturingBLL _manufacturingBLL;

        public ManufacturingController(IManufacturingBLL manufacturingBLL)
        {
            _manufacturingBLL = manufacturingBLL;

        }
        [Route("request-section-item")]
        [HttpGet]
        public async Task<DynamicResponse> RequestSectionItem(string request, string route, int page_index, int page_count)
        {
            return await _manufacturingBLL.RequestSectionItem(request, route, page_index, page_count);
        }
        [Route("get-voucher-transaction")]
        [HttpGet]
        public async Task<DynamicResponse> GetVoucherTransaction(string vCCode)
        {
            return await _manufacturingBLL.GetVoucherTransaction(vCCode);
        }
        [Route("get-item-materials")]
        [HttpGet]
        public async Task<DynamicResponse> GetItemMaterials(string item)
        {
            return await _manufacturingBLL.GetItemMaterials(item);
        }
        [Route("get-semi-products")]
        [HttpGet]
        public async Task<DynamicResponse> GetSemiProducts(string lsx, string section, string searchValue, int page_index, int page_count)
        {
            return await _manufacturingBLL.GetSemiProducts( lsx, section, searchValue, page_index, page_count);
        }
        
        [Route("create-factory-transaction-voucher-modify")]
        [HttpPost]
        public async Task<CommonsResponse> CreateFactoryTransactionVoucherModify(ManufacturingRequest request)
        {
            return await this._manufacturingBLL.CreateFactoryTransactionVoucherModify(request);
        }
    }
}
