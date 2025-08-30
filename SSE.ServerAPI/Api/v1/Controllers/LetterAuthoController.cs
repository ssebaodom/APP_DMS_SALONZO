using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSE.Business.Api.v1.Interfaces;
using SSE.Common.Api.v1.Requests.LetterAutho;
using SSE.Common.Api.v1.Responses.LetterAutho;
using SSE.Common.Constants.v1;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/letter-authority")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LetterAuthoController : ControllerBase
    {
        private readonly ILetterAuthoBLL letterAuthoBLL;

        public LetterAuthoController(ILetterAuthoBLL letterAuthoBLL)
        {
            this.letterAuthoBLL = letterAuthoBLL;
        }

        [Route("letter-display")]
        [HttpGet]
        public async Task<LetterAuDisplayResponse> LetterDisplay()
        {
            LetterAuDisplayResquest request = new LetterAuDisplayResquest();
            return await this.letterAuthoBLL.LetterAuDisplay(request);
        }

        [Route("letter-list")]
        [HttpPost]
        public async Task<LetterListResponse> LetterList(LetterListResquest request)
        {
            return await this.letterAuthoBLL.LetterList(request);
        }

        [Route("letter-approval")]
        [HttpGet]
        public async Task<LetterApproResponse> LetterApproval(LetterAuthorityAction ActionType, string LetterId)
        {
            LetterApproResquest request = new LetterApproResquest() { LetterId = LetterId, ActionType = ActionType };
            return await this.letterAuthoBLL.LetterApproval(request);
        }

        [Route("letter-detail")]
        [HttpGet]
        public async Task<LetterDetailResponse> LetterDetail(string LetterId)
        {
            LetterDetailResquest request = new LetterDetailResquest() { LetterId = LetterId };
            return await this.letterAuthoBLL.LetterDetail(request);
        }
        [Route("detail")]
        [HttpGet]
        public async Task<LetterDetailResponse2> LetterDetail2(string LetterId)
        {
            LetterDetailResquest request = new LetterDetailResquest() { LetterId = LetterId };
            return await this.letterAuthoBLL.LetterDetail2(request);
        }
    }
}