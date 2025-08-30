using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System.Collections.Generic;
using System.Dynamic;

namespace SSE.Common.Api.v1.Results.LetterAutho
{
    public class LetterDetailResult : CommonResult
    {
        public IEnumerable<ReportHeaderDescDTO> MainHeaderDesc { get; set; }
        public dynamic MainValues { get; set; }
        public IEnumerable<ReportHeaderDescDTO> DetailHeaderDesc { get; set; }
        public dynamic DetailValues { get; set; }
    }
    public class DataResult2
    {
        public List<MainDataDTO> main { get; set; }
        public DetailDataDTO detail { get; set; }
        public List<FanDataDTO> financials { get; set; }
        public List<InfoDataDTO> infomation { get; set; }
    }
    public class LetterDetailResult2 : CommonResult
    {
        public List<ValuesFile> listValuesFile { get; set; }
        public string Data { get; set; }
    }

    public class ValuesFile
    {
        public string valuesFile { get; set; }
        public string fileName { get; set; }
        public string fileExtension { get; set; }
    }

    public class DapperHelpers
    {
        public static dynamic ToExpandoObject(object value)
        {
            IDictionary<string, object> dapperRowProperties = value as IDictionary<string, object>;

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (KeyValuePair<string, object> property in dapperRowProperties)
                expando.Add(property.Key, property.Value);

            return expando as ExpandoObject;
        }
    }
}