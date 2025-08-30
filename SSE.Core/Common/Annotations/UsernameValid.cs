using System.Text.RegularExpressions;

namespace SSE.Core.Common.Annotations
{
    public class UsernameValid : AnnotationBase
    {
        public bool AllowLowerCase { set; get; } = true;
        public bool AllowUpperCase { set; get; }
        public bool AllowDigit { set; get; } = true;
        public string AllowCustomCase { set; get; } = "";

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            string stringValue = value.ToString();
            string contentUsername = "";
            if (AllowLowerCase)
            {
                contentUsername += @"[a-z]+";
            }
            if (AllowUpperCase)
            {
                contentUsername += @"[A-Z]+";
            }
            if (AllowDigit)
            {
                contentUsername += @"[0-9]+";
            }
            contentUsername += $@"[{AllowCustomCase}]+";
            return (!Regex.IsMatch(stringValue, contentUsername));
        }
    }
}