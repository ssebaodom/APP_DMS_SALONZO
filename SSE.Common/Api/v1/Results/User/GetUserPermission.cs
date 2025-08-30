using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.User
{
    public class UserPermissionResult : BaseResult
    {
        public List<UserPermissionDTO> UserPermission { get; set; }
        public List<UserPermissionAccountDTO> UserPermissionAccount { get; set; }
        //public dynamic UserPermission { get; set; }
    }
}