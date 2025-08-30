using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.DisCountRequest;
using SSE.Common.Api.v1.Responses.DisCount;
using SSE.Core.Services.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/discount")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DiscountController : ControllerBase
    {
        private readonly IDisCountBLL discountBLL;

        public DiscountController(IDisCountBLL discountBLL)
        {
            this.discountBLL = discountBLL;
        }

        [Route("checkdiscount")]
        [HttpPost]
        public async Task<DisCountResponse> GetDisCount(DisCountRequest request)
        {
            return await this.discountBLL.GetDisCount(request);
        }
        [Route("get-discount-when-update")]
        [HttpPost]
        public async Task<DisCountResponse> GetDisCountWhenUpdate(DisCountWhenUpdateRequest request)
        {
            return await this.discountBLL.GetDisCountWhenUpdate(request);
        }

        [AllowAnonymous]
        [Route("get")]
        [HttpGet]
        public string Check()
        {
            //string s = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            return "1.0.60";
        }

        [AllowAnonymous]
        [Route("genpass")]
        [HttpGet]
        public string GenPass(string pass)
        {
            string s = CryptHelper.Encrypt(pass);
            return s;
        }

        [AllowAnonymous]
        [Route("get2")]
        [HttpGet]
        public string Check2()
        {
            string s = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(s + "Value/df.json");
            string str = r.ReadToEnd();
            JObject json = JObject.Parse(str);
            return str;
        }

        /// <summary>
        /// 19/12/2022 
        /// V2 Tổng quát chương trình khuyến mại
        /// </summary>
        /// <creater name="tiennq"></creater>
        /// <returns>list discount</returns>
        [Route("apply-discount-v2")]
        [HttpPost]
        public async Task<DisCountApplyResponse> ApplyDiscount(DisCountItemRequest request)
        {
            return await this.discountBLL.ApplyDiscount(request);
        }
    }
}
