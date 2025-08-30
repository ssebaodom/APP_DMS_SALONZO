using Dapper;
using Microsoft.Extensions.Configuration;
using SSE.Common.Api.v1.Common;
//using SSE.Common.Api.v1.Requests.DMS;
using SSE.Common.Api.v1.Requests.Report;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Api.v1.Results.Todos;
//using SSE.Common.Api.v1.Requests.Todos;
//using SSE.Common.Api.v1.Results.DMS;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Constants;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.DataAccess.Support.Functs;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class DMSDAL : IDMSDAL
    {
        private readonly IDapperService dapperService;
        private readonly ICached cached;
        private readonly IConfiguration configuration;

        public DMSDAL(IDapperService dapperService, ICached cached, IConfiguration configuration)
        {
            this.dapperService = dapperService;
            this.cached = cached;
            this.configuration = configuration;
        }

        public async Task<TodosLayoutResult> TodosLayoutPage(long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);


            GridReader gridReader = await dapperService.QueryMultipleAsync("app_TodosLayoutPage", parameters);

            List<TodoTypeListDTO> todoTypeLists = gridReader.Read<TodoTypeListDTO>().ToList();
            List<TodoReportDTO> todoReports = gridReader.Read<TodoReportDTO>().ToList();
            dynamic todoStatus = gridReader.Read<dynamic>();
            dynamic todoStatusSelect = gridReader.Read<dynamic>();
            dynamic TodoChart = gridReader.Read<dynamic>();

            if (gridReader == null)
            {
                return new TodosLayoutResult
                {
                    IsSucceeded = false
                };
            }

            return new TodosLayoutResult
            {
                IsSucceeded = true,
                TodoType = todoTypeLists,
                TodoReport = todoReports,
                TodoStatusSelection = todoStatusSelect,
                TodoStatus = todoStatus,
                TodoChart = TodoChart
            };
        }
        public async Task<TodosListResult> TodosList(string Code, string Status, int PageIndex, long UserId, string UnitId, string Lang, int Admin, List<ReportRequestDTO> FilterData)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Code", Code);
            parameters.Add("@Status", Status);
            parameters.Add("@PageIndex", UserId);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            if ( FilterData.Count > 0 )
                foreach (var element in FilterData)
                {
                    parameters.Add($"@{element.Field}", element.Value is null ? "" : element.Value);
                }

            var result = await dapperService.QueryAsync<dynamic>("[app_TodosList]", parameters);

            if (result == null)
            {
                return new TodosListResult
                {
                    IsSucceeded = false
                };
            }

            return new TodosListResult
            {
                IsSucceeded = true,
                Data = result,

            };
        }
        public async Task<TodosViewerResult> TodosViewer(string TodoId, long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TodoId", TodoId);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);


            var result = await dapperService.QueryFirstOrDefaultAsync<dynamic>("[app_TodosViewer]", parameters);


            if (result == null)
            {
                return new TodosViewerResult
                {
                    IsSucceeded = false
                };
            }

            return new TodosViewerResult
            {
                IsSucceeded = true,
                Data = result,

            };
        }
        public async Task<TodosCreateResult> TodosCreate(TodoCreateRequest values)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<TodoCreateRequest>(values);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Todos_Create", parameters);
             
            string result = gridReader.ReadFirstOrDefault<string>();
            UserNotifyDTO requestNotify = gridReader.ReadFirstOrDefault<UserNotifyDTO>();
            
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new TodosCreateResult()
                {
                    IsSucceeded = true,
                    Message = message,
                    userNotify = requestNotify
                };
            }
            else
            {
                return new TodosCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        public async Task<TodosCreateResult> TodoUpdate(TodoUpdateRequest values)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<TodoUpdateRequest>(values);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_TodoUpdate", parameters);

            string result = gridReader.ReadFirstOrDefault<string>();
            UserNotifyDTO requestNotify = gridReader.ReadFirstOrDefault<UserNotifyDTO>();

            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new TodosCreateResult()
                {
                    IsSucceeded = true,
                    Message = message,
                    userNotify = requestNotify
                };
            }
            else
            {
                return new TodosCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        /// <summary>
        /// Check-in 09/2022 tiennq
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// 

        public async Task<CommonResult> OrderCreateFromCheckIn(OrderCreateFromCheckInRequest request)
        {
            string str = OrderCreateHelp.OrderCreateFromCheckIn_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
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

        public async Task<CommonResult> CreateRequestOpenStore(RequestOpenStoreRequest request)
        {
            string str = OrderCreateHelp.CreateRequestOpenStore_GetQuery(request);

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
        
        public async Task<CommonResult> UpdateRequestOpenStore(UpdateRequestOpenStoreRequest request)
        {
            string str = OrderCreateHelp.UpdateRequestOpenStore_GetQuery(request);

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

        public async Task<CommonResult> CancelRequestOpenStore(string idTour, string idRequestOpenStore)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@tour", idTour);
            parameters.Add("@stt_rec_dm", idRequestOpenStore);

            string result = await dapperService.ExecuteScalarAsync<string>("app_Delete_Lead", parameters);

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

        public async Task<CommonResult> CreateNewTicket(CreatNewTicketRequest request)
        {
            string str = OrderCreateHelp.CreateNewTicket(request);

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
        
        public async Task<CheckInResult> GetListCheckin(CheckinRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@date", request.datetime);
            parameters.Add("@userID", request.UserId);
            parameters.Add("@PageIndex", request.page_index);
            parameters.Add("@PageCount", request.page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_Daily_job", parameters);
            List<dynamic> listCheckInToDay = gridReader.Read<dynamic>().ToList();
            //List<dynamic> listCheckInOther = gridReader.Read<dynamic>().ToList();
            int TotalPage = gridReader.ReadFirst<int>();

            //dynamic data = gridReader.Read<dynamic>();
            if (gridReader == null)
            {
                return new CheckInResult
                {
                    IsSucceeded = false
                };
            }

            return new CheckInResult
            {
                IsSucceeded = true,
                //ListCheckInOther = listCheckInOther,
                ListCheckInToDay = listCheckInToDay,
                TotalPage = TotalPage
            };
        }

        public async Task<DataRequestOpenStoreResult> GetListRequestOpenStore(string dateForm, string dateTo, string district, int status,string dateTime, long UserId, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@district", district);
            parameters.Add("@dateCreate", dateTime);
            parameters.Add("@userID", UserId);
            parameters.Add("@status", status);
            
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);
            
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
           
            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_lead", parameters);

            List<MasterRequestOpenStore> listMaster = gridReader.Read<MasterRequestOpenStore>().ToList();
            List<ImageListRequestOpentStore> listImage = gridReader.Read<ImageListRequestOpentStore>().ToList();
            List<Khuvuc> listKhuVuc = gridReader.Read<Khuvuc>().ToList();

            List<MasterRequestOpentStoreDTO> listValues = new List<MasterRequestOpentStoreDTO>();

            DataRequestOpentStoreDTO Data = new DataRequestOpentStoreDTO();

            string hostUrl = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.SERVER_LINK].ToString();

            foreach (MasterRequestOpenStore item in listMaster)
            {
                MasterRequestOpentStoreDTO values = new MasterRequestOpentStoreDTO();
                values.Master = item;

                foreach (ImageListRequestOpentStore item2 in listImage)
                {
                    if (item.key_value.Trim() == item2.key_value.Trim())
                    {
                        ImageListRequestOpentStore valuesChangePath = new ImageListRequestOpentStore();
                        valuesChangePath.key_value = item2.key_value;
                        valuesChangePath.ma_album = item2.ma_album;
                        valuesChangePath.path_l = hostUrl + "/fsdUpload/" + item2.path_l;
                        valuesChangePath.ma_kh = item2.ma_kh;
                        valuesChangePath.ten_album = item2.ten_album;
                        values.imageListRequestOpenStore.Add(valuesChangePath);
                    }
                }
                listValues.Add(values);
            }

            Data.dataRequest = listValues;

            if (gridReader == null)
            {
                return new DataRequestOpenStoreResult
                {
                    IsSucceeded = false
                };
            }

            return new DataRequestOpenStoreResult
            {
                IsSucceeded = true,
                Data = Data,
                listKhuVuc = listKhuVuc
            };
        }
        
        public async Task<DetailCheckInResult> GetDetailCheckin(string idCheckIn, string idCustomer )
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", idCheckIn);
            parameters.Add("@customerId", idCustomer);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_Daily_job_detail", parameters);

            List<dynamic> data = gridReader.Read<dynamic>().ToList();
            List<dynamic> listAlbumCheckIn = gridReader.Read<dynamic>().ToList();
            List<dynamic> listTicket = gridReader.Read<dynamic>().ToList();
            if (gridReader == null)
            {
                return new DetailCheckInResult
                {
                    IsSucceeded = false
                };
            }

            return new DetailCheckInResult
            {
                IsSucceeded = true,
                Master = data,
                ListAlbum = listAlbumCheckIn,
                ListTicket = listTicket
            };
        }

        public async Task<DetailRequestOpenStoreResult> GetListRequestOpenStoreDetail(string idRequestOpenStore, long UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id_lead", idRequestOpenStore);
            parameters.Add("@userID", UserId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_lead_detail", parameters);

            List<MasterRequestOpenStoreDetailDTO> listDetail = gridReader.Read<MasterRequestOpenStoreDetailDTO>().ToList();
            List<RoleUpdateRequestOpenStore> listRoles = gridReader.Read<RoleUpdateRequestOpenStore>().ToList();

            MasterRequestOpenStoreDetailDTO masterDetail = new MasterRequestOpenStoreDetailDTO();

            RoleUpdateRequestOpenStore roles = new RoleUpdateRequestOpenStore();

            foreach (MasterRequestOpenStoreDetailDTO item in listDetail)
            {
                masterDetail = item;
            }

            foreach (RoleUpdateRequestOpenStore role in listRoles)
            {
                roles = role;
            }

            if (gridReader == null)
            {
                return new DetailRequestOpenStoreResult
                {
                    IsSucceeded = false
                };
            }

            return new DetailRequestOpenStoreResult
            {
                IsSucceeded = true,
                DetailRequestOpenStore = masterDetail,
                Roles = roles
            };
        }

        public async Task<DynamicResult> GetTourCheckin(string searchKey, int page_index, int page_count, string unitId, long idUser)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", searchKey);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);
            parameters.Add("@UnitID", unitId);
            parameters.Add("@UserID", idUser);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_dmtuyen", parameters);

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
                Data = data
            };
        }   
        
        public async Task<DynamicResult> GetListStoreForm(string searchKey, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", searchKey);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Forms_Customer", parameters);

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
                Data = data
            };
        }
        
        public async Task<DynamicResult> GetListTypeStore(string searchKey, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", searchKey);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Classify_Customer", parameters);

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
                Data = data
            };
        }
        
        public async Task<DynamicResult> GetListProvince(string province, string dictrict, int page_index, int page_count, string idArea)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Province", province);
            parameters.Add("@District", dictrict);
            
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);
            parameters.Add("@idArea", idArea);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Geographical_Location", parameters);

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
                Data = data
            };
        }
        
        public async Task<DynamicResult> GetListArea(string searchKey, int page_index, int page_count, long UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", searchKey);
            parameters.Add("@userID ", UserId);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count); 

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Area", parameters);

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
                Data = data
            };
        }
        
        public async Task<DynamicResult> GetListTypeTicket( int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_ticket_type_list", parameters);

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
                Data = data
            };
        }

        public async Task<DynamicResult> SearchListCheckIn(long UserId, string dateTime, string searchKey, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@date", dateTime);
            parameters.Add("@SearchKey ", searchKey);
            parameters.Add("@userID ", UserId);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Search_Daily_job", parameters);

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
                Data = data
            };
        }

        public async Task<DataListTicketResult> GetListTicket(string ticketType, string idCheckIn, string idCustomer, long idUser, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@taskId", idCheckIn);
            parameters.Add("@CustomerID", idCustomer);

            parameters.Add("@ticketType", ticketType);
            parameters.Add("@userID", idUser);

            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_ticket", parameters);

            List<MasterTicketDTO> listMasterTicket = gridReader.Read<MasterTicketDTO>().ToList();
            List<ImageListRequestOpentStore> listImage = gridReader.Read<ImageListRequestOpentStore>().ToList();

            List<MasterTicketDTO> listValues = new List<MasterTicketDTO>();

            string hostUrl = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.SERVER_LINK].ToString();

            foreach (MasterTicketDTO item in listMasterTicket)
            {
                MasterTicketDTO values = new MasterTicketDTO();
                values = item;
                foreach (ImageListRequestOpentStore item2 in listImage)
                {
                    if (item.id_ticket.Trim() == item2.key_value.Trim())
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
                listValues.Add(values);
            }

            if (gridReader == null)
            {
                return new DataListTicketResult
                {
                    IsSucceeded = false
                };
            }

            return new DataListTicketResult
            {
                IsSucceeded = true,
                Data = listValues
            };
        }

        public async Task<DynamicResult> GetListAlbumCheckIn(string idAlbum)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ma_album", idAlbum);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_album_list", parameters);

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
                Data = data
            };
        }
        
        public async Task<DynamicResult> GetListInventoryCheckIn(string idCustomer,string idCheckIn,int page_index, int page_count)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerID", idCustomer);
            parameters.Add("@idJob", idCheckIn);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_inventory_check", parameters);

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
                Data = data
            };
        }

        public async Task<ListImageCheckInResult> GetListImageCheckin(string idAlbum, string idCustomer,string idCheckin, int page_index, int page_count, long UserId, string UnitId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerID", idCustomer);
            parameters.Add("@idJob", idCheckin);
            parameters.Add("@userID", UserId);
            parameters.Add("@albumCode", idAlbum);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_image_daily_job", parameters);

            List<dynamic> listCheckInAlbum = gridReader.Read<dynamic>().ToList();
            List<CheckinImageDTO> listCheckInImage = gridReader.Read<CheckinImageDTO>().ToList();
           
            dynamic data = gridReader.Read<dynamic>();
            if (gridReader == null)
            {
                return new ListImageCheckInResult
                {
                    IsSucceeded = false
                };
            }

            return new ListImageCheckInResult
            {
                IsSucceeded = true,
                ListAlbum = listCheckInAlbum,
                ListImage = listCheckInImage
            };
        }

        public async Task<CommonResult> CreateTicket(CreateTicketRequest values)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<CreateTicketRequest>(values);

            string result = await dapperService.ExecuteScalarAsync<string>("app_create_ticket", parameters);

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

        public async Task<CommonResult> InventoryControl(InventoryControlRequest request)
        {
            string str = OrderCreateHelp.InventoryControl_GetQuery(request);

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
        
        public async Task<CommonResult> UpdateSaleOut(InventoryControlAndSaleOutRequest request)
        {
            string str = OrderCreateHelp.UpdateSaleOut_GetQuery(request);

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
        
        public async Task<CommonResult> CheckOut(CheckOutRequest request)
        {
            string str = OrderCreateHelp.CheckOut_GetQuery(request);

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

        public async Task<CommonResult> ReportLocation(ReportLocationRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<ReportLocationRequest>(request);

            string result = await dapperService.ExecuteScalarAsync<string>("app_CheckinCreate", parameters);

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

        public async Task<CommonResult> ReportLocationV2(ReportLocationRequestV2 request)
        {
            string str = OrderCreateHelp.ReportLocation_GetQuery(request);

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

        public async Task<DailyJobOfflineResult> DailyJobOffline(string dateTime, long idUser)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@date", dateTime);
            parameters.Add("@userID", idUser);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_daily_job_for_a_day", parameters);

            List<dynamic> listCheckIn = gridReader.Read<dynamic>().ToList();
            List<dynamic> listCheckInAlbum = gridReader.Read<dynamic>().ToList();
            List<dynamic> ListTicket = gridReader.Read<dynamic>().ToList();
            
            if (gridReader == null)
            {
                return new DailyJobOfflineResult
                {
                    IsSucceeded = false
                };
            }

            return new DailyJobOfflineResult
            {
                IsSucceeded = true,
                ListCustomer = listCheckIn,
                ListAlbum = listCheckInAlbum,
                ListTicket = ListTicket
            };
        }

        public async Task<CommonResult> TimeKeepingCreate(TimeKeepingRequest values)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<TimeKeepingRequest>(values);

            string result = await dapperService.ExecuteScalarAsync<string>("app_TimeKeeping", parameters);            

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

        public async Task<DynamicResult> CheckinHistory(CheckinHistoryRequest values)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<CheckinHistoryRequest>(values);

            string query = $"select * from app_checkin where user_id = @UserId and datetime between @DateFrom and @DateTo";

            var result = await dapperService.QueryAsync<dynamic>(query, parameters, System.Data.CommandType.Text);

            if (result == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }

            return new DynamicResult
            {
                IsSucceeded = true,
                Data = result,

            };
        }

        public async Task<TimeKeepingHistoryResult> TimeKeepingHistory(TimeKeepingHistoryRequest values)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", values.dateTime);
            parameters.Add("@UserID", values.UserId);
            parameters.Add("@Admin", values.Admin);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_rpt_time_keeping", parameters);

            List<dynamic> listDataTimeKeeping = gridReader.Read<dynamic>().ToList();
            MasterTimeKeeping infoUser = gridReader.ReadFirst<MasterTimeKeeping>();


            if (gridReader == null)
            {
                return new TimeKeepingHistoryResult
                {
                    IsSucceeded = false
                };
            }

            return new TimeKeepingHistoryResult
            {
                IsSucceeded = true,
                Master = infoUser,
                listTimeKeepingHistory = listDataTimeKeeping
            };
        }

        public async Task<DynamicResult> GetListHistoryActionEmployee(string dateFrom, string dateTo, string idCustomer, long idUser, string unitId, int page_index, int page_count)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateFrom);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@UserID", idUser);
            parameters.Add("@UnitID", unitId);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("list_history_customer_action", parameters);

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
                Data = data
            };
        }
        
        public async Task<DynamicResult> GetListHistoryTicket(string dateFrom, string dateTo, string idCustomer, string employeeCode, int status, long idUser, string unitId, int page_index, int page_count)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateFrom);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@Employee", employeeCode);
            parameters.Add("@Status", status);
            parameters.Add("@UserID", idUser);
            parameters.Add("@UnitID", unitId);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_ticket_list", parameters);

            dynamic data = gridReader.Read<dynamic>();
            int TotalCount = gridReader.ReadFirst<int>();

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
                 TotalPage = TotalCount
            };
        }

        public async Task<DynamicResult> GetListHistorySaleOut(string dateFrom, string dateTo, string idCustomer,int idTransaction, long idUser, string unitId, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateFrom);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@Customer", idCustomer);
            parameters.Add("@Transaction", idTransaction);
            parameters.Add("@UserID", idUser);
            parameters.Add("@UnitID", unitId);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_sale_out_list", parameters);

            dynamic data = gridReader.Read<dynamic>();
            int TotalCount = gridReader.ReadFirst<int>();

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
                TotalPage = TotalCount
            };
        }

        public async Task<DynamicResult> GetDetailHistorySaleOut(string stt_rec, string invoice_date, long idUser, string unitId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Stt_rec", stt_rec);
            parameters.Add("@Invoice_date", invoice_date);
            parameters.Add("@UserID", idUser);
            parameters.Add("@UnitId", unitId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_sale_out_detail", parameters);

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

        public async Task<DynamicResult> ListKPISummaryByDay(string dateFrom, string dateTo, long idUser, string unitId, string sotreId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateFrom);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", idUser);
            parameters.Add("@UnitID", unitId);
            parameters.Add("@StoreId", sotreId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_KPI_summary_by_day", parameters);

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
                Data = data
            };
        }

        public async Task<DetailHistoryTicketResult> GetDetailHistoryTicket(string idTicket, long idUser, string unitId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTicket", idTicket);
            parameters.Add("@UserID", idUser);
            parameters.Add("@UnitID", unitId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_ticket_detail", parameters);

            List<FeedbackHistoryDetailTicketDTO> listFeedback = gridReader.Read<FeedbackHistoryDetailTicketDTO>().ToList();
            List<ImageListHistoryDetailTicketDTO> listImage = gridReader.Read<ImageListHistoryDetailTicketDTO>().ToList();

            //List<MasterHistoryDetailTicketDTO> listValues = new List<MasterHistoryDetailTicketDTO>();

            MasterHistoryDetailTicketDTO valuesMaster = new MasterHistoryDetailTicketDTO();

            string hostUrl = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.SERVER_LINK].ToString();

            foreach (FeedbackHistoryDetailTicketDTO item in listFeedback)
            {

                valuesMaster.id = item.id_ticket;
                valuesMaster.phan_hoi = item.phan_hoi;
                foreach (ImageListHistoryDetailTicketDTO item2 in listImage)
                {
                    ImageListHistoryDetailTicketDTO valuesChangePath = new ImageListHistoryDetailTicketDTO();
                    valuesChangePath.code = item2.code;
                    valuesChangePath.ma_album = item2.ma_album;
                    valuesChangePath.path_l = hostUrl + "/fsdUpload/" + item2.path_l;
                    valuesChangePath.ten_album = item2.ten_album;
                    valuesMaster.imageList.Add(valuesChangePath);
                }
                //listValues.Add(values);
            }

            if (gridReader == null)
            {
                return new DetailHistoryTicketResult
                {
                    IsSucceeded = false
                };
            }

            return new DetailHistoryTicketResult
            {
                IsSucceeded = true,
                Data = valuesMaster,
            };
        }

        public async Task<DynamicResult> ListStateOpenStore(string keySearch, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", keySearch);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_State_list", parameters);

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
                Data = data
            };
        }

        public async Task<DynamicResult> GetListSaleOutCompleted(string idAgency,long UserId, string unitID, string dateForm, string dateTo, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@Customer", "");
            parameters.Add("@Distributor", idAgency);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_sale_out_completed_list", parameters);
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

        public async Task<DynamicResult> DetailSaleOutCompleted(long UserId, string unitID, string sct, string invoiceDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Stt_rec", sct);
            parameters.Add("@Invoice_date", invoiceDate);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_sale_out_completed_detail", parameters);
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

        public async Task<CommonResult> RefundCreateSaleOutV1(RefundSaleOutCreateV1Request request)
        {
            string str = OrderCreateHelp.RefundSaleOutCreate_GetQueryV1(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
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
        
        public async Task<DynamicResult> CreateTaskFromCustomer(string idCustomer, long UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerId", idCustomer);
            parameters.Add("@UserId", UserId);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_create_task_from_customer", parameters);
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
        public async Task<DynamicResult> DanhSachCauHoi(string searchKey, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@keySearch", searchKey, DbType.String);
            parameters.Add("@page_index", page_index);
            parameters.Add("@page_count", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_dmcauhoi", parameters);
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
        public async Task<DynamicResult> DanhSachCauTraLoi(string stt_rec, string ma_cau_hoi)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", stt_rec);
            parameters.Add("@ma_cau_hoi", ma_cau_hoi);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_dmcauhoi_traloi", parameters, commandType: CommandType.StoredProcedure);
            var rawData = gridReader.Read<dynamic>().ToList();

            if (gridReader == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false
                };
            }
            else
            {
                var grouped = rawData
                                    .GroupBy(x => new
                                    {
                                        SttRec = (string)x.stt_rec,
                                        SttRec0 = (string)x.stt_rec0
                                    })
                                    .Select(g => new
                                    {
                                        SttRec = g.Key.SttRec,
                                        SttRec0 = g.Key.SttRec0,
                                        Answers = g
                                            .Where(a => !string.IsNullOrWhiteSpace((string)a.ma_cau_tra_loi))
                                            .Select(a => new {
                                                MaTraLoi = ((string)a.ma_cau_tra_loi)?.Trim(),
                                                TenCauTraLoi = (string)a.cau_tra_loi
                                            })
                                            .ToList()
                                    })
                                    .Where(x => x.Answers.Any())
                                    .ToList();

                return new DynamicResult
                {
                    IsSucceeded = true,
                    Data = grouped,

                };
            }

        }
    }
}