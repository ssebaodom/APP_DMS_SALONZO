using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.DisCountRequest;
using SSE.Common.Api.v1.Responses.DisCount;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Interfaces
{
    public interface IDisCountBLL
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <param name="getDisCount"></param>
        /// <returns></returns>

        Task<DisCountResponse> GetDisCount(DisCountRequest disCountRequest);

        Task<DisCountResponse> GetDisCountWhenUpdate(DisCountWhenUpdateRequest disCountRequest);
        /// <summary>
        /// 19/12/2022 
        /// V2 Tổng quát chương trình khuyến mại
        /// </summary>
        /// <creater name="tiennq"></creater>
        /// <returns>list discount</returns>
        Task<DisCountApplyResponse> ApplyDiscount(DisCountItemRequest disCountItemRequest);
    }
}
