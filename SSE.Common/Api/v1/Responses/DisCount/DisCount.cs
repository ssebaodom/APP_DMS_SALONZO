using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Results.DisCount;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Responses.DisCount
{
    public class DisCountResponse : CommonResponse
    {
        public DataDisCount data { get; set; }
    }

    public class DisCountApplyResponse : CommonResponse
    {
        public List<dynamic> list_ck_tong_don { get; set; }
        public List<dynamic> list_ck { get; set; }
        public List<dynamic> list_ck_mat_hang { get; set; }
        public TotalMoneyDTO totalMoneyDiscount { get; set; }
    }
}
