using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.HR;
using SSE.Common.Api.v1.Requests.Todos;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/hr")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HRController : ControllerBase
    {       
        private readonly IHRBLL _hrBLL;

        public HRController(IHRBLL hrBLL)
        {
            _hrBLL = hrBLL;
           
        }

        [Route("history-list-leave-letter")]
        [HttpGet]
        public async Task<DynamicResponse> GetListLeaveLetter(string status, string dateFrom, string dateTo, int page_index, int page_count)
        {
            return await _hrBLL.GetListLeaveLetter(status, dateFrom, dateTo, page_index, page_count);
        }

        [Route("create-leave-letter")]
        [HttpPost]
        public async Task<CommonResponse> CreateLeaveLetter(CreateLeaveLetterRequest request)
        {
            return await this._hrBLL.CreateLeaveLetter(request);
        }
        
        [Route("cancel-leave-letter")]
        [HttpDelete]
        public async Task<CommonResponse> CancelLeaveLetter(string sctRec, string stt)
        {
            return await this._hrBLL.CancelLeaveLetter(sctRec, stt);
        }

        [AllowAnonymous]
        [Route("push-notification-birthday")]
        [HttpPut]
        public async Task<ApiObjectResponse<bool>> GetListCustomerBirthday()
        {
            return await _hrBLL.GetListCustomerBirthday();
        }
    }
}
