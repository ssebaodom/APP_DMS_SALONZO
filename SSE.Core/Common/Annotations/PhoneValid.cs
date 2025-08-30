using System;
using System.Text.RegularExpressions;

namespace SSE.Core.Common.Annotations
{
    public class PhoneValid : AnnotationBase
    {
        public override bool IsValid(object phoneNumber)
        {
            try
            {
                string phone = phoneNumber.ToString();
                return Regex.IsMatch(phone, "^0[0-9]{9,10}$");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}