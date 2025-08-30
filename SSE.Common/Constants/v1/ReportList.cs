using SSE.Common.Functions;
using System.Collections.Generic;

namespace SSE.Common.Constants.v1
{
    public class Report
    {
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public string SQLStoreName { get; set; }
    }

    public static class StaticValues
    {
#if DEBUG
        public static List<Report> reports = FileReader.LoadFileJson<List<Report>>("/SSE.Common/VariableData", "ReportList.json");
#else
        public static List<Report> reports = FileReader.LoadFileJson<List<Report>>("/AppData", "ReportList.json");
#endif
    }
}