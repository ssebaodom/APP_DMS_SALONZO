using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.User
{
    public class GetCompaniesResult : BaseResult
    {
        public IEnumerable<CompanyDTO> Companies { get; set; }
    }
}