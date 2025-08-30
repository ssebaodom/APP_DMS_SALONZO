using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Order
{

    public class DNCResult : CommonResult
    {
        public DNCMasterResultDTO master { get; set; }
        public List<DNCDetailResultDTO> detail { get; set; }
    }
   
}