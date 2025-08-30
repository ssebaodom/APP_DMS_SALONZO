using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Results.User;
using SSE.Common.DTO.v1;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IUserDAL
    {
        Task<GetSysDbNameResult> GetDatabaseInfo(string hostId,
                                                 string userName);

        Task<GetAppDbNameResult> GetAppDbName(string companyId);

        Task<SigninResult> SignIn(string ServerName,
                                  string sysDbName,
                                  string SqlUserLogin,
                                  string SqlPassLogin,
                                  string userName,
                                  string passWord);

        Task<GetCompaniesResult> GetCompanies(string hostId);

        Task<GetUnitsResult> GetUnits(long userId,
                                      int role,
                                      string lang);

        Task<GetStoresResult> GetStores(long userId,
                                        int role,
                                        string maDvcs,
                                        string lang);
        Task<GetNotifyResult> GetNotifyList(long UserId, int PageIndex, string Lang, int Admin);
        Task<CommonResult> UpdateNotify(string NotifyId, int Action, int Type, long UserId, string Lang, int Admin);
        Task<string> LogUserNotify(UserNotifyDTO data);
        Task<GetNotifyTotalResult> GetNotifyTotal(long UserId, string Lang, int Admin);
        Task<UpdateUIdUserResult> UpdateUIdUser(long UserId, string UId);
        Task<UserPermissionResult> GetUserPermission(string userName);
        Task<UserPermissionResult> GetUserPermission2(string userName);
        Task<GetSettingOptionsResult> GetSettingOptions(string hostId);
        Task<GetSettingOptionsV2Result> GetSettingOptionsV2(string hostId);
        Task<CommonResult> DeleteAccount(long UserId);
        Task<EmployeeResult> GetListEmployee( string UserId, string unitID, int page_index, int page_count, string keySearch, int typeAction);
    }
}