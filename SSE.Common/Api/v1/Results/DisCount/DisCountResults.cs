using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.DisCount
{
    public class DisCountResults : BaseResult
    {
        public DataDisCount data { get; set; }
    }

    public class DisCountApplyResult : BaseResult
    {
        public List<dynamic> list_ck_tong_don { get; set; }
        public List<dynamic> list_ck { get; set; }
        public List<dynamic> list_ck_mat_hang { get; set; }
        public TotalMoneyDTO totalMoneyDiscount { get; set; }
    }
}
