using Microsoft.AspNetCore.Mvc;
using SSE.Common.Constants.v1;

namespace SSE.Api.v1.Base
{
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        [Route("api/check-connect")]
        [HttpGet]
        public IActionResult CheckConnect()
        {
            return Ok(API_STRINGS.SERVER_CONNECTED);
        }
    }
}