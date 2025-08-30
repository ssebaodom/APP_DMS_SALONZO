namespace SSE.Core.Common.BaseApi
{
    public abstract class BaseResult
    {
        public bool IsSucceeded { set; get; } = false;
        public string Message { set; get; }
    }
}