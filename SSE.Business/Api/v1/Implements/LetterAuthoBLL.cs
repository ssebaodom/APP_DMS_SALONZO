using Microsoft.AspNetCore.Http;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Requests.LetterAutho;
using SSE.Common.Api.v1.Responses.LetterAutho;
using SSE.Core.Common.Entities;
using SSE.DataAccess.Api.v1.Interfaces;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    public class LetterAuthoBLL : ILetterAuthoBLL
    {
        private readonly ILetterAuthoDAL letterAuthoDAL;
        private UserInfoCache userInfoCache;

        public LetterAuthoBLL(ILetterAuthoDAL letterAuthoDAL,
                       IUserBLLService userBLLService,
                       IHttpContextAccessor httpContextAccessor)
        {
            this.letterAuthoDAL = letterAuthoDAL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }

        public async Task<LetterAuDisplayResponse> LetterAuDisplay(LetterAuDisplayResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.letterAuthoDAL.LetterAuDisplay(request);
            if (result.IsSucceeded == true)
                return new LetterAuDisplayResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data
                };
            else
                return new LetterAuDisplayResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<LetterListResponse> LetterList(LetterListResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.letterAuthoDAL.LetterList(request);
            if (result.IsSucceeded == true)
                return new LetterListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Values = result.Values,
                    HeaderDesc = result.HeaderDesc,
                    Status = result.Status,
                    PageIndex = request.PageIndex,
                    TotalCount = result.TotalCount
                };
            else
                return new LetterListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<LetterApproResponse> LetterApproval(LetterApproResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.letterAuthoDAL.LetterApproval(request);
            if (result.IsSucceeded == true)
                return new LetterApproResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            else
                return new LetterApproResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<LetterDetailResponse> LetterDetail(LetterDetailResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.letterAuthoDAL.LetterDetail(request);
            if (result.IsSucceeded == true)
                return new LetterDetailResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    MainValues = result.MainValues,
                    MainHeaderDesc = result.MainHeaderDesc,
                    DetailValues = result.DetailValues,
                    DetailHeaderDesc = result.DetailHeaderDesc
                };
            else
                return new LetterDetailResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<LetterDetailResponse2> LetterDetail2(LetterDetailResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.letterAuthoDAL.LetterDetail2(request);
            if (result.IsSucceeded == true)
                return new LetterDetailResponse2
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data,
                    listValuesFile = result.listValuesFile
                };
            else
                return new LetterDetailResponse2
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
    }
}