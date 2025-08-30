using System;

namespace SSE.Core.Common.Extensions
{
    public static class NumberExtension
    {
        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value);
        }
    }
}