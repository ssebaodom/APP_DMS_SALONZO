using Dapper;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Results.Fulfillment;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class CDTranDAL : ICDTranDAL
    {
        private readonly IDapperService dapperService;

        public CDTranDAL(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }
        public async Task<FulfillmentResults> DetailCDTranPdf(FulfillmentRequest req)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", req.UserId);
            parameters.Add("@UnitId", req.UnitId);
            parameters.Add("@DateTimeFrom", req.DateTimeFrom);
            parameters.Add("@DateTimeTo", req.DateTimeTo);
            parameters.Add("@CusTomer", req.CusTomer);
            parameters.Add("@ItemCode", req.Itemcode);


            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_fulfillment", parameters);
            var total = gridReader.Read<string>().ToList();
            List<GETListFulfillmentDTO> lineck = gridReader.Read<GETListFulfillmentDTO>().ToList();

            FulfillmentResults re = new FulfillmentResults();
            return re;
        }
       
    }
}
