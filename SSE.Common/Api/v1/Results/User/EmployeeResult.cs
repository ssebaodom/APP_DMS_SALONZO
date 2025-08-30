using SSE.Core.Common.BaseApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.Api.v1.Results.User
{
    public class EmployeeResult : BaseResult
    {
        public List<dynamic> totalOrder { get; set; }
        public List<dynamic> listEmployee { get; set; }
        public int totalPage { get; set; }
    }
}
