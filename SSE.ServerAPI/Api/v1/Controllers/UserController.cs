using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.User;
using SSE.Common.Api.v1.Responses.User;
using SSE.Common.Constants.v1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL userBLL;

        public UserController(IUserBLL userBLL)
        {
            this.userBLL = userBLL;
        }

        [AllowAnonymous]
        [Route("signin")]
        [HttpPost]
        public async Task<SignInResponse> Signin(SigninRequest request)
        {
            return await userBLL.SignIn(request);
        }

        [Route("config")]
        [HttpPost]
        public async Task<CommonResponse> Config(ConfigRequest request)
        {
            return await userBLL.Config(request);
        }

        [Route("refresh-token")]
        [HttpPost]
        public async Task<RefreshAccessTokenResponse> RefreshToken(RefreshAccessTokenRequest request)
        {
            return await this.userBLL.RefreshAccessToken(request);
        }

        [Route("signout")]
        [HttpPost]
        public async Task<CommonResponse> SignOut()
        {
            return await userBLL.SignOut();
        }

        [Route("companies")]
        [HttpGet]
        public async Task<GetCompaniesResponse> GetCompanies()
        {
            return await userBLL.GetCompanies();
        }

        [Route("units")]
        [HttpGet]
        public async Task<GetUnitsResponse> GetUnits()
        {
            return await userBLL.GetUnits();
        }

        [Route("stores")]
        [HttpGet]
        public async Task<GetStoresResponse> GeStores(string unitId)
        {
            return await userBLL.GetStores(unitId);
        }

        [Route("lang")]
        [HttpPost]
        public async Task<CommonResponse> UpdateLang(UpdateLangRequest request)
        {
            return await userBLL.UpdateLang(request);
        }

        [Route("stores/cached")]
        [HttpGet]
        public async Task<CommonResponse> SetCacheStore(string storeId)
        {
            return await userBLL.SetCacheStore(storeId);
        }
        [Route("push-notify")]
        [HttpPost]
        public async Task<CommonResponse> PushNotify(PushNotifyRequest request)
        {           
            bool successed = await userBLL.PushNotification(request);
            if (successed)
                return new CommonResponse { StatusCode = 200 };
            else
                return new CommonResponse { StatusCode = 500 };
        }
        [Route("get-notify-list")]
        [HttpGet]
        public async Task<GetNotifyResponse> GetNotify(int PageIndex)
        {
            return await userBLL.GetNotify(PageIndex);
        }
        [Route("update-notify")]
        [HttpGet]
        public async Task<CommonResponse> UpdateNotify(string NotifyId, NotifyAction Action, NotifyType Type)
        {
            return await userBLL.UpdateNotify(NotifyId, (int)Action, (int)Type);
        }
        [Route("get-notify-total")]
        [HttpGet]
        public async Task<GetNotifyTotalResponse> GetNotifyTotal()
        {
            return await userBLL.GetNotifyTotal();
        }
        [Route("update-uid-user")]
        [HttpPost]
        public async Task<EmployeeResponse> UpdateUIdUser(string UIdUser)
        {
            return await userBLL.UpdateUIdUser(UIdUser);
        }
        [Route("get-setting-options")]
        [HttpGet]
        public async Task<GetSettingOptionsResponse> GetSettingOptions()
        {
            return await userBLL.GetSettingOptions();
        }
        [Route("get-setting-options-v2")]
        [HttpGet]
        public async Task<GetSettingOptionsV2Response> GetSettingOptionsV2()
        {
            return await userBLL.GetSettingOptionsV2();
        }
        [Route("get-permission-user")]
        [HttpGet]
        public async Task<PermissionUserResponse> GetPermissionOfUser()
        {
            return await userBLL.GetPermissionOfUser();
        }
        
        [Route("get-permission-user-v2")]
        [HttpGet]
        public async Task<PermissionUserResponse> GetPermissionOfUser2()
        {
            return await userBLL.GetPermissionOfUser2();
        }
        [Route("delete-account")]
        [HttpGet]
        public async Task<CommonResponse> DeleteAccount()
        {
            return await userBLL.DeleteAccount();
        }
        [Route("get-list-employee")]
        [HttpGet]
        public async Task<EmployeeResponse> GetListEmployee(int page_index, int page_count, string userCode, string keySearch, int typeAction)
        {
            return await userBLL.GetListEmployee(page_index, page_count, userCode,keySearch,typeAction);
        }
    }
}