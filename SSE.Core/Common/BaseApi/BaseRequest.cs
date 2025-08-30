using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSE.Core.Common.Api
{
    public abstract class BaseRequest : IValidatableObject
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}