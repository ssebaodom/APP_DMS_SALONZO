namespace SSE.Business.Api.v1.Interfaces
{
    public interface ISmsBLL
    {
        T sendMessage<T>(string content, string phones);
    }
}