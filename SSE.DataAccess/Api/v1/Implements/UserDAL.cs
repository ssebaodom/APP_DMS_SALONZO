using Dapper;
using Microsoft.Extensions.Configuration;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Results.User;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Common.Entities.v1;
using SSE.Core.Common.Constants;
using SSE.Core.Services.Dapper;
using SSE.Core.Services.Helpers;
using SSE.Core.UoW;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.DataAccess.Context;
using SSE.DataAccess.Services.v1.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class UserDAL : IUserDAL
    {
        private readonly IConfiguration configuration;
        private readonly IDapperService dapperService;
        private readonly IUnitOfWork<GlobalDbContext> unitOfWork;
        private readonly IUserDALService userDALService;

        public UserDAL(IConfiguration configuration,
                       IDapperService dapperService,
                       IUnitOfWork<GlobalDbContext> unitOfWork,
                       IUserDALService userDALService)
        {
            this.configuration = configuration;
            this.dapperService = dapperService;
            this.unitOfWork = unitOfWork;
            this.userDALService = userDALService;
        }

        public async Task<GetAppDbNameResult> GetAppDbName(string companyId)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@companyId", companyId);

            string appdbName = await dapperService.ExecuteScalarAsync<string>("app_get_app_db_name", dynamicParameter);

            return new GetAppDbNameResult
            {
                IsSucceeded = !string.IsNullOrWhiteSpace(appdbName),
                AppDbName = appdbName
            };
        }

        public async Task<GetCompaniesResult> GetCompanies(string hostId)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@host_id", hostId);

            IEnumerable<CompanyDTO> companies = await dapperService.QueryAsync<CompanyDTO>("app_get_companies", dynamicParameter);

            if (companies == null || companies.AsList<CompanyDTO>().Count == 0)
            {
                return new GetCompaniesResult
                {
                    IsSucceeded = false
                };
            }

            return new GetCompaniesResult
            {
                IsSucceeded = true,
                Companies = companies
            };
        }

        public async Task<GetStoresResult> GetStores(long userId, int role, string maDvcs, string lang = "v")
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@user_id", userId);
            dynamicParameter.Add("@role", role);
            dynamicParameter.Add("@ma_dvcs", maDvcs);
            dynamicParameter.Add("@lang", lang);

            IEnumerable<StoreDTO> stores = await dapperService.QueryAsync<StoreDTO>("app_get_stores", dynamicParameter);

            if (stores == null || stores.AsList<StoreDTO>().Count == 0)
            {
                return new GetStoresResult
                {
                    IsSucceeded = false
                };
            }

            return new GetStoresResult
            {
                IsSucceeded = true,
                Stores = stores
            };
        }

        public async Task<GetSysDbNameResult> GetDatabaseInfo(string hostId,
                                                           string userName)
        {
            DbSysInfo userinfo = await unitOfWork.
                                       GetRepository<DbSysInfo>().
                                       FirstOrDefaultAsync(d => //d.UserName == userName &&
                                                                d.HostId == hostId);
            if (userinfo == null)
            {
                return new GetSysDbNameResult
                {
                    IsSucceeded = false,
                    Message = API_STRINGS.USER_INFO_INVALID
                };
            }

            return new GetSysDbNameResult
            {
                IsSucceeded = true,
                SysDbName = userinfo.SysDbName,
                ServerName = userinfo.ServerName,
                SqlUserLogin = userinfo.SqlUserLogin,
                SqlPassLogin = userinfo.SqlPassLogin
            };
        }

        public async Task<GetUnitsResult> GetUnits(long userId, int role, string lang = "v")
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@user_id", userId);
            dynamicParameter.Add("@role", role);
            dynamicParameter.Add("@lang", lang);

            IEnumerable<UnitDTO> units = await dapperService.QueryAsync<UnitDTO>("app_get_units", dynamicParameter);

            if (units == null || units.AsList<UnitDTO>().Count == 0)
            {
                return new GetUnitsResult
                {
                    IsSucceeded = false
                };
            }

            return new GetUnitsResult
            {
                IsSucceeded = true,
                Units = units
            };
        }

        public async Task<SigninResult> SignIn(string ServerName,
                                               string sysDbName,
                                               string SqlUserLogin,
                                               string SqlPassLogin,
                                               string userName,
                                               string passWord)
        {
            string conStr = this.configuration.GetConnectionString(CONFIGURATION_KEYS.CONNECTION_STRING_FORMAT);
            conStr = string.Format(conStr, ServerName, sysDbName,
                                           SqlUserLogin, CryptHelper.Decrypt(SqlPassLogin));

            // Đặt lại giá trị do chuỗi kết nối có sự thay đổi.
            // Khi gọi phương thức này, chuỗi kết nối của Dapper đang có giá trị trỏ tới Db Global.
            // Do đó phải đổi lại sang chuỗi kết nối trỏ tới Db Sys.
            dapperService.SetNewConnection(conStr);
            userDALService.SetConnectionString(conStr);

            UserInfoDTO user = await userDALService.GetUserByName(userName);

            if (user == null)
            {
                return new SigninResult
                {
                    IsSucceeded = false,
                    Message = API_STRINGS.USER_NAME_INVALID
                };
            }

            if (user != null && user.Active == 0)
            {
                return new SigninResult
                {
                    IsSucceeded = false,
                    Message = API_STRINGS.USER_NAME_LOCK
                };
            }

           
            // Kiểm tra mật khẩu mã hóa có chính xác.
            // Mật khẩu được mã hoá bằng MD5. Có 2 cách để kiểm tra mật khẩu.
            // Cách 1:
            // Thay đổi phần tử Đầu tiên và phần tử Cuối cùng của chuỗi mã hoá giá trị gốc bằng
            // phần tử Đầu tiên và phần tử cuối cùng trong giá trị của thuộc tính
            // keys trong bảng userinfo2. So sánh giá trị thu được với mật khẩu mã hoá lưu
            // trong database.
            // Cách 2:
            // Đọc chuỗi mật khẩu mã hoá lưu trong database, Thay đổi phần tử Đầu tiên và
            // phần tử Cuối cùng của chuỗi bằng phần tử Thứ 3 và phần tử thứ 2. (index from 1)
            // trong giá trị của thuộc tính keys trong bảng userinfo2.
            // So sánh giá trị thu được với chuỗi mã hoá MD5 giá trị mật khẩu gốc.

            passWord = EncodeHelper.Encode(passWord);
            //"99a395c2e8d91c123ef3fc00fc632cdf"
            //"99a395c2e8d91c123ef3fc00fc632cdf"
            //"ae67efebccce81d3a58af33fc2ccd774"
            //
            //Key : "9f90e4" - P-TEAM
            // PASS: "49a395c2e8d91c123ef3fc00fc632cd9"
            string keys = user.Keys;
            string encryptPass = user.PassWord.Substring(1, user.PassWord.Length - 2);
            encryptPass = string.Concat(keys[2], encryptPass, keys[1]);

            if (!passWord.Equals(encryptPass))
            {
                return new SigninResult
                {
                    IsSucceeded = false,
                    Message = API_STRINGS.USER_NAME_INVALID
                };
            }

            // Nếu mật khẩu chính xác. Trả về user.
            return new SigninResult
            {
                IsSucceeded = true,
                User = user
            };
        }

        public async Task<GetNotifyResult> GetNotifyList(long UserId, int PageIndex, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
           
            parameters.Add("@UserId", UserId);
            parameters.Add("@PageIndex", PageIndex);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            var data = await dapperService.QueryAsync<UserNotifyDTO>("app_Get_User_Notify", parameters);

            if (data == null)
            {
                return new GetNotifyResult
                {
                    IsSucceeded = false
                };
            }

            return new GetNotifyResult
            {
                IsSucceeded = true,
                Data = data
            };
        }
        public async Task<CommonResult> UpdateNotify(string NotifyId, int Action, int Type, long UserId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", UserId);
            parameters.Add("@NotifyId", NotifyId);
            parameters.Add("@Action", Action);
            parameters.Add("@Type", Type);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            string result = await dapperService.ExecuteScalarAsync<string>("app_Update_User_Notify", parameters);

            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new CommonResult
                {
                    IsSucceeded = true,
                    Message = message
                };
            }

            return new CommonResult
            {
                IsSucceeded = false,
                Message = message
            };
        }
        public async Task<string> LogUserNotify(UserNotifyDTO data)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<UserNotifyDTO>(data);
            return await dapperService.ExecuteScalarAsync<string>("app_log_user_notify", parameters);
        }

        public async Task<GetNotifyTotalResult> GetNotifyTotal(long UserId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", UserId);
          
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            int data = await dapperService.ExecuteScalarAsync<int>("app_GetNotifyTotal", parameters);          

            return new GetNotifyTotalResult
            {
                IsSucceeded = true,
                UnreadTotal = data
            };
        }

        public async Task<UpdateUIdUserResult> UpdateUIdUser(long UserId, string UId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", UserId);

            parameters.Add("@uId", UId);

            int data = await dapperService.ExecuteScalarAsync<int>("app_update_UID_User", parameters);

            return new UpdateUIdUserResult
            {
                IsSucceeded = true
            };
        }

        public async Task<UserPermissionResult> GetUserPermission(string userName) {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@userName", userName);
            string ProceName = "[GetPermissionOfUser]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);

            List<UserPermissionDTO> userPermissions = gridReader.Read<UserPermissionDTO>().ToList();

            return new UserPermissionResult
            {
                IsSucceeded = true,
                UserPermission = userPermissions
            };
        }
        
        public async Task<UserPermissionResult> GetUserPermission2(string userName) {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@userName", userName);
            string ProceName = "[GetPermissionOfUser_v2]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);

            List<UserPermissionDTO> userPermissions = gridReader.Read<UserPermissionDTO>().ToList();
            List<UserPermissionAccountDTO> userPermissionsAccount = gridReader.Read<UserPermissionAccountDTO>().ToList();

            return new UserPermissionResult
            {
                IsSucceeded = true,
                UserPermission = userPermissions,
                UserPermissionAccount = userPermissionsAccount
            };
        }

        public async Task<GetSettingOptionsResult> GetSettingOptions(string hotId)
        {
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@hotId", hotId);
                string ProceName = "[GetSettingOptions]";
                GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);
                dynamic inStockCheck = gridReader.Read<dynamic>();
                List<dynamic> listTransaction = gridReader.Read<dynamic>().ToList();
                dynamic distanceLocationCheckIn = gridReader.ReadFirst();
                return new GetSettingOptionsResult
                {
                    IsSucceeded = true,
                    inStockCheck = inStockCheck,
                    //listTransaction = listTransaction,
                    distanceLocationCheckIn = distanceLocationCheckIn
                };
            }
        }

        public async Task<GetSettingOptionsV2Result> GetSettingOptionsV2(string hotId)
        {
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@hotId", hotId);
                string ProceName = "[GetSettingOptionsV2]";
                GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);
               
                List<dynamic> listTransaction = gridReader.Read<dynamic>().ToList();
                List<AppSettingsDTO> listSettings = gridReader.Read<AppSettingsDTO>().ToList();
                List<dynamic> listAgency = gridReader.Read<dynamic>().ToList();
                List<dynamic> listTypePayment = gridReader.Read<dynamic>().ToList();
                List<dynamic> listFunctionQrCode = gridReader.Read<dynamic>().ToList();
                List<dynamic> listTypeDelivery = gridReader.Read<dynamic>().ToList();
                List<dynamic> listTypeVoucher = gridReader.Read<dynamic>().ToList();

                AppSettingsDTO master = new AppSettingsDTO();

                foreach (AppSettingsDTO item in listSettings)
                {
                    master = item;
                }

                return new GetSettingOptionsV2Result
                {
                    IsSucceeded = true,
                    masterAppSettings = master,
                    listTransaction = listTransaction,
                    listAgency = listAgency,
                    listTypePayment = listTypePayment,
                    listFunctionQrCode = listFunctionQrCode,
                    listTypeDelivery = listTypeDelivery,
                    listTypeVoucher = listTypeVoucher,
                };
            }
        }

        public async Task<CommonResult> DeleteAccount(long UserId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@userId", UserId);

            string result = await dapperService.ExecuteScalarAsync<string>("app_Delete_Account", parameters);

            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new CommonResult
                {
                    IsSucceeded = true,
                    Message = message
                };
            }

            return new CommonResult
            {
                IsSucceeded = false,
                Message = message
            };
        }

        public async Task<EmployeeResult> GetListEmployee(string userID,string unitID, int page_index, int page_count, string keySearch, int typeAction)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Unit", unitID);
            parameters.Add("@UserID", userID);
            parameters.Add("@TypeAction ", typeAction);
            parameters.Add("@KeyWord", keySearch);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_managed_staff", parameters);
         
            dynamic data = gridReader.Read<dynamic>();
            dynamic data2 = gridReader.Read<dynamic>();
            int TotalCount = gridReader.ReadFirst<int>();

            if (gridReader == null)
            {
                return new EmployeeResult
                {
                    IsSucceeded = false
                };
            }

            return new EmployeeResult
            {
                IsSucceeded = true,
                totalOrder = data,
                listEmployee = data2,
                totalPage = TotalCount
            };
        }
    }
}