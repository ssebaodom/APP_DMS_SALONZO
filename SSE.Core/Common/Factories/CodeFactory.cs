using SSE.Core.Common.Enums;
using System;
using System.Linq;

namespace SSE.Core.Common.Factories
{
    public class CodeFactory
    {
        private string numbers = "0123456789";

        public string createStringCode(StringCodeEnums code, int length = 6)
        {
            switch (code)
            {
                case StringCodeEnums.OTP:
                    return otpCode(length);
            }
            return "";
        }

        private string otpCode(int length)
        {
            Random rd = new Random();
            return new String(Enumerable.Repeat(numbers, length).Select(item => numbers[rd.Next(numbers.Length)]).ToArray());
        }
    }
}