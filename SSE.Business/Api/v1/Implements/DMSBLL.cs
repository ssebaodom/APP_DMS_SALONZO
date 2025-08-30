using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Todos;
using SSE.Common.Api.v1.Requests.User;
using SSE.Common.Api.v1.Responses.Todos;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Files;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SSE.Core.Common.Constants;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SSE.Business.Api.v1.Implements
{
    internal class DMSBLL : IDMSBLL
    {
        private readonly IFileService fileService;
        private readonly IDMSDAL todosDAL;
        private readonly IUserBLL userBLL;
        private UserInfoCache userInfoCache;
        private readonly IConfiguration configuration;

        public DMSBLL(IDMSDAL todosDAL,
                        IUserBLL userBLL,
                       IUserBLLService userBLLService,
                       IHttpContextAccessor httpContextAccessor, IFileService fileService, IConfiguration configuration)
        {
            this.todosDAL = todosDAL;
            this.fileService = fileService;
            this.userBLL = userBLL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
            this.configuration = configuration;
        }

        public async Task<CommonResponse> CreateTodo(TodoCreateRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.todosDAL.TodosCreate(request);


            if (result.IsSucceeded == true)
            {
                // Send Notify:
                if (result.userNotify != null)
                {
                    PushNotifyRequest notifyRequest = new PushNotifyRequest
                    {
                        Type = result.userNotify.Type,
                        Tittle = result.userNotify.Title,
                        Body = result.userNotify.Body,
                        UserIds = result.userNotify.UserIds.Split(',').ToList(),
                        DevideToken = result.userNotify.Tokens.Split(',').ToList()
                    };
                    await Task.Run(() =>
                    {
                        userBLL.PushNotification(notifyRequest);
                    });
                }


                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }

            else
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonResponse> UpdateTodo(TodoUpdateRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.todosDAL.TodoUpdate(request);


            if (result.IsSucceeded == true)
            {
                // Send Notify:
                if (result.userNotify != null)
                {
                    PushNotifyRequest notifyRequest = new PushNotifyRequest
                    {
                        Type = result.userNotify.Type,
                        Tittle = result.userNotify.Title,
                        Body = result.userNotify.Body,
                        UserIds = result.userNotify.UserIds.Split(',').ToList(),
                        DevideToken = result.userNotify.Tokens.Split(',').ToList()
                    };
                    await Task.Run(() =>
                    {
                        userBLL.PushNotification(notifyRequest);
                    });
                }


                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }

            else
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<TodosLayoutResponse> TodosLayoutPage()
        {
            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.todosDAL.TodosLayoutPage(UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
            {
                return new TodosLayoutResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    TodoChart = result.TodoChart,
                    TodoReport = result.TodoReport,
                    TodoStatus = result.TodoStatus,
                    TodoStatusSelection = result.TodoStatusSelection,
                    TodoType = result.TodoType
                };
            }
            else
            {
                return new TodosLayoutResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
        public async Task<TodosViewerResponse> TodosViewer(string TodoId)
        {
            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.todosDAL.TodosViewer(TodoId, UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
            {
                return new TodosViewerResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data

                };
            }
            else
            {
                return new TodosViewerResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
        public async Task<TodosListResponse> TodosList(string Code, string Status, int PageIndex, List<ReportRequestDTO> FilterData)
        {
            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.todosDAL.TodosList(Code, Status, PageIndex, UserId, UnitId, Lang, Admin, FilterData);

            if (result.IsSucceeded == true)
            {
                return new TodosListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data

                };
            }
            else
            {
                return new TodosListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        /// <summary>
        /// Checkin 09/2022 tiennq
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        /// Convert base 64 to 
        /// 
       
        public async Task<CommonResponse> OrderCreateFromCheckIn(OrderCreateFromCheckInRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            List<IFormFile> ListFile = new List<IFormFile>();
            request.Image = new List<CheckinListImageDTO>();

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();

            if (request.Data.ListImage != null)
            {
                foreach (var item in request.Data.ListImage)
                {
                    byte[] bytes = Convert.FromBase64String(item.PathBase64);
                    MemoryStream stream = new MemoryStream(bytes);

                    IFormFile file = new FormFile(stream, 0, bytes.Length, item.NameImage, item.NameImage);
                    ListFile.Add(file);
                }
            }
            if (ListFile != null)
            {
                foreach (var item in ListFile)
                {
                    var detail = new CheckinListImageDTO();
                    string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                    string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
                    fileService.SaveFile(item, pathSaveToServer);
                    String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                    detail.Path = pathSaveDb;
                    detail.NameImage = item.FileName;
                    detail.CodeImage = Guid.NewGuid().ToString();
                    request.Image.Add(detail);
                }
            }

            var result = await this.todosDAL.OrderCreateFromCheckIn(request);

            if (result.IsSucceeded == true)
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<CommonResponse> CreateRequestOpenStore(RequestOpenStoreRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            request.Image = new List<CheckinListImageDTO>();

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();

            if (request.ListFile != null)
            {
                foreach (var item in request.ListFile)
                {
                    var detail = new CheckinListImageDTO();
                    string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                    //string appPath = HttpContext.Current.Request.ApplicationPath;
                    string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
                    //System.Web.Hosting.HostingEnvironment.MapPath(path);

                    //string fullPath = Path.Combine(programPath, "fsdUpload", pathSaveToServer);
                    fileService.SaveFile(item, pathSaveToServer); //fullPath
                    String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                    detail.Path = pathSaveDb;
                    detail.NameImage = item.FileName;
                    detail.CodeImage = Guid.NewGuid().ToString();
                    request.Image.Add(detail);
                }
            }

            var result = await this.todosDAL.CreateRequestOpenStore(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<CommonResponse> UpdateRequestOpenStore(UpdateRequestOpenStoreRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            request.Image = new List<CheckinListImageDTO>();

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();

            if (request.ListFile != null)
            {
                foreach (var item in request.ListFile)
                {
                    var detail = new CheckinListImageDTO();
                    string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                    //string appPath = HttpContext.Current.Request.ApplicationPath;
                    string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
                    //System.Web.Hosting.HostingEnvironment.MapPath(path);

                    //string fullPath = Path.Combine(programPath, "fsdUpload", pathSaveToServer);
                    fileService.SaveFile(item, pathSaveToServer); //fullPath
                    String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                    detail.Path = pathSaveDb;
                    detail.NameImage = item.FileName;
                    detail.CodeImage = Guid.NewGuid().ToString();
                    request.Image.Add(detail);
                }
            }

            var result = await this.todosDAL.UpdateRequestOpenStore(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> CancelRequestOpenStore(string idTour, string idRequestOpenStore)
        {

            var result = await this.todosDAL.CancelRequestOpenStore(idTour,idRequestOpenStore);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> CreateNewTicket(CreatNewTicketRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            request.Image = new List<CheckinListImageDTO>();

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();

            if (request.ListFile != null)
            {
                foreach (var item in request.ListFile)
                {
                    var detail = new CheckinListImageDTO();
                    string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                    //string appPath = HttpContext.Current.Request.ApplicationPath;
                    string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
                    //System.Web.Hosting.HostingEnvironment.MapPath(path);

                    //string fullPath = Path.Combine(programPath, "fsdUpload", pathSaveToServer);
                    fileService.SaveFile(item, pathSaveToServer); //fullPath
                    String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                    detail.Path = pathSaveDb;
                    detail.NameImage = item.FileName;
                    detail.CodeImage = Guid.NewGuid().ToString();
                    request.Image.Add(detail);
                }
            }

            var result = await this.todosDAL.CreateNewTicket(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> SearchListCheckIn(string dateTime, string searchKey, int page_index, int page_count)
        {

            var result = await this.todosDAL.SearchListCheckIn(userInfoCache.UserId,dateTime, searchKey, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CheckInResponse> GetListCheckin(CheckinRequest request)
        {
            if (request.userId != null && request.userId != "null" && request.userId != "0" && request.userId != "" )
            {
                request.UserId = int.Parse(request.userId);
            }
            else
            {
                request.UserId = userInfoCache.UserId;
            }
            //request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.todosDAL.GetListCheckin(request);

            if (result.IsSucceeded == true)
            {
                return new CheckInResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    ListCheckInToDay = result.ListCheckInToDay,
                    TotalPage = result.TotalPage
                    //ListCheckInOther = result.ListCheckInOther
                };
            }
            else
            {
                return new CheckInResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DetailCheckInResponse> GetDetailCheckin(string idCheckIn,string idCustomer)
        {
            var result = await this.todosDAL.GetDetailCheckin(idCheckIn, idCustomer);

            if (result.IsSucceeded == true)
            {
                return new DetailCheckInResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Master = result.Master,
                    ListAlbum = result.ListAlbum,
                    ListTicket = result.ListTicket
                };
            }
            else
            {
                return new DetailCheckInResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<ListRequestOpenStoreResponse> GetListRequestOpenStore(string dateForm, string dateTo, string district, int status,string dateTime, int page_index, int page_count)
        {
            long UserId = userInfoCache.UserId;
            var result = await this.todosDAL.GetListRequestOpenStore(dateForm,dateTo,district,status,dateTime, UserId, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new ListRequestOpenStoreResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    listKhuVuc = result.listKhuVuc
                };
            }
            else
            {
                return new ListRequestOpenStoreResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DetailRequestOpenStoreResponse> GetListRequestOpenStoreDetail(string idRequestOpenStore)
        {
            long UserId = userInfoCache.UserId;
            var result = await this.todosDAL.GetListRequestOpenStoreDetail(idRequestOpenStore,UserId);

            if (result.IsSucceeded == true)
            {
                return new DetailRequestOpenStoreResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    DetailRequestOpenStore = result.DetailRequestOpenStore,
                    Roles = result.Roles
                };
            }
            else
            {
                return new DetailRequestOpenStoreResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetTourCheckin(string searchKey, int page_index, int page_count)
        {
            string unitId = userInfoCache.UnitId;
            long UserId = userInfoCache.UserId;
            var result = await this.todosDAL.GetTourCheckin(searchKey, page_index, page_count, unitId, UserId);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListStoreForm(string searchKey, int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListStoreForm(searchKey, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<DynamicResponse> GetListTypeStore(string searchKey, int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListTypeStore(searchKey, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<DynamicResponse> GetListProvince(string province, string dictrict, int page_index, int page_count, string idArea)
        {

            var result = await this.todosDAL.GetListProvince(province,dictrict ,page_index, page_count, idArea);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<DynamicResponse> GetListArea(string searchKey, int page_index, int page_count)
        {
            long UserId = userInfoCache.UserId;
            var result = await this.todosDAL.GetListArea(searchKey, page_index, page_count, UserId);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListTypeTicket(int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListTypeTicket(page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DetailListTicketResponse> GetListTicket(string ticketType, string idCheckIn, string idCustomer, int page_index, int page_count)
        {
            long UserId = userInfoCache.UserId;

            var result = await this.todosDAL.GetListTicket(ticketType, idCheckIn, idCustomer,UserId, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DetailListTicketResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DetailListTicketResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListInventoryCheckIn(string idCustomer,string idCheckIn, int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListInventoryCheckIn(idCustomer,idCheckIn ,page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<DynamicResponse> GetListAlbumCheckIn(string idAlbum)
        {

            var result = await this.todosDAL.GetListAlbumCheckIn(idAlbum);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<ListImageCheckInResponse> GetListImageCheckin(string idAlbum, string idCustomer, string idCheckin, int page_index, int page_count)
        {
            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.todosDAL.GetListImageCheckin(idAlbum, idCustomer, idCheckin, page_index, page_count, UserId, UnitId);
            string hostUrl = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.SERVER_LINK].ToString();
            foreach (var item in result.ListImage)
            {
                item.path_l = hostUrl + "/fsdUpload/" + item.path_l;
            }

            if (result.IsSucceeded == true)
            {
                return new ListImageCheckInResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    ListAlbum = result.ListAlbum,
                    ListImage = result.ListImage
                };
            }
            else
            {
                return new ListImageCheckInResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> CreateTicket(CreateTicketRequest request)
        {
            request.userID = userInfoCache.UserId;
            //request.Lang = userInfoCache.Lang;
            //request.UnitId = userInfoCache.UnitId;
            //request.Admin = userInfoCache.Role;

            var result = await this.todosDAL.CreateTicket(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> InventoryControl(InventoryControlRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;

            var result = await this.todosDAL.InventoryControl(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<CommonResponse> UpdateSaleOut(InventoryControlAndSaleOutRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;

            var result = await this.todosDAL.UpdateSaleOut(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> CheckOut(CheckOutRequest request)
        {
            request.Data = new CheckinDTO()
            {
                
                Detail = new List<CheckinListImageDTO>(),
            };

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();

            //"E:\\test\\"
            dynamic myObject;
            if (request.IdAlbum != null) 
            {
                myObject = JValue.Parse(request.IdAlbum);
                foreach (dynamic itemImage in myObject)
                {
                    string codeAlbum = itemImage.maAlbum;
                    string nameFile = itemImage.fileName;

                    foreach (var item in request.ListFile)
                    {
                        if (nameFile == item.FileName.Trim())
                        {
                            var detail = new CheckinListImageDTO();
                            string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                            string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
                            fileService.SaveFile(item, pathSaveToServer); //fullPath
                            String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                            detail.Path = pathSaveDb;
                            detail.NameImage = item.FileName;
                            detail.CodeImage = Guid.NewGuid().ToString();
                            detail.codeAlbum = codeAlbum;
                            request.Data.Detail.Add(detail);
                        }
                    }
                }
            }

            
            //if (request.ListFile != null) {

            //    foreach (var item in request.ListFile)
            //    {
            //        var detail = new CheckinListImageDTO();
            //        string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
            //        string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
            //        fileService.SaveFile(item, pathSaveToServer); //fullPath
            //        String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
            //        detail.Path = pathSaveDb;
            //        detail.NameImage = item.FileName;
            //        detail.CodeImage = Guid.NewGuid().ToString();
            //        request.Data.Detail.Add(detail);
            //    }
            //}

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;

            var result = await this.todosDAL.CheckOut(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> ReportLocation(ReportLocationRequest request)
        {

            //request.Detail = new List<CheckinListImageDTO>();

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();

            ////"E:\\test\\"

            //if (request.ListFile != null)
            //{
            //    foreach (var item in request.ListFile)
            //    {
            //        var detail = new CheckinListImageDTO();
            //        string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
            //        string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
            //        fileService.SaveFile(item, pathSaveToServer); //fullPath
            //        String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
            //        detail.Path = pathSaveDb;
            //        detail.NameImage = item.FileName;
            //        detail.CodeImage = Guid.NewGuid().ToString();
            //        detail.codeAlbum = null;
            //        request.Detail.Add(detail);
            //    }
            //}

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;
            request.Datetime = DateTime.Now;

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();
            IFormFile file;

            if (request.Image != null && request.Image != "")
            {
                byte[] bytes = Convert.FromBase64String(request.Image);
                MemoryStream stream = new MemoryStream(bytes);

                file = new FormFile(stream, 0, bytes.Length, request.NameFile, request.NameFile);
                string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                string pathSaveToServer = fileService.createPathFile(fsdUploadPath, file);
                fileService.SaveFile(file, pathSaveToServer);
                String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                request.CodeImage = Guid.NewGuid().ToString();
                request.Path = pathSaveDb;
            }

            var result = await this.todosDAL.ReportLocation(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }


        public async Task<CommonResponse> ReportLocationV2(ReportLocationRequestV2 request)
        {
            request.Detail = new List<CheckinListImageDTO>();
            string programPath = Directory.GetCurrentDirectory(); //"F:\\Project2023\\API\\DMS\\Thien-Vuong\\Version 1.0.0\\SSE.ServerAPI"
            //string programPath2 = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString(); //"E:\\APP_API\\thienvuong\\"

            if (request.ListFile != null)
            {
                foreach (var item in request.ListFile)
                {
                    var detail = new CheckinListImageDTO();
                    string fsdUploadPath = Path.Combine(programPath, "fsdUpload");
                    string pathSaveToServer = fileService.createPathFile(fsdUploadPath, item);
                    fileService.SaveFile(item, pathSaveToServer); //fullPath
                    String pathSaveDb = pathSaveToServer.Split("fsdUpload\\")[1];
                    detail.Path = pathSaveDb;
                    detail.NameImage = item.FileName;
                    detail.CodeImage = Guid.NewGuid().ToString();
                    detail.codeAlbum = null;
                    request.Detail.Add(detail);
                }
            }

            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;
            request.Datetime = request.Datetime.Split('.')[0];

            var result = await this.todosDAL.ReportLocationV2(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DailyJobOfflineResponse> DailyJobOffline(string dateTime)
        {

            var result = await this.todosDAL.DailyJobOffline(dateTime, userInfoCache.UserId);

            if (result.IsSucceeded == true)
            {
                return new DailyJobOfflineResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    ListAlbum = result.ListAlbum,
                    ListCustomer = result.ListCustomer,
                    ListTicket = result.ListTicket
                };
            }
            else
            {
                return new DailyJobOfflineResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> TimeKeepingCreate(TimeKeepingRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;

            var result = await this.todosDAL.TimeKeepingCreate(request);

            if (result.IsSucceeded == true)
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            else
            {
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<ListTimeKeepingHistoryResponse> TimeKeepingHistory(TimeKeepingHistoryRequest request)
        {
            request.UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

     

            var result = await this.todosDAL.TimeKeepingHistory(request);

            if (result.IsSucceeded == true)
            {
                return new ListTimeKeepingHistoryResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Master = result.Master,
                    listTimeKeepingHistory = result.listTimeKeepingHistory
                };
            }
            else
            {
                return new ListTimeKeepingHistoryResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<DynamicResponse> CheckinHistory(CheckinHistoryRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.UnitId = userInfoCache.UnitId;
            request.Admin = userInfoCache.Role;

            var result = await this.todosDAL.CheckinHistory(request);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListHistoryActionEmployee(string dateFrom, string dateTo, string idCustomer, int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListHistoryActionEmployee(dateFrom,dateTo,idCustomer, userInfoCache.UserId, userInfoCache.UnitId,page_index,page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        
        public async Task<DynamicResponse> GetListHistoryTicket(string dateFrom, string dateTo, string idCustomer,string employeeCode, int status, int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListHistoryTicket(dateFrom,dateTo,idCustomer, employeeCode, status, userInfoCache.UserId, userInfoCache.UnitId,page_index,page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    TotalPage = result.TotalPage
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListHistorySaleOut(string dateFrom, string dateTo, string idCustomer, int idTransaction, int page_index, int page_count)
        {

            var result = await this.todosDAL.GetListHistorySaleOut(dateFrom, dateTo, idCustomer, idTransaction, userInfoCache.UserId, userInfoCache.UnitId, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    TotalPage = result.TotalPage
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetDetailHistorySaleOut(string stt_rec, string invoice_date)
        {

            var result = await this.todosDAL.GetDetailHistorySaleOut(stt_rec,invoice_date, userInfoCache.UserId, userInfoCache.UnitId);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data,
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> ListKPISummaryByDay(string dateFrom, string dateTo)
        {

            var result = await this.todosDAL.ListKPISummaryByDay(dateFrom, dateTo, userInfoCache.UserId, userInfoCache.UnitId, userInfoCache.StoreId);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DetailHistoryTicketResponse> GetDetailHistoryTicket(string idTicket)
        {
            var result = await this.todosDAL.GetDetailHistoryTicket(idTicket, userInfoCache.UserId, userInfoCache.UnitId);

            if (result.IsSucceeded == true)
            {
                return new DetailHistoryTicketResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DetailHistoryTicketResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> ListStateOpenStore(string keySearch, int page_index, int page_count)
        {

            var result = await this.todosDAL.ListStateOpenStore(keySearch, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListSaleOutCompleted(string idAgency,string dateForm, string dateTo, int page_index, int page_count)
        {
            var result = await this.todosDAL.GetListSaleOutCompleted(idAgency,userInfoCache.UserId, userInfoCache.UnitId, dateForm, dateTo, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    TotalPage = result.TotalPage
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> DetailSaleOutCompleted(string sct, string invoiceDate)
        {
            var result = await this.todosDAL.DetailSaleOutCompleted(userInfoCache.UserId, userInfoCache.UnitId, sct, invoiceDate);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                };
            }
            else
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CommonResponse> RefundCreateSaleOutV1(RefundSaleOutCreateV1Request request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.todosDAL.RefundCreateSaleOutV1(request);

            if (result.IsSucceeded == true)
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            else
                return new CommonResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<DynamicResponse> CreateTaskFromCustomer(string idCustomer)
        {
            var result = await this.todosDAL.CreateTaskFromCustomer(idCustomer, userInfoCache.UserId);

            if (result.IsSucceeded == true)
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            else
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<DynamicResponse> DanhSachCauHoi(string searchKey, int page_index, int page_count)
        {
            var result = await this.todosDAL.DanhSachCauHoi(searchKey, page_index, page_count);

            if (result.IsSucceeded == true)
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    TotalPage = result.TotalPage
                };
            else
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<DynamicResponse> DanhSachCauTraLoi(string stt_rec, string ma_cau_hoi)
        {
            var result = await this.todosDAL.DanhSachCauTraLoi(stt_rec, ma_cau_hoi);

            if (result.IsSucceeded == true)
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            else
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
    }
}