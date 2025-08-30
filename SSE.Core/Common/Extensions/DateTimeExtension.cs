using System;

namespace SSE.Core.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToVn(this DateTime DateTime)
        {
            return DateTime.ToUniversalTime().AddHours(7);
        }
    }
}