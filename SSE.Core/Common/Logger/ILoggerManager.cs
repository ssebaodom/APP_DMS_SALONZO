using System;

namespace SSE.Core.Common.Logger
{
    public interface ILoggerManager
    {
        public void LogInformation(string message);

        public void LogError(string message, Exception exception);
    }
}