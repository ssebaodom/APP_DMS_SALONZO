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
using SSE.Common.Api.v1.Requests.HR;
using System.Linq;
using SSE.Core.Common.BaseApi;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class HRDAL : IHRDAL
    {
        private readonly IDapperService dapperService;
        private readonly ICached cached;
        private readonly IConfiguration configuration;

        public HRDAL(IDapperService dapperService, ICached cached, IConfiguration configuration)
        {
            this.dapperService = dapperService;
            this.cached = cached;
            this.configuration = configuration;
        }

        public async Task<DynamicResult> GetListLeaveLetter(string status, long UserId, string unitID, string Lang, string dateFrom, string dateTo, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateFrom);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@status", status);
            parameters.Add("@Language", Lang);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_leave_letter", parameters);
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

        public async Task<CommonResult> CreateLeaveLetter(CreateLeaveLetterRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", request.dateFrom);
            parameters.Add("@DateTo", request.dateTo);
            parameters.Add("@TimeFrom", request.timeFrom);
            parameters.Add("@TimeTo", request.timeTo);
            parameters.Add("@LeaveType", request.leaveType);
            parameters.Add("@Description", request.description);
            parameters.Add("@MaCong", request.maCong);
            parameters.Add("@Date", request.date);
            
            parameters.Add("@UnitId", request.unitId);
            parameters.Add("@UserId", request.userId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_create_leave_letter", parameters);

            List<string> data = gridReader.Read<string>().ToList();
            string result = data[0];
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new CommonResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new CommonResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        
        public async Task<CommonResult> CancelLeaveLetter(string sctRec, string stt, long UserId, string UnitId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", sctRec);
            parameters.Add("@stt", stt);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", UnitId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_cancel_leave_letter", parameters);

            List<string> data = gridReader.Read<string>().ToList();
            string result = data[0];
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new CommonResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new CommonResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }

        public async Task<ApiListResponse<string>> GetListCustomerBirthday()
        {
            DynamicParameters parameters = new DynamicParameters();

            var result = await dapperService.QueryAsync<string>("getCustomerBirthInDay", parameters);


            if (result != null)
            {
                return new ApiListResponse<string>()
                {
                    StatusCode = 200,
                    Data = result
                };
            }
            else
            {
                return new ApiListResponse<string>()
                {
                    StatusCode = 500,
                    Message = "Lấy danh sách sinh nhật không thành công",
                    Data = null
                };
            }
        }
    }
}
