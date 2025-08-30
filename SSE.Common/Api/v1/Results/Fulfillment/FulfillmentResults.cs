using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.DTO.v1;
using SSE.Core.Common.BaseApi;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Results.Fulfillment
{
    public class FulfillmentResults : BaseResult
    {
        public List<GETListFulfillmentDTO> data { get; set; }
    }
    public class DetailFulfillmentResults : BaseResult
    {
        public DetailFulfillmentResultsDTO data { get; set; }
    }
    public class ConfirmFulfillmentResults : BaseResult
    {
        public string data { get; set; }
    }
}
