using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.User;
using SSE.Common.Api.v1.Responses.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IUserBLL
    {
        Task<SignInResponse> SignIn(SigninRequest request);

        Task<CommonResponse> Config(ConfigRequest request);

        Task<RefreshAccessTokenResponse> RefreshAccessToken(RefreshAccessTokenRequest request);

        Task<CommonResponse> SignOut();

        Task<CommonResponse> UpdateLang(UpdateLangRequest request);

        Task<GetCompaniesResponse> GetCompanies();

        Task<GetUnitsResponse> GetUnits();
        Task<GetStoresResponse> GetStores(string unitId);
        Task<CommonResponse> SetCacheStore(string storeId);
        Task<bool> PushNotification(PushNotifyRequest request);
        Task<GetNotifyResponse> GetNotify(int PageIndex);
        Task<CommonResponse> UpdateNotify(string NotifyId, int Action, int Type);
        Task<CommonResponse> DeleteAccount();
        Task<GetNotifyTotalResponse> GetNotifyTotal();
        Task<EmployeeResponse> UpdateUIdUser(string uId);
        Task<GetSettingOptionsResponse> GetSettingOptions(); 
        Task<GetSettingOptionsV2Response> GetSettingOptionsV2(); 
        Task<PermissionUserResponse> GetPermissionOfUser();
        Task<PermissionUserResponse> GetPermissionOfUser2();
        Task<EmployeeResponse> GetListEmployee(int page_index, int page_count, string userCode, string keySearch, int typeAction);
    }
}