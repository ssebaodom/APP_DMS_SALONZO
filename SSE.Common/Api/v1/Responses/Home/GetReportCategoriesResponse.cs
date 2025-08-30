using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Responses.Home
{
    public class GetReportCategoriesResponse : BaseResponse
    {
        public IEnumerable<ReportCategoryDTO> ReportCategories { get; set; }
    }
}