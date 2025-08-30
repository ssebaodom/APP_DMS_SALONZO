using SSE.Core.Common.Extensions;

namespace SSE.Core.Common.Annotations
{
    /// <summary>
    /// Validate giá trị của số thập phân trong khoảng min -> max
    /// </summary>
    public class DecimalValid : AnnotationBase
    {
        public int MinValue { set; get; }
        public int MaxValue { set; get; }

        public override bool IsValid(object value)
        {
            if (value == null || MaxValue <= MinValue)
            {
                return false;
            }
            int intValue = value.ToInt32();
            return (MinValue <= intValue && intValue <= MaxValue);
        }
    }
}