using Microsoft.AspNetCore.Http;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Requests.Report;
using SSE.Common.Api.v1.Responses.Report;
using SSE.Common.Api.v1.Results.Report;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Caches;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Threading.Tasks;
using SSE.Common.Api.v1.Common;

namespace SSE.Business.Api.v1.Implements
{
    internal class ReportBLL : IReportBLL
    {
        private readonly IReportDAL reportDAL;
        private UserInfoCache userInfoCache;
        private readonly ICached cached;
        public ReportBLL(IReportDAL reportDAL,
                       IUserBLLService userBLLService,
                       ICached cached,
                       IHttpContextAccessor httpContextAccessor)
        {
            this.reportDAL = reportDAL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            this.cached = cached;
        }

        public async Task<GetReportListResponse> GetReportList(GetReportListRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.reportDAL.GetReportList(request);

            if (result.IsSucceeded == true)
            {
                return new GetReportListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.ReportList
                };
            }
            else
            {
                return new GetReportListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ReportLayoutResponse> GetReportLayout(ReportLayoutRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.reportDAL.GetReportLayout(request);

            if (result.IsSucceeded == true)
            {
                return new ReportLayoutResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.ReportLayout
                };
            }
            else
            {
                return new ReportLayoutResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ReportFieldLookupResponse> GetReportFieldLookup(ReportFieldLookupRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.reportDAL.GetReportFieldLookup(request);

            if (result.IsSucceeded == true)
            {
                return new ReportFieldLookupResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    data = result.reportFields,
                    PageIndex = request.PageIndex,
                    TotalCount = result.TotalCount
                };
            }
            else
            {
                return new ReportFieldLookupResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ReportExecuteResponse> GetReportResult(ReportExecuteRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.reportDAL.GetReportResult(request);

            if (result.IsSucceeded == true)
            {
                return new ReportExecuteResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Values = result.Values,
                    HeaderDesc = result.HeaderDesc
                };
            }
            else
            {
                return new ReportExecuteResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
        public CommonResponse ResetCache(string cachekey)
        {
            bool successed = cached.Remove(cachekey);

            if (successed == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                   
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}