using SSE.Common.Api.v1.Requests.LetterAutho;
using SSE.Common.Api.v1.Results.LetterAutho;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface ILetterAuthoDAL
    {
        Task<LetterAuDisplayResult> LetterAuDisplay(LetterAuDisplayResquest letterAuDisplay);

        Task<LetterListResult> LetterList(LetterListResquest letterList);

        Task<LetterApproResult> LetterApproval(LetterApproResquest letterAppro);

        Task<LetterDetailResult> LetterDetail(LetterDetailResquest letterDetail);
        Task<LetterDetailResult2> LetterDetail2(LetterDetailResquest letterDetail);
    }
}