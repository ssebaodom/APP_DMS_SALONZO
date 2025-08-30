using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Home;
using SSE.Common.Api.v1.Responses.Home;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/home")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeBLL homeBLL;

        public HomeController(IHomeBLL homeBLL)
        {
            this.homeBLL = homeBLL;
        }

        [Route("filter-times")]
        [HttpGet]
        public async Task<GetFilterTimeResponse> GetFilterTime()
        {
            return await this.homeBLL.GetFilterTime();
        }

        [Route("reports")]
        [HttpGet]
        public async Task<GetReportCategoriesResponse> GetReportCategories()
        {
            return await this.homeBLL.GetReportCategories();
        }

        [Route("reports")]
        [HttpPost]
        public async Task<GetReportDataResponse> GetReportData(RepostDataRequest request)
        {
            return await this.homeBLL.GetReportData(request);
        }

        [HttpGet]
        public async Task<GetDefaultDataResponse> GetDefaultData()
        {
            return await this.homeBLL.GetDefaultData();
        }
        [Route("get-slider-images")]
        [HttpGet]
        public async Task<GetListSliderImageResponse> GetSliderImages()
        {
            return await this.homeBLL.GetSliderImages();
        }
    }
}