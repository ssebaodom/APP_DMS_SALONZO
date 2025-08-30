using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.User;
using SSE.Common.Api.v1.Responses.User;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.AuthenticationIdentity;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Helpers;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserDAL userDAL;
        private readonly IUserBLLService userBLLService;
        private readonly ICached cached;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;

        public UserBLL(IUserDAL userDAL,
                       IUserBLLService userBLLService,
                       ICached cached,
                       IHttpContextAccessor httpContextAccessor,
                       IConfiguration configuration)
        {
            this.configuration = configuration;
            this.userDAL = userDAL;
            this.userBLLService = userBLLService;
            this.cached = cached;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<SignInResponse> SignIn(SigninRequest request)
        {
            string server = "", dbName = "", sqlUserName = "", sqlPass = "";
            string hostUrl = this.configuration[CONFIGURATION_KEYS.HOST_URL].ToString();

            // Nếu là Bản Cho thuê/demo. Sử dụng thông tin sys db trong bảng DbSysInfo (GLOBAL_SYS database).
            // Điều Kiện: Giá trị HostUrl gửi lên sẽ bằng với giá trị HostUrl khai báo trong web config.
            if (request.HostId.Trim() == hostUrl)
            {
                var getSysDbNameResult = await userDAL.GetDatabaseInfo(request.HostId, request.UserName);

                // Kiểm tra Host Id và user Name có tồn tại, nếu có, lấy thông tin:
                // Server name, Tên Database Sys, Sql user login, Sql pass login
                if (getSysDbNameResult.IsSucceeded == false)
                {
                    return new SignInResponse
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = getSysDbNameResult.Message
                    };
                }

                server = getSysDbNameResult.ServerName;
                dbName = getSysDbNameResult.SysDbName;
                sqlUserName = getSysDbNameResult.SqlUserLogin;
                sqlPass = getSysDbNameResult.SqlPassLogin;
            }
            // Nếu là bản Khách hàng. Sử dụng thông tin sys db trong web config.
            // Điều Kiện: Giá trị HostUrl gửi lên sẽ khác với giá trị HostUrl khai báo trong web config.
            else
            {
                string sysConStr = this.configuration.GetConnectionString(CONFIGURATION_KEYS.SYS_CONNECTION_STRING);
                SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(sysConStr);

                server = connectionStringBuilder.DataSource;
                dbName = connectionStringBuilder.InitialCatalog;
                sqlUserName = connectionStringBuilder.UserID;
                sqlPass = CryptHelper.Encrypt(connectionStringBuilder.Password);
            }

            // Nếu lấy được tên Database Sys. Kiểm tra thông tin user:
            // - Tên đăng nhập và mật khẩu. Trong Database Sys.

            var signinResult = await userDAL.SignIn(server,
                                                    dbName,
                                                    sqlUserName,
                                                    sqlPass,
                                                    request.UserName,
                                                    request.Password);

            if (!signinResult.IsSucceeded)
            {
                return new SignInResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = signinResult.Message
                };
            }

           

            UserInfoDTO user = signinResult.User;
            user.HostId = request.HostId;

            UserInfoCache userInfoCache = new UserInfoCache();
            userInfoCache = MapperHelper.Map<UserInfoCache>(userInfoCache, user);
            userInfoCache.DbSysName = dbName;
            userInfoCache.ServerName = server;
            userInfoCache.SqlUserLogin = sqlUserName;
            userInfoCache.SqlPassLogin = sqlPass;
            userInfoCache.DeviceToken = request.DevideToken;
            userInfoCache.Lang = request.Language;

            string accessToken = string.Empty, refreshAccessToken = string.Empty;

            userBLLService.GetToken(userInfoCache,
                                    ref accessToken,
                                    ref refreshAccessToken);

            // Lưu thông tin người dùng vào trong cached
            userBLLService.SetCacheUser(ref userInfoCache);

            // Lưu access token để check refresh token.
            userBLLService.SetCacheAccessToken(user.HostId, user.UserName, refreshAccessToken);


            return new SignInResponse
            {
                StatusCode = StatusCodes.Status200OK,
                AccessToken = accessToken,
                RefreshToken = refreshAccessToken,
                User = user,
            };
        }

        /// <summary
        /// - Cài đặt AppDb name cho user trong cache dựa vào companyId (prop code in entity Table (sysDb)
        /// </summary>
        /// <param name="request">ConfigRequest</param>
        /// <returns>CommonResponse</returns>
        public async Task<CommonResponse> Config(ConfigRequest request)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            if (userInfoCache == null)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            userInfoCache.DbAppName = null;
            userBLLService.SetCacheUser(ref userInfoCache);

            var getAppDbNameResult = await userDAL.GetAppDbName(request.CompanyId);

            if (!getAppDbNameResult.IsSucceeded)
            {
                new CommonResponse
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            bool isSetAppDbNameSuccess = userBLLService.SetAppDbNameAsync(getAppDbNameResult.AppDbName,
                                                                          request.CompanyId,
                                                                          httpContextAccessor.HttpContext);

            if (!isSetAppDbNameSuccess)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status202Accepted
                };
            }

            return new CommonResponse
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// Lấy chuỗi Refresh Token cho client.
        /// </summary>
        /// <param name="request">RefreshAccessTokenRequest</param>
        /// <returns>RefreshAccessTokenResponse</returns>
        public Task<RefreshAccessTokenResponse> RefreshAccessToken(RefreshAccessTokenRequest request)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            if (userInfoCache == null)
            {
                return Task.FromResult(new RefreshAccessTokenResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            string keyAuthenInfo = string.Concat(CORE_CACHE_KEYS.AUTHEN, userInfoCache.HostId, "_", userInfoCache.UserName);
            AuthenInfo authenInfo = this.cached.Get<AuthenInfo>(keyAuthenInfo);

            // Kiểm tra chuỗi refresh token có còn hợp lệ.
            if (authenInfo == null)
            {
                return Task.FromResult(new RefreshAccessTokenResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = API_STRINGS.INVALID_REFRESH_TOKEN
                });
            }

            // Kiểm tra chuỗi token có tồn tại hay không.
            if (authenInfo.RefreshToken != request.RefreshToken)
            {
                return Task.FromResult(new RefreshAccessTokenResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = API_STRINGS.TOKEN_NOT_EXIST
                });
            }

            // Lấy token và access token mới
            string accessToken = string.Empty;
            string refreshToken = string.Empty;
            userBLLService.GetToken(userInfoCache, ref accessToken, ref refreshToken);

            // Lưu thông tin token vào cache.
            userBLLService.SetCacheAccessToken(userInfoCache.HostId, userInfoCache.UserName, refreshToken);

            return Task.FromResult(new RefreshAccessTokenResponse
            {
                StatusCode = StatusCodes.Status200OK,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// Đăng xuất người dùng khỏi hệ thống.
        /// Xoá thông tin user lưu trong cache.
        /// </summary>
        /// <returns>CommonResponse</returns>
        public Task<CommonResponse> SignOut()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            if (userInfoCache == null)
            {
                return Task.FromResult(new CommonResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            userBLLService.RemoveCacheUser(userInfoCache.HostId, userInfoCache.UserName);

            return Task.FromResult(new CommonResponse
            {
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// Lấy danh sách các Công ty.
        /// Sau khi Signin thành công phải gọi API này để xác định App Db.
        /// Nếu chỉ có 1 công ty (1 App Db) thì Lấy luôn giá trị App Db
        /// này gán cho User và không phải gọi api Config nữa.
        /// Nếu có nhiều hơn 1 công ty trong 1 host.
        /// Người dùng phải chọn 1 công ty trong danh sách rồi gọi api Config.
        /// </summary>
        /// <returns></returns>
        public async Task<GetCompaniesResponse> GetCompanies()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            var getCompaniesResult = await userDAL.GetCompanies(userInfoCache.HostId);

            if (!getCompaniesResult.IsSucceeded)
            {
                return new GetCompaniesResponse
                {
                    StatusCode = StatusCodes.Status202Accepted,
                    Message = API_STRINGS.GET_COMPANIES_FAILED
                };
            }

            // Nếu chỉ có 1, gán luôn AppDB cho user trong cache.
            var coms = getCompaniesResult.Companies.ToList();

            if (coms.Count == 1)
            {
                bool isSetAppDbNameSuccess = userBLLService.SetAppDbNameAsync(coms[0].AppDbName,
                                                                              coms[0].CompanyId,
                                                                              httpContextAccessor.HttpContext);

                if (!isSetAppDbNameSuccess)
                {
                    return new GetCompaniesResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = API_STRINGS.SET_APP_DBNAME_FAILED
                    };
                }
            }

            return new GetCompaniesResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Companies = getCompaniesResult.Companies
            };
        }

        /// <summary>
        /// Lấy danh sách Đơn vị cơ sở.
        /// Mỗi App Db sẽ có 1 List các Đơn vị cơ sở.
        /// </summary>
        /// <returns>List các Đơn vị cơ sở.</returns>
        public async Task<GetUnitsResponse> GetUnits()
        {
            UserInfoCache user = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            if (user == null)
            {
                return new GetUnitsResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            var getUnitsResult = await userDAL.GetUnits(user.UserId, user.Role, user.Lang);

            if (!getUnitsResult.IsSucceeded)
            {
                return new GetUnitsResponse
                {
                    StatusCode = StatusCodes.Status202Accepted,
                    Message = API_STRINGS.GET_UNITS_FAILED
                };
            }

            return new GetUnitsResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Units = getUnitsResult.Units
            };
        }

        /// <summary>
        /// Lấy danh sách các Bộ phận (cửa hàng).
        /// Phụ thuộc vào mã đơn vị cơ sở.
        /// </summary>
        /// <param name="unitId">Mã đơn vị cơ sở</param>
        /// <returns>List các cửa hàng</returns>
        public async Task<GetStoresResponse> GetStores(string unitId)
        {
            UserInfoCache user = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            if (user == null)
            {
                return new GetStoresResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            user.UnitId = unitId;
            userBLLService.SetCacheUser(ref user);

            var getStoresResult = await userDAL.GetStores(user.UserId, user.Role, unitId, user.Lang);

            if (!getStoresResult.IsSucceeded)
            {
                return new GetStoresResponse
                {
                    StatusCode = StatusCodes.Status202Accepted,
                    Message = API_STRINGS.GET_UNITS_FAILED
                };
            }

            return new GetStoresResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Stores = getStoresResult.Stores
            };
        }

        /// <summary>
        /// Cập nhật ngôn ngữ ứng dụng.
        /// </summary>
        /// <param name="request">Giá trị ngôn ngữ mới: 'e' hoặc 'v'</param>
        /// <returns></returns>
        public Task<CommonResponse> UpdateLang(UpdateLangRequest request)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            userInfoCache.Lang = request.Lang;

            userBLLService.SetCacheUser(ref userInfoCache);

            return Task.FromResult(new CommonResponse
            {
                StatusCode = StatusCodes.Status200OK
            });
        }

        public Task<CommonResponse> SetCacheStore(string storeId)
        {
            UserInfoCache user = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            if (user == null)
            {
                return Task.FromResult(new CommonResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            user.StoreId = storeId;
            userBLLService.SetCacheUser(ref user);

            return Task.FromResult(new CommonResponse
            {
                StatusCode = StatusCodes.Status200OK
            });
        }
        public async Task<GetNotifyResponse> GetNotify(int PageIndex)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            long UserId = userInfoCache.UserId;            
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await userDAL.GetNotifyList(UserId, PageIndex, Lang, Admin);
            if (!result.IsSucceeded)
            {
                return new GetNotifyResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                   
                };
            }

            return new GetNotifyResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = result.Data
            };
        }
        public async Task<CommonResponse> UpdateNotify(string NotifyId, int Action, int Type)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await userDAL.UpdateNotify(NotifyId, Action, Type, UserId, Lang, Admin);
            if (!result.IsSucceeded)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                    
                };
            }

            return new CommonResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message
            };
        }

        public async Task<bool> PushNotification(PushNotifyRequest requestNotify)
        {
            bool isSuccess = false;
            string str_data = requestNotify.Data != null ? JsonConvert.SerializeObject(requestNotify.Data) : "";
            List<string> registration_ids = new List<string>();

            
            List<string> tokens = new List<string>();
            if (requestNotify.DevideToken != null && requestNotify.DevideToken[0] != "")
            {
                tokens = requestNotify.DevideToken;
                goto Label1;
            }
                

            string userName = string.Empty;
            List<string> keyList = cached.Keys().ToList();

            foreach (string x in keyList)
            {
                if (x.Split('_').Length > 1)
                {
                    userName = x.Split('_')[1];

                    if (requestNotify.UserIds.Contains(userName))
                    {
                        registration_ids.Add(userName);
                        tokens.Add(cached.Get<UserInfoCache>(x).DeviceToken);
                    }

                }                                
                
            }

        Label1:

            string SERVER_KEY = GoogleFirbase.AUTHORIZATION_FIREBASE;
            string FireBasePushNotificationsURL = GoogleFirbase.URL_FIREBASE;

            if (requestNotify.Data == null)
            {
                requestNotify.Data = new ExpandoObject();
            }

            requestNotify.Data.title = requestNotify.Tittle;
            requestNotify.Data.body = requestNotify.Body;

            var notificationInformation = new
            {
                registration_ids = tokens,
                notification = new
                {
                    title = requestNotify.Tittle,
                    body = requestNotify.Body,
                    sound = "default"
                },
                data = new
                {
                    click_action = "FLUTTER_NOTIFICATION_CLICK",
                    payload = requestNotify.Data
                }
            };

            string jsonMessage = JsonConvert.SerializeObject(notificationInformation);
            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + SERVER_KEY);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage send_result = null;

            using (var client = new HttpClient())
            {
                send_result = await client.SendAsync(request);
                isSuccess = send_result.IsSuccessStatusCode;
            }
            if (isSuccess || !isSuccess)
            {
                await Task.Run(() => {
                    UserNotifyDTO userNotify = new UserNotifyDTO
                    {
                        UserIds = string.Join(',', requestNotify.UserIds),
                        Title = requestNotify.Tittle,
                        Body = requestNotify.Body
                    };
                    userDAL.LogUserNotify(userNotify);
                });
            }
            
            
            return isSuccess;
        }
        public async Task<GetNotifyTotalResponse> GetNotifyTotal()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await userDAL.GetNotifyTotal(UserId, Lang, Admin);
            if (!result.IsSucceeded)
            {
                return new GetNotifyTotalResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError

                };
            }

            return new GetNotifyTotalResponse
            {
                StatusCode = StatusCodes.Status200OK,
                UnreadTotal = result.UnreadTotal
            };
        }
        public async Task<EmployeeResponse> UpdateUIdUser(string uId)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            long UserId = userInfoCache.UserId;

            var result = await userDAL.UpdateUIdUser(UserId,uId);
            if (!result.IsSucceeded)
            {
                return new EmployeeResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Có lỗi xảy ra"

                };
            }

            return new EmployeeResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Update UId thành công"
            };
        }

        public async Task<GetSettingOptionsResponse> GetSettingOptions()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            string HostId = userInfoCache.HostId;

            var result = await userDAL.GetSettingOptions(HostId);
            if (!result.IsSucceeded)
            {
                return new GetSettingOptionsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Có lỗi xảy ra"

                };
            }

            return new GetSettingOptionsResponse
            {
                inStockCheck = result.inStockCheck,
                listTransaction = "",
                distanceLocationCheckIn = "300",
                StatusCode = StatusCodes.Status200OK,
                Message = "Lấy dữ liệu thành công"
            };
        }

        public async Task<GetSettingOptionsV2Response> GetSettingOptionsV2()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            string HostId = userInfoCache.HostId;

            var result = await userDAL.GetSettingOptionsV2(HostId);
            if (!result.IsSucceeded)
            {
                return new GetSettingOptionsV2Response
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Có lỗi xảy ra"

                };
            }

            return new GetSettingOptionsV2Response
            {
                listTransaction = result.listTransaction,
                masterAppSettings = result.masterAppSettings,
                listAgency = result.listAgency,
                listTypePayment = result.listTypePayment,
                listFunctionQrCode = result.listFunctionQrCode,
                listTypeDelivery = result.listTypeDelivery,
                listTypeVoucher = result.listTypeVoucher, 
                StatusCode = StatusCodes.Status200OK,
                Message = "Lấy dữ liệu thành công"
            };
        }

        public async Task<PermissionUserResponse> GetPermissionOfUser()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            string userName = userInfoCache.UserName;

            var result = await userDAL.GetUserPermission(userName);
            if (!result.IsSucceeded)
            {
                return new PermissionUserResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Có lỗi xảy ra"

                };
            }

            return new PermissionUserResponse
            {
                UserPermission = result.UserPermission,
                StatusCode = StatusCodes.Status200OK,
                Message = "Lấy dữ liệu thành công"
            };
        }
        
        public async Task<PermissionUserResponse> GetPermissionOfUser2()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            string userName = userInfoCache.UserName;

            var result = await userDAL.GetUserPermission2(userName);
            if (!result.IsSucceeded)
            {
                return new PermissionUserResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Có lỗi xảy ra"

                };
            }

            return new PermissionUserResponse
            {
                UserPermission = result.UserPermission,
                UserPermissionAccount = result.UserPermissionAccount,
                StatusCode = StatusCodes.Status200OK,
                Message = "Lấy dữ liệu thành công"
            };
        }

        public async Task<CommonResponse> DeleteAccount()
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);

            long UserId = userInfoCache.UserId;

            var result = await userDAL.DeleteAccount( UserId);
            if (!result.IsSucceeded)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message

                };
            }

            return new CommonResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message
            };
        }

        public async Task<EmployeeResponse> GetListEmployee(int page_index, int page_count, string userCode, string keySearch, int typeAction)
        {
            UserInfoCache userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
             
            string unitID = userInfoCache.UnitId;

            var result = await this.userDAL.GetListEmployee(userCode, unitID, page_index,page_count,keySearch, typeAction);

            if (result.IsSucceeded == true)
            {
                return new EmployeeResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    totalOrder = result.totalOrder,
                    listEmployee = result.listEmployee,
                    totalPage = result.totalPage
                };
            }
            else
            {
                return new EmployeeResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
    }
}