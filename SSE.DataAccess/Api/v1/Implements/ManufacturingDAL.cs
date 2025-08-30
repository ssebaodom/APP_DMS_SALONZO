using Dapper;
using Microsoft.Extensions.Configuration;
using SSE.Common.Api.v1.Common;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Dapper;

using System;
using System.Collections.Generic;
using System.Text;
using static Dapper.SqlMapper;
using System.Threading.Tasks;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Constants.v1;
using SSE.DataAccess.Support.Functs;
using System.Data;
using System.Linq;
using SSE.Core.Common.BaseApi;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Results.Order;
using SSE.Common.Api.v1.Requests.Manufacturing;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class ManufacturingDAL : IManufacturingDAL
    {
        private readonly IDapperService dapperService;
        private readonly ICached cached;
        private readonly IConfiguration configuration;

        public ManufacturingDAL(IDapperService dapperService, ICached cached, IConfiguration configuration)
        {
            this.dapperService = dapperService;
            this.cached = cached;
            this.configuration = configuration;
        }

        public async Task<DynamicResult> RequestSectionItem( long UserId, string unitID, string request, string route, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Request", request);
            parameters.Add("@Route", route);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@UserId", UserId);
            parameters.Add("@Page_index", page_index);
            parameters.Add("@Page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_request_section_item", parameters);
            dynamic data = gridReader.Read<dynamic>();
            int totalPage = gridReader.ReadFirst<int>();
            if (gridReader == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }

            return new DynamicResult
            {
                IsSucceeded = true,
                Data = data,
                TotalPage = totalPage
            };
        }
        public async Task<DynamicResult> GetVoucherTransaction( long UserId, string unitID, string vCCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@VCCode", vCCode);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@UserId", UserId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_voucher_transaction", parameters);
            dynamic data = gridReader.Read<dynamic>();
            
            if (gridReader == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }

            return new DynamicResult
            {
                IsSucceeded = true,
                Data = data,
              
            };
        }
        public async Task<DynamicResult> GetItemMaterials( long UserId, string unitID, string item)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Item", item);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@UserId", UserId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_item_Materials", parameters);
            dynamic data = gridReader.Read<dynamic>();
          
            if (gridReader == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }

            return new DynamicResult
            {
                IsSucceeded = true,
                Data = data,
             
            };
        } 
        public async Task<DynamicResult> GetSemiProducts(long UserId, string UnitId, string lsx, string section, string searchValue, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@LSX", lsx);
            parameters.Add("@Section", section);
            parameters.Add("@SearchValue", searchValue);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@UserId", UserId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_semi_products", parameters);
            dynamic data = gridReader.Read<dynamic>();
            int totalPage = gridReader.ReadFirst<int>();
            if (gridReader == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }

            return new DynamicResult
            {
                IsSucceeded = true,
                Data = data,
                TotalPage = totalPage
            };
        }
        public async Task<CommonsResult> CreateFactoryTransactionVoucherModify(ManufacturingRequest request)
        {
            string str = ManufacturingCreateHelp.ManufacturingCreate_GetQueryV3(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new CommonsResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new CommonsResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
    }
}
