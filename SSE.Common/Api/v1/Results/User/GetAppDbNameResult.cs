using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Results.User
{
    public class GetAppDbNameResult : BaseResult
    {
        public string AppDbName { get; set; }
    }
}