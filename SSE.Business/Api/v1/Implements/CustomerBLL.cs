using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.Api.v1.Responses.Customer;
using SSE.Common.DTO.v1;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Files;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    internal class CustomerBLL : ICustomerBLL
    {
        private readonly IFileService fileService;
        private readonly ICustomerDAL customerDAL;
        private UserInfoCache userInfoCache;
        private readonly IConfiguration configuration;

        public CustomerBLL(ICustomerDAL customerDAL,
                       IUserBLLService userBLLService,
                       IHttpContextAccessor httpContextAccessor, IFileService fileService, IConfiguration configuration)
        {
            this.customerDAL = customerDAL;
            this.fileService = fileService;
            this.configuration = configuration;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }

        public async Task<CustomerListResponse> CustomerList(CustomerListResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.customerDAL.CustomerList(request);
            if (result.IsSucceeded == true)
                return new CustomerListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data,
                    PageIndex = request.PageIndex,
                    TotalCount = result.TotalCount
                };
            else
                return new CustomerListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<CustomerListResponse> SearchCustomerList(CustomerListResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.customerDAL.SearchCustomerList(request);
            if (result.IsSucceeded == true)
                return new CustomerListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data,
                    PageIndex = request.PageIndex,
                    TotalCount = result.TotalCount
                };
            else
                return new CustomerListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<CustomerCreateResponse> CustomerCreate(CustomerCreateResquest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.customerDAL.CustomerCreate(request);

            if (result.IsSucceeded == true)
                return new CustomerCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            else
                return new CustomerCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<CustomerDetailResponse> CustomerInfo(string codeCustomer)
        {

            ///, long UserId, string unitID, string storeID, string lang, int admin
            //request.UserId = userInfoCache.UserId;
            //request.Lang = userInfoCache.Lang;
            //request.Admin = userInfoCache.Role;
            //request.UnitId = userInfoCache.UnitId;

            var result = await this.customerDAL.CustomerInfo(codeCustomer, userInfoCache.UserId, userInfoCache.UnitId, userInfoCache.StoreId, userInfoCache.Lang, 1);

            if (result.IsSucceeded == true)
                return new CustomerDetailResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data
                };
            else
                return new CustomerDetailResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<ListCustomerCareResponse> ListCustomerCare(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
      
            var result = await this.customerDAL.ListCustomerCare(userInfoCache.UserId,userInfoCache.UnitId, dateForm, dateTo, idCustomer, page_index, page_count);

            if (result.IsSucceeded == true)
            {
                return new ListCustomerCareResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    ListCustomerCare = result.ListCustomerCare,

                    TotalPage = result.TotalPage
                };
            }
            else
            {
                return new ListCustomerCareResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<CustomerCreateResponse> CustomerCareCreate(CustomerCareCreateResquest request)
        {

            request.Detail = new List<CheckinListImageDTO>(); 

            //string programPath = this.configuration[CONFIGURATION_KEYS.SERVER_INFO + ":" + CONFIGURATION_KEYS.IMG_PATH].ToString();
            string programPath = Directory.GetCurrentDirectory();
            //"E:\\test\\"

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
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.customerDAL.CustomerCareCreate(request);

            if (result.IsSucceeded == true)
                return new CustomerCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                };
            else
                return new CustomerCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<DynamicResponse> ListCustomerAction(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            var result = await this.customerDAL.ListCustomerAction(userInfoCache.UserId, userInfoCache.UnitId, dateForm, dateTo, idCustomer, page_index, page_count);

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
    }
}