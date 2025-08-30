using System.Text.RegularExpressions;

namespace SSE.Core.Common.Annotations
{
    public class PasswordValid : AnnotationBase
    {
        public bool RequiredDigit { set; get; } = true;
        public int MinDigit { set; get; } = 6;
        public bool RequiredLowerCase { set; get; } = true;
        public bool RequiredUpperCase { set; get; } = true;
        public bool RequiredSpecialCase { set; get; } = true;

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            string stringValue = value.ToString();
            if (stringValue.Length < MinDigit)
            {
                return false;
            }
            if (RequiredDigit)
            {
                if (!Regex.IsMatch(stringValue, @"[0-9]+"))
                {
                    return false;
                }
            }
            if (RequiredLowerCase)
            {
                if (!Regex.IsMatch(stringValue, @"[a-z]+"))
                {
                    return false;
                }
            }
            if (RequiredUpperCase)
            {
                if (!Regex.IsMatch(stringValue, @"[A-Z]+"))
                {
                    return false;
                }
            }
            if (RequiredSpecialCase)
            {
                if (!Regex.IsMatch(stringValue, @"[^0-9a-zA-z]+"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}