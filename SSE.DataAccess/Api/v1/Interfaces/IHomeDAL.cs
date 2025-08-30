using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Home;
using SSE.Common.Api.v1.Results.Home;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IHomeDAL
    {
        Task<GetFilterTimeResult> GetFilterTime(string lang = "v");

        Task<GetReportCategoriesResult> GetReportCategories(long userId, string lang = "v");

        Task<GetReportDataResult> GetReportData(RepostDataRequest request);

        Task<SettingValuesResult> GetSettingValues(string unitId, long userId, int Role, string lang = "v");
        Task<GetListSliderImageResult> GetSliderImages(long UserId, string UnitId);
    }
}