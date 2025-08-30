using SSE.Core.Common.BaseApi;

namespace SSE.Common.Api.v1.Results.User
{
    public class GetSysDbNameResult : BaseResult
    {
        public string ServerName { get; set; }
        public string SysDbName { get; set; }
        public string SqlUserLogin { get; set; }
        public string SqlPassLogin { get; set; }
    }
}