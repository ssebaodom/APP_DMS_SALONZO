using Microsoft.AspNetCore.Authorization;
using System;

namespace SSE.Core.Common.Attributes
{
    /// <summary>
    /// Sử dụng để check quyền
    /// </summary>
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params string[] roles) : base()
        {
            if (roles != null && roles.Length > 0)
            {
                if (roles.Length == 1)
                {
                    Roles = roles[0];
                }
                else
                {
                    Roles = String.Join(",", roles);
                }
            }
        }
    }
}