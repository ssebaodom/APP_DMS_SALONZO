using Dapper;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Results.Fulfillment;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.DataAccess.Support.Functs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class FulfillmentDAL : IFulfillmentDAL
    {
        private readonly IDapperService dapperService;

        public FulfillmentDAL(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }
        public async Task<FulfillmentResults> GetListFulfillment(FulfillmentRequest req)
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
            re.data = lineck;
            re.IsSucceeded = true;
            return re;
        }
        public async Task<DetailFulfillmentResults> GetDetailFulfillment(DetailFulfillmentRequest req)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", req.stt_rec);


            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_fulfillment_detail", parameters);
            List<DetailListFulfillmentMasterDTO> master = gridReader.Read<DetailListFulfillmentMasterDTO>().ToList();
            List<DetailListFulfillmentDetailDTO> line = gridReader.Read<DetailListFulfillmentDetailDTO>().ToList();
            DetailFulfillmentResults re = new DetailFulfillmentResults();
            if(master.Count == 0)
            {
                re.IsSucceeded = false;
                re.Message = "Không tồn tại phiếu giao hàng";
                return re;
            }
            re.IsSucceeded = true;
            DetailFulfillmentResultsDTO data = new DetailFulfillmentResultsDTO();
            data.master = master[0];
            data.dettail = line;
            re.data = data;
            return re;
        }
        public async Task<ConfirmFulfillmentResults> CofirmFulfillment(ConfirmFulfillmentRequest req)
        {
            ConfirmFulfillmentResults re = new ConfirmFulfillmentResults();
            try
            {
                string lst_line = "";
                string lst_sl = "";
                foreach (var line in req.ds_line)
                {
                    lst_line += $"{line.stt_rec0},";
                    lst_sl += $"{line.so_luong},";
                }
                lst_line = lst_line.Remove(lst_line.Length - 1);
                lst_sl = lst_sl.Remove(lst_sl.Length - 1);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@stt_rec", req.ds_line[0].stt_rec);
                parameters.Add("@lst_line", lst_line);
                parameters.Add("@lst_sl", lst_sl);
                parameters.Add("@TypePayment", req.TypePayment);
                parameters.Add("@Status", req.status);
                parameters.Add("@Description ", req.desc);
                parameters.Add("@UserID", req.UserId);


                int gridReader = dapperService.Execute("app_fulfillment_confirm", parameters);


                re.IsSucceeded = true;
                re.data = "giao thành công";
                return re;
            }
            catch (Exception e)
            {
                re.IsSucceeded = false;
                re.Message = e.Message;
                return re;
            }
        }

        public async Task<ConfirmFulfillmentResults> Test(ConfirmFulfillmentRequest req)
        {
            ConfirmFulfillmentResults re = new ConfirmFulfillmentResults();
            try
            {
                string lst_line = "";
                string lst_sl = "";
                foreach (var line in req.ds_line)
                {
                    lst_line += $"{line.stt_rec0},";
                    lst_sl += $"{line.so_luong},";
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@stt_rec", req.ds_line[0].stt_rec);
                parameters.Add("@stt_rec", req.ds_line[0].stt_rec);
                parameters.Add("@stt_rec", req.ds_line[0].stt_rec);



                re.IsSucceeded = true;
                re.data = "giao thành công";
                return re;
            }
            catch (Exception e)
            {
                re.IsSucceeded = false;
                re.Message = e.Message;
                return re;
            }
        }
        public async Task<DynamicResult> GetAuthorizeList(GetAuthorizeListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetAuthorizeListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("Lay_ds_phieu_duyet", parameters);

            dynamic data1 = gridReader.Read<dynamic>();
            dynamic data = gridReader.Read<dynamic>();
            int totalPage = gridReader.ReadFirst<int>();
            if (data == null)
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
        public async Task<DynamicResult> GetAuthorizeTypeList(GetAuthorizeTypeListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetAuthorizeTypeListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Authorize_Type_List", parameters);

            dynamic data = gridReader.Read<dynamic>();
            if (data == null)
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
        public async Task<DynamicResult> GetAuthorizeStatusList(GetAuthorizeStatusListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetAuthorizeStatusListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Authorize_Status_List", parameters);

            dynamic data = gridReader.Read<dynamic>();
            if (data == null)
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
        public async Task<AuthorizeResult> Authorize(AuthorizeRequest request)
        {
            string str = OrderCreateHelp.Authorize_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new AuthorizeResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new AuthorizeResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        /// <summary>
        /// Kế hoạch giao hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DynamicResult> ListDeliveryPlan(ListDeliveryPlanRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<ListDeliveryPlanRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_delivery_plan", parameters);


            dynamic data = gridReader.Read<dynamic>();

            if (data == null)
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

        public async Task<DynamicResult> DetailDeliveryPlan(DetailDeliveryPlanRequest request)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", request.stt_rec);
            parameters.Add("@ngay_giao", request.ngay_giao);
            parameters.Add("@ma_kh", request.ma_kh);
            parameters.Add("@ma_vc", request.ma_vc);
            parameters.Add("@nguoi_giao", request.nguoi_giao);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_deliveryplan_detail", parameters);


            dynamic data = gridReader.Read<dynamic>();

            if (data == null)
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

        public async Task<DynamicResult> UpdateDeliveryPlan(DeliveryPlanRequest request)
        {
            string str = OrderCreateHelp.UpdateDeliveryPlan_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new DynamicResult
                {
                    IsSucceeded = true,
                };
            }
            else
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }
        }

        public async Task<DynamicResult> CreateDeliveryPlan(DeliveryPlanRequest request)
        {
            string str = OrderCreateHelp.CreateDeliveryPlan_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new DynamicResult
                {
                    IsSucceeded = true,
                };
            }
            else
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }
        }
    }
}
