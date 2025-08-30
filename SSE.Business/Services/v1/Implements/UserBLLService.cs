using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SSE.Business.Services.v1.Interfaces;
using SSE.Core.AuthenticationIdentity;
using SSE.Core.Common.Constant;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Entities;
using SSE.Core.Common.Enums;
using SSE.Core.Common.Factories;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Helpers;
using SSE.Core.Services.JwT;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.DataAccess.Services.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SSE.Business.Services.v1.Implements
{
    public class UserBLLService : IUserBLLService
    {
        private ICached cached;
        private readonly TokenFactory tokenFactory;
        private readonly IConfiguration configuration;
        private readonly IUserDAL UserDAL;
        private readonly IUserDALService userDALService;

        public UserBLLService(
                IUserDAL UserDAL,
                IUserDALService userDALService,
                ICached cached,
                TokenFactory tokenFactory,
                IConfiguration configuration)
        {
            this.UserDAL = UserDAL;
            this.userDALService = userDALService;
            this.cached = cached;
            this.tokenFactory = tokenFactory;
            this.configuration = configuration;
        }

        /// <summary>
        /// Lấy chuỗi token và refresh token cho client.
        /// </summary>
        /// <param name="user">UserInfoCache</param>
        /// <param name="token">JwT token</param>
        /// <param name="refreshToken">Refresh token</param>
        public void GetToken(UserInfoCache user,
                             ref string token,
                             ref string refreshToken)
        {
            JwtEntity jwtEntity = new JwtEntity
            {
                UserId = user.UserId.ToString(),
                UserName = user.UserName,
                HostId = user.HostId,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            token = this.tokenFactory.createToken(TokenTypes.AccessToken, jwtEntity);
            refreshToken = this.tokenFactory.createToken(TokenTypes.RefreshAccessToken);
        }

        /// <summary>
        /// Lưu object User vào cached.
        /// </summary>
        /// <param name="user">UserInfoCache</param>
        public void SetCacheUser(ref UserInfoCache userInfoCache)
        {
            string keyUserInfo = string.Concat(CORE_CACHE_KEYS.ACCOUNT,
                                               userInfoCache.HostId, "_",
                                               userInfoCache.UserName);

            this.cached.Set(keyUserInfo, userInfoCache);
        }

        /// <summary>
        /// Xoá user lưu trong Cached.
        /// </summary>
        /// <param name="hostId">hostId của user cần xoá.</param>
        /// <param name="userName">userName của object cần xoá.</param>
        public void RemoveCacheUser(string hostId, string userName)
        {
            string keyUserInfo = string.Concat(CORE_CACHE_KEYS.ACCOUNT, hostId, "_", userName);
            string keyAuthenInfo = string.Concat(CORE_CACHE_KEYS.AUTHEN, hostId, "_", userName);
            this.cached.Remove(keyAuthenInfo);
            this.cached.Remove(keyUserInfo);
        }

        /// <summary>
        /// Lấy user object lưu trong cache. Trường hợp trong cache chưa có hoặc yêu cầu
        /// refresh user trong cache (isRefresh = true) thì lấy user trong database rồi
        /// lưu lại vào cache.
        /// </summary>
        /// <param name="hostId">hostId của user.</param>
        /// <param name="userName">Tên đăng nhập.</param>
        /// <param name="isRefresh">Làm mới user trong cache hay không.</param>
        /// <returns></returns>
        public UserInfoCache GetUser(string hostId,
                                     string userName,
                                     bool isRefresh = false)
        {
            string keyUserInfo = string.Concat(CORE_CACHE_KEYS.ACCOUNT, hostId, "_", userName);
            UserInfoCache userInfoCache = this.cached.Get<UserInfoCache>(keyUserInfo);

            if (userInfoCache == null || isRefresh)
            {
                var user = this.userDALService.GetUserByName(userName).Result;

                if (user != null)
                {
                    userInfoCache = MapperHelper.Map<UserInfoCache>(userInfoCache, user);
                    SetCacheUser(ref userInfoCache);
                }
            }

            return userInfoCache;
        }

        /// <summary>
        /// Lưu refresh accesstoken vào trong cache.
        /// </summary>
        /// <param name="hostId">hostId của user.</param>
        /// <param name="userName">userName</param>
        /// <param name="refreshAccessToken">refreshAccessToken</param>
        public void SetCacheAccessToken(string hostId,
                                        string userName,
                                        string refreshAccessToken)
        {
            string keyAuthenInfo = string.Concat(CORE_CACHE_KEYS.AUTHEN, hostId, "_", userName);
            AuthenInfo authenInfo = new AuthenInfo
            {
                RefreshToken = refreshAccessToken,
                HostId = hostId,
                UserName = userName
            };

            int expiredAuthenInfo = this.configuration.GetValue<int>(string.Concat(CONFIGURATION_KEYS.JWT, ":", CONFIGURATION_KEYS.JWT_REFRESHTOKEN_EXPIRE_HOURS));
            this.cached.Set<AuthenInfo>(keyAuthenInfo, authenInfo, TimeSpan.FromHours(expiredAuthenInfo));
        }

        public bool SetAppDbNameAsync(string appDbName, string companyId, HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity.IsAuthenticated)
            {
                IEnumerable<Claim> claims = identity.Claims;

                string hostId = claims.FirstOrDefault(c => c.Type == CLAIM_NAMES.HOSTID).Value;
                string userName = claims.FirstOrDefault(c => c.Type == CLAIM_NAMES.USERNAME).Value;

                var user = cached.Get<UserInfoCache>(string.Concat(CORE_CACHE_KEYS.ACCOUNT, hostId, "_", userName));
                user.CompanyId = companyId;
                user.DbAppName = appDbName;

                SetCacheUser(ref user);

                return true;
            }


            return false;
        }

        public UserInfoCache GetUserFromContext(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                var identity = httpContext.User.Identity as ClaimsIdentity;

                if (identity.IsAuthenticated)
                {
                    IEnumerable<Claim> claims = identity.Claims;

                    string hostId = claims.FirstOrDefault(c => c.Type == CLAIM_NAMES.HOSTID).Value;
                    string userName = claims.FirstOrDefault(c => c.Type == CLAIM_NAMES.USERNAME).Value;

                    return GetUser(hostId, userName);
                }
            }
            return null;
        }

    }
}