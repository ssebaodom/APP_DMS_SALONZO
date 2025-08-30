using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Home;
using SSE.Common.Api.v1.Responses.Home;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IHomeBLL
    {
        Task<GetFilterTimeResponse> GetFilterTime();

        Task<GetReportCategoriesResponse> GetReportCategories();

        Task<GetReportDataResponse> GetReportData(RepostDataRequest request);

        Task<GetDefaultDataResponse> GetDefaultData();
        Task<GetListSliderImageResponse> GetSliderImages();
    }
}