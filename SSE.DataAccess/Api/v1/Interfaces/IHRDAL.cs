using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.HR;
using SSE.Core.Common.BaseApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IHRDAL
    {
        Task<DynamicResult> GetListLeaveLetter(string status, long UserId, string UnitId, string Lang, string dateFrom, string dateTo, int page_index, int page_count);
        Task<CommonResult> CreateLeaveLetter(CreateLeaveLetterRequest values);
        Task<CommonResult> CancelLeaveLetter(string sctRec, string stt, long UserId, string UnitId);
        Task<ApiListResponse<string>> GetListCustomerBirthday();
    }
}
