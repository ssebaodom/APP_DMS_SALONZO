using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SSE.Core.Common.Attributes
{
    /// <summary>
    /// Dùng attribute này để bắt lỗi dữ liệu gửi lên từ client cho api, khi mà cố tình cheat để vượt qua validate bên đó.
    /// </summary>
    public sealed class ApiControllerFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}