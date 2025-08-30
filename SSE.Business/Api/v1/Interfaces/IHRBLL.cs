using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.HR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IHRBLL
    {
        Task<DynamicResponse> GetListLeaveLetter(string status, string dateFrom, string dateTo, int page_index, int page_count);
        Task<CommonResponse> CreateLeaveLetter(CreateLeaveLetterRequest request);
        Task<CommonResponse> CancelLeaveLetter(string sctRec, string stt);
        Task<ApiObjectResponse<bool>> GetListCustomerBirthday();
    }
}
