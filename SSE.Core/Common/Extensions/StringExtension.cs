using System.Text.RegularExpressions;

namespace SSE.Core.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsEmail(this string value)
        {
            return Regex.IsMatch(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }

        public static bool IsPhone(this string value)
        {
            return Regex.IsMatch(value, "([0-9]+){10,11}");
        }
    }
}