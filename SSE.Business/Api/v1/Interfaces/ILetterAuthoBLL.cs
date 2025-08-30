using SSE.Common.Api.v1.Requests.LetterAutho;
using SSE.Common.Api.v1.Responses.LetterAutho;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface ILetterAuthoBLL
    {
        Task<LetterAuDisplayResponse> LetterAuDisplay(LetterAuDisplayResquest request);

        Task<LetterListResponse> LetterList(LetterListResquest request);

        Task<LetterApproResponse> LetterApproval(LetterApproResquest letterAppro);

        Task<LetterDetailResponse> LetterDetail(LetterDetailResquest letterDetail);
        Task<LetterDetailResponse2> LetterDetail2(LetterDetailResquest letterDetail);
    }
}