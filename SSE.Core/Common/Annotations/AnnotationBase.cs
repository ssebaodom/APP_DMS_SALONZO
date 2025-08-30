using System.ComponentModel.DataAnnotations;

namespace SSE.Core.Common.Annotations
{
    public abstract class AnnotationBase : ValidationAttribute
    {
        /// <summary>
        /// Tự thiết lập validate, khi sử dụng thuộc tính này, các thuộc tính khác sẽ không được kích hoạt
        /// </summary>
       // public object CustomExpress { set; get; }
    }
}