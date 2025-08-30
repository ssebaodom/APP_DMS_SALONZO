using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Results.DisCount;
using SSE.Common.Api.v1.Results.Fulfillment;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Responses.Fulfillment
{
    public class FulfillmentResponse : CommonResponse
    {
        public List<GETListFulfillmentDTO> data { get; set; }
    }
    public class DetailFulfillmentResponse : CommonResponse 
    {
        public DetailFulfillmentResultsDTO data { get; set; }
    }
    public class ConfirmFulfillmentResponse : CommonResponse
    {
        public string data { get; set; }
    }
}
