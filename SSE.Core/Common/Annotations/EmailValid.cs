using SSE.Core.Common.Extensions;

namespace SSE.Core.Common.Annotations
{
    public class EmailValid : AnnotationBase
    {
        public override bool IsValid(object emailAddress)
        {
            return emailAddress != null && emailAddress.ToString().IsEmail();
        }
    }
}