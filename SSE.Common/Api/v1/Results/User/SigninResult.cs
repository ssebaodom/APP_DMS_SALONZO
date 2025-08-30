using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Results.User
{
    public class SigninResult : BaseResult
    {
        public UserInfoDTO User { get; set; }
        //public UserPermissionDTO UserPermission { get; set; }
    }
}