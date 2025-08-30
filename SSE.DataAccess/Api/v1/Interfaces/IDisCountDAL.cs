using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Requests.DisCountRequest;
using SSE.Common.Api.v1.Results.DisCount;
using System.Threading.Tasks;
using SSE.Common.Api.v1.Common;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface IDisCountDAL
    {
        Task<DisCountResults> GetDisCount(DisCountRequest req);
        Task<DisCountResults> GetDisCountWhenUpdate(DisCountWhenUpdateRequest req);
        /// <summary>
        /// 19/12/2022 
        /// V2 Tổng quát chương trình khuyến mại
        /// </summary>
        /// <creater name="tiennq"></creater>
        /// <returns>list discount</returns>
        Task<DisCountApplyResult> ApplyDiscount(DisCountItemRequest disCountItemRequest);

    }
}
