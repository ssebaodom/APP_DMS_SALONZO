using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Home
{
    public class GetReportCategoriesResult : BaseResult
    {
        public IEnumerable<ReportCategoryDTO> ReportCategories { get; set; }
    }
}