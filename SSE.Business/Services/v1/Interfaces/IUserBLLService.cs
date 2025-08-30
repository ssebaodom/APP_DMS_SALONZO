using Microsoft.AspNetCore.Http;
using SSE.Core.Common.Entities;

namespace SSE.Business.Services.v1.Interfaces
{
    public interface IUserBLLService
    {
        public void GetToken(UserInfoCache user, ref string token, ref string refreshToken);

        public UserInfoCache GetUser(string hostId,
                                     string userName,
                                     bool isRefresh = false);

        public UserInfoCache GetUserFromContext(HttpContext httpContext);

        public void SetCacheUser(ref UserInfoCache user);

        public void RemoveCacheUser(string hostId, string userName);

        public void SetCacheAccessToken(string hostId,
                                        string userName,
                                        string refreshAccessToken);

        public bool SetAppDbNameAsync(string appDbName, string companyId, HttpContext httpContext);

    }
}