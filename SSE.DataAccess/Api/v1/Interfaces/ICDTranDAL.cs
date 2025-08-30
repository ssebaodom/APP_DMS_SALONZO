using System;
using System.Collections.Generic;
using System.Text;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Results.Fulfillment;
using System.Threading.Tasks;

namespace SSE.DataAccess.Api.v1.Interfaces
{
    public interface ICDTranDAL
    {
        Task<FulfillmentResults> DetailCDTranPdf(FulfillmentRequest req);
    }
}
