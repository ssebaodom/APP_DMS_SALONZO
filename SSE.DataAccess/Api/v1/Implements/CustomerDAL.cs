using Dapper;
using Microsoft.Extensions.Configuration;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.Api.v1.Results.Customer;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Constants;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.DataAccess.Support.Functs;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class CustomerDAL : ICustomerDAL
    {
        private readonly IDapperService dapperService;
        private readonly IConfiguration configuration;

        public CustomerDAL(IDapperService dapperService, IConfiguration configuration)
        {
            this.dapperService = dapperService;
            this.configuration = configuration;
        }

        public async Task<CustomerListResult> CustomerList(CustomerListResquest customerList)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<CustomerListResquest>(customerList);

            GridReader gridReader = await dapperService.QueryMultipleAsync("[app_Customer_List]", parameters);

            IEnumerable<CustomerListDTO> customerList1 = gridReader.Read<CustomerListDTO>().ToList();
            int CountTotal = gridReader.ReadSingle<int>();

            if (gridReader == null)
            {
                return new CustomerListResult
                {
                    IsSucceeded = false
                };
            }

            return new CustomerListResult
            {
                IsSucceeded = true,
                Data = customerList1,
                PageIndex = customerList.PageIndex,
                TotalCount = CountTotal
            };
        }

        public async Task<CustomerListResult> SearchCustomerList(CustomerListResquest customerList)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<CustomerListResquest>(customerList);

            GridReader gridReader = await dapperService.QueryMultipleAsync("[app_Seach_Customer_List_v2]", parameters);

            IEnumerable<CustomerListDTO> customerList1 = gridReader.Read<CustomerListDTO>().ToList();
            int CountTotal = gridReader.ReadSingle<int>();

            if (gridReader == null)
            {
                return new CustomerListResult
                {
                    IsSucceeded = false
                };
            }

            return new CustomerListResult
            {
                IsSucceeded = true,
                Data = customerList1,
                PageIndex = customerList.PageIndex,
                TotalCount = CountTotal
            };
        }

        public async Task<CustomerCreateResult> CustomerCreate(CustomerCreateResquest customerCreate)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<CustomerCreateResquest>(customerCreate);

            string result = await dapperService.ExecuteScalarAsync<string>("app_Customer_Create", parameters, CommandType.StoredProcedure);

            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
                return new CustomerCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            else
                return new CustomerCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
        }

        public async Task<CustomerDetailResult> CustomerInfo(string codeCustomer, long UserId, string unitID, string storeID, string lang, int admin)
        {
            //DynamicParameters parameters = dapperService.CreateDynamicParameters<CustomerDetailResquest>(customerDetail);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerCode", codeCustomer);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@StoreId", storeID);
            parameters.Add("@Lang", lang);
            parameters.Add("@Admin", admin);


            GridReader gridReader = await dapperService.QueryMultipleAsync("[app_Customer_Info]", parameters);

            CustomerInfoDTO customerInfos = gridReader.ReadFirstOrDefault<CustomerInfoDTO>();
            List<CustomerInfoOtherDTO> customerInfoOthers = gridReader.Read<CustomerInfoOtherDTO>().ToList();
            customerInfos.OtherData = customerInfoOthers;

            if (customerInfos == null)
                return new CustomerDetailResult()
                {
                    IsSucceeded = false
                };
            else
                return new CustomerDetailResult()
                {
                    IsSucceeded = true,
                    Data = customerInfos,
                };
        }

        public async Task<ListCustomerCareResult> ListCustomerCare(long UserId,string unitID ,string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@Customer", idCustomer);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_customer_care_list", parameters);
            List<CareCustomerListDTO> listCustomerCare = gridReader.Read<CareCustomerListDTO>().ToList();
            List<ImageListRequestOpentStore> listImage = gridReader.Read<ImageListRequestOpentStore>().ToList();
            int totalPage = gridReader.ReadFirst<int>();

            List<CareCustomerListDTO> listValuesCustomerCare = new List<CareCustomerListDTO>();
            string hostUrl = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.SERVER_LINK].ToString();

            foreach (CareCustomerListDTO item in listCustomerCare)
            {
                CareCustomerListDTO values = new CareCustomerListDTO();
                
                values = item;
                foreach (ImageListRequestOpentStore item2 in listImage)
                {
                    if (item.stt_rec.Trim() == item2.key_value.Trim())
                    {
                        ImageListRequestOpentStore valuesChangePath = new ImageListRequestOpentStore();
                        valuesChangePath.key_value = item2.key_value;
                        valuesChangePath.ma_album = item2.ma_album;
                        valuesChangePath.path_l = hostUrl + "/fsdUpload/" + item2.path_l;
                        valuesChangePath.ma_kh = item2.ma_kh;
                        valuesChangePath.ten_album = item2.ten_album;
                        values.imageList.Add(valuesChangePath);
                    }
                }
                listValuesCustomerCare.Add(values);
            }


            if (gridReader == null)
            {
                return new ListCustomerCareResult
                {
                    IsSucceeded = false
                };
            }

            return new ListCustomerCareResult
            {
                IsSucceeded = true,
                ListCustomerCare = listValuesCustomerCare,
                TotalPage = totalPage
            };
        }

        public async Task<CommonResult> CustomerCareCreate(CustomerCareCreateResquest request)
        {
            string str = CustomerHelp.CreateCareCustomer_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new CommonResult()
                {
                    IsSucceeded = true,
                    Message = message,

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

        public async Task<DynamicResult> ListCustomerAction(long UserId, string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@Customer", idCustomer);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("list_history_customer_action", parameters);
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
    }
}