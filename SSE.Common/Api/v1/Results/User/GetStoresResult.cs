using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.User
{
    public class GetStoresResult : BaseResult
    {
        public IEnumerable<StoreDTO> Stores { get; set; }
    }
}