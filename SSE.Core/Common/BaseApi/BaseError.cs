namespace SSE.Core.Common.BaseApi
{
    public abstract class BaseError
    {
        public string Message { set; get; }
        public string InnerMessage { set; get; }
    }
}