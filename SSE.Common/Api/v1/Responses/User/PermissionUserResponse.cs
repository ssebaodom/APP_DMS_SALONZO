using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.User
{
    public class PermissionUserResponse : BaseResponse
    {
        public List<UserPermissionDTO> UserPermission { get; set; }
        public List<UserPermissionAccountDTO> UserPermissionAccount { get; set; }
    }
}