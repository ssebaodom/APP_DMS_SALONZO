using Microsoft.AspNetCore.Http;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Home;
using SSE.Common.Api.v1.Responses.Home;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Caches;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    public class HomeBLL : IHomeBLL
    {
        private readonly IHomeDAL homeDAL;
        private UserInfoCache userInfoCache;
        private readonly IUserBLLService userBLLService; 
        private readonly ICached cached;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeBLL(IHomeDAL homeDAL,
                       IUserBLLService userBLLService,
                       ICached cached,
                       IHttpContextAccessor httpContextAccessor)
        {
            this.homeDAL = homeDAL;
            this.userBLLService = userBLLService;
            this.cached = cached;
            this.httpContextAccessor = httpContextAccessor;

            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }

        public async Task<GetFilterTimeResponse> GetFilterTime()
        {
            string filterTimeKey = string.Concat(CACHE_KEYS.FILTER_TIMES, "_", userInfoCache.Lang);

            // Lấy dữ liệu trong cache.
            List<FilterTimeDTO> timesList = cached.GetList<FilterTimeDTO>(filterTimeKey);

            // Nếu trong cache chưa có dữ liệu. Đọc trong Db rồi lưu vào cache.
            if (timesList == null || timesList.Count == 0)
            {
                var getFilterTimeResult = await homeDAL.GetFilterTime(userInfoCache.Lang);

                if (!getFilterTimeResult.IsSucceeded)
                {
                    return new GetFilterTimeResponse
                    {
                        StatusCode = StatusCodes.Status202Accepted
                    };
                }

                cached.AddRangeToList(filterTimeKey, getFilterTimeResult.FilterTimes.ToList());

                return new GetFilterTimeResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    FilterTimes = getFilterTimeResult.FilterTimes
                };
            }

            return new GetFilterTimeResponse
            {
                StatusCode = StatusCodes.Status200OK,
                FilterTimes = timesList
            };
        }

        public async Task<GetReportCategoriesResponse> GetReportCategories()
        {
            var getFilterTimeResult = await homeDAL.GetReportCategories(userInfoCache.UserId, userInfoCache.Lang);

            if (!getFilterTimeResult.IsSucceeded)
            {
                return new GetReportCategoriesResponse
                {
                    StatusCode = StatusCodes.Status202Accepted
                };
            }

            return new GetReportCategoriesResponse
            {
                StatusCode = StatusCodes.Status200OK,
                ReportCategories = getFilterTimeResult.ReportCategories
            };
        }

        public async Task<GetDefaultDataResponse> GetDefaultData()
        {
            GetFilterTimeResponse getFilterTimeResponse = await GetFilterTime();

            if (getFilterTimeResponse.StatusCode != StatusCodes.Status200OK)
            {
                return new GetDefaultDataResponse
                {
                    StatusCode = StatusCodes.Status204NoContent
                };
            }

            GetReportCategoriesResponse getReportCategoriesResponse = await GetReportCategories();

            if (getReportCategoriesResponse.StatusCode != StatusCodes.Status200OK)
            {
                return new GetDefaultDataResponse
                {
                    StatusCode = StatusCodes.Status204NoContent
                };
            }

            RepostDataRequest request = new RepostDataRequest
            {
                user_id = userInfoCache.UserId,
                role = userInfoCache.Role,
                lang = userInfoCache.Lang,
                store_id = userInfoCache.StoreId,
                unit_id = userInfoCache.UnitId,
                report_id = getReportCategoriesResponse.ReportCategories.FirstOrDefault().ReportId
            };

            GetReportDataResponse getReportDataResponse = await GetReportData(request);

            if (getReportDataResponse.StatusCode != StatusCodes.Status200OK)
            {
                return new GetDefaultDataResponse
                {
                    StatusCode = StatusCodes.Status204NoContent
                };
            }

            var settingValues = await homeDAL.GetSettingValues(userInfoCache.UnitId, userInfoCache.UserId, userInfoCache.Role, userInfoCache.Lang);

            if (settingValues.IsSucceeded == false)
                settingValues.CurrencyList = null;

            return new GetDefaultDataResponse
            {
                StatusCode = StatusCodes.Status200OK,
                FilterTimes = getFilterTimeResponse.FilterTimes,
                ReportCategories = getReportCategoriesResponse.ReportCategories,
                ReportInfo = getReportDataResponse.ReportInfo,
                ReportData = getReportDataResponse.ReportData,
                CurrencyList = settingValues.CurrencyList,
                StockList = settingValues.StockList,
                IsCallServerCart = settingValues.IsCallServerCart,
                NumberFormat = settingValues.NumberFormat
            };
        }

        public async Task<GetReportDataResponse> GetReportData(RepostDataRequest request)
        {
            request.user_id = userInfoCache.UserId;
            request.role = userInfoCache.Role;
            request.unit_id = userInfoCache.UnitId;
            request.store_id = userInfoCache.StoreId;
            request.lang = userInfoCache.Lang;

            var getReportDataResult = await homeDAL.GetReportData(request);

            if (!getReportDataResult.IsSucceeded)
            {
                return new GetReportDataResponse
                {
                    StatusCode = StatusCodes.Status202Accepted
                };
            }

            return new GetReportDataResponse
            {
                StatusCode = StatusCodes.Status200OK,
                ReportInfo = getReportDataResult.ReportInfo,
                ReportData = getReportDataResult.ReportData,
                HeaderDesc = getReportDataResult.HeaderDesc
            };
        }

        public async Task<GetListSliderImageResponse> GetSliderImages()
        {
            var result = await this.homeDAL.GetSliderImages(userInfoCache.UserId, userInfoCache.UnitId);

            if (result.IsSucceeded == true)
            {
                return new GetListSliderImageResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    listSliderImageActive = result.listSliderImageActive,
                    listSliderImageDisable = result.listSliderImageDisable
                };
            }
            else
            {
                return new GetListSliderImageResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
    }
}