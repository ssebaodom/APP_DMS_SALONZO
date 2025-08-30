using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Interfaces;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Responses.Order;
using SSE.Core.Common.Entities;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSE.Business.Api.v1.Implements
{
    internal class OrderBLL : IOrderBLL
    {
        private readonly IOrderDAL orderDAL;
        private UserInfoCache userInfoCache;

        public OrderBLL(IOrderDAL orderDAL,
                       IUserBLLService userBLLService,
                       IHttpContextAccessor httpContextAccessor)
        {
            this.orderDAL = orderDAL;
            userInfoCache = userBLLService.GetUserFromContext(httpContextAccessor.HttpContext);
        }

        /// <summary>
        /// Tìm kiếm sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ListDetailApprovalResponse> SearchItem(OrderItemSearchRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.orderDAL.SearchItem(request);
            if (result.IsSucceeded == true)
                return new ListDetailApprovalResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Result,
                    DescriptField = result.DescriptField,
                    PageIndex = request.PageIndex,
                    TotalCount = result.TotalCount
                };
            else
                return new ListDetailApprovalResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<ListDetailApprovalResponse> SearchItemV2(OrderItemSearchV2Request request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;

            var result = await this.orderDAL.SearchItemV2(request);
            if (result.IsSucceeded == true)
                return new ListDetailApprovalResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Result,
                    DescriptField = result.DescriptField,
                    PageIndex = request.PageIndex,
                    TotalCount = result.TotalCount
                };
            else
                return new ListDetailApprovalResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<OrderItemScanResponse> ScanItem(OrderItemScanRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.orderDAL.ScanItem(request);

            if (result.IsSucceeded == true)
                return new OrderItemScanResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Result
                };
            else
                return new OrderItemScanResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<OrderCreateResponse> CreateOrder(OrderCreateRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.OrderCreate(request);

            if (result.IsSucceeded == true)
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        
        public async Task<OrderCreateResponse> CreateOrderV3(OrderCreateV3Request request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.CreateOrderV3(request);

            if (result.IsSucceeded == true)
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        
        public async Task<OrderCreateResponse> RefundCreateOrderV1(RefundOrderCreateV1Request request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.RefundCreateOrderV1(request);

            if (result.IsSucceeded == true)
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<OrderListResponse> OrderList(OrderListRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.orderDAL.OrderList(request);

            if (result.IsSucceeded == true)
                return new OrderListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Values = result.Values,
                    PageIndex = request.PageIndex
                };
            else
                return new OrderListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<OrderListResponse> GetHistoryOrder(OrderListRequest request)
        {
            request.UserId = long.Parse(request.UserCode.ToString().Trim());
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetHistoryOrder(request);

            if (result.IsSucceeded == true)
                return new OrderListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Values = result.Values,
                    PageIndex = request.PageIndex
                };
            else
                return new OrderListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<OrderCartResponse> OrderCart(OrderCartRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;

            var result = await this.orderDAL.OrderCart(request);

            if (result.IsSucceeded == true)
                return new OrderCartResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Items = result.Items,
                    Total = result.Total
                };
            else
                return new OrderCartResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<ItemDetailResponse> ItemDetail(string ItemCode, string Currency)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.ItemDetail(ItemCode, Currency, UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
                return new ItemDetailResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data
                };
            else
                return new ItemDetailResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<ItemGroupResponse> ItemGroup(int GroupType, int Level)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.ItemGroup(GroupType, Level, UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
                return new ItemGroupResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data
                };
            else
                return new ItemGroupResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<BannerAdResponse> BannerAdd()
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetBannerAd(UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
                return new BannerAdResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data
                };
            else
                return new BannerAdResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<OrderCountResponse> OrderCount()
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            int result = await this.orderDAL.OrderCount(UserId, UnitId, Lang, Admin);

            return new OrderCountResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = result
            };
        }
        public async Task<OrderDetailResponse> OrderDetail(string stt_rec)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.OrderDetail(UserId, UnitId, Admin,stt_rec);

            OrderDetailResponseData data = new OrderDetailResponseData();
            data.master = result.master;
            data.line_items = result.listProduct;
            data.infoPayment = result.infoPayment;

            if (result.IsSucceeded == true)
                return new OrderDetailResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data =data
                };
            else
                return new OrderDetailResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<OrderUpdateResponse> CreateUpdate(OrderUpdateRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.OrderUpdate(request);

            if (result.IsSucceeded == true)
                return new OrderUpdateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new OrderUpdateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<OrderCancelResponse> OrderCancel(string stt_rec)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.OrderCancel(UserId, UnitId, Admin, stt_rec);


            if (result.IsSucceeded == true)
                return new OrderCancelResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                };
            else
                return new OrderCancelResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<DynamicResponse> GetProductList(GetProductListRequest request)
        {
            var result = await this.orderDAL.GetProductList(request);

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

        public async Task<DynamicResponse> GetProduceOrderList(GetProduceOrderListRequest request)
        {
            request.UnitId = userInfoCache.UnitId;

            var result = await this.orderDAL.GetProduceOrderList(request);

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
        public async Task<DynamicResponse> GetCommandList(GetCommandListRequest request)
        {
            request.UnitId = userInfoCache.UnitId;

            var result = await this.orderDAL.GetCommandList(request);

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
        public async Task<DynamicResponse> GetCommandDetail(string stt_rec,string ma_kh)
        {
            var result = await this.orderDAL.GetCommandDetail(stt_rec,ma_kh);

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
        public async Task<OrderCreateResponse> TaoPhieuTKCD(TaoPhieuTKCDRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.TaoPhieuTKCD(request);

            if (result.IsSucceeded == true)
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }

        public async Task<OrderCreateResponse> UpdateTKCDDrafts(UpdateTKCDDraftsRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.UpdateTKCDDraftsTKCD(request);

            if (result.IsSucceeded == true)
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new OrderCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<DynamicResponse> FindProductAllStore(string codeProduct)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.FindProductAllStore(codeProduct, UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data
                };
            else
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }

        public async Task<ListStoreAndGroupResponse> GetListStoreAndGroup(string codeProduct, string listKeyGroup, int checkGroup, int checkStock, int checkStockEmployee)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetListStoreAndGroup(codeProduct, UserId, UnitId, Lang, Admin, listKeyGroup, checkGroup, checkStock, checkStockEmployee);

            if (result.IsSucceeded == true)
            {
                return new ListStoreAndGroupResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    listGroup = result.listGroup,
                    listStore = result.listStore,
                   
                };
            }
            else
            {
                return new ListStoreAndGroupResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListTax()
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            string Lang = userInfoCache.Lang;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetListTax( UserId, UnitId, Lang, Admin);

            if (result.IsSucceeded == true)
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = result.Data
                };
            else
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        /// <summary>
        /// Tạo ra một chuỗi ngẫu nhiên với độ dài cho trước
        /// </summary>
        /// <param name="size">Kích thước của chuỗi </param>
        /// <param name="lowerCase">Nếu đúng, tạo ra chuỗi chữ thường</param>
        /// <returns>Random string</returns>
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public async Task<DNCCreateResponse> TaoPhieuDNC(TaoPhieuDNCRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            request.Encode = RandomString(32,true);
            request.Ticket = RandomString(32,true);

            var result = await this.orderDAL.TaoPhieuDNC(request);

            if (result.IsSucceeded == true)
                return new DNCCreateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new DNCCreateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<DynamicResponse> GetDNCList(GetDNCListRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.GetDNCList(request);

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
        public async Task<DNCDetailResponse> DNCDetail(string stt_rec)
        {

            var result = await this.orderDAL.GetDNCDetail(stt_rec);

            DNCDetailResponseData data = new DNCDetailResponseData();
            data.master = result.master;
            data.detail = result.detail;

            if (result.IsSucceeded == true)
                return new DNCDetailResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = data
                };
            else
                return new DNCDetailResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<DNCUpdateResponse> UpdateDNC(UpdateDNCRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.UpdateDNC(request);

            if (result.IsSucceeded == true)
                return new DNCUpdateResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new DNCUpdateResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<DNCAuthorizeResponse> AuthorizeDNC(DNCAuthorizeRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.AuthorizeDNC(request);

            if (result.IsSucceeded == true)
                return new DNCAuthorizeResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new DNCAuthorizeResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
        }
        public async Task<DynamicResponse> GetSiteList(GetSiteListRequest request)
        {
            request.UnitID = userInfoCache.UnitId;

            var result = await this.orderDAL.GetSiteList(request);

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

        public async Task<DynamicResponse> GetListDNCHistory(GetDNCListHistoryRequest request)
        {
            request.UnitID = userInfoCache.UnitId;
            request.UserID = userInfoCache.UserId;

            var result = await this.orderDAL.GetListDNCHistory(request);

            if (result.IsSucceeded == true)
            {
                return new DynamicResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                    TotalPage = result.TotalPage,
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

        public async Task<ListVVHDResponse> GetListVVHD(string idCustomer)
        {
            long UserId = userInfoCache.UserId;
            string Lang = userInfoCache.Lang;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetListVVHD(idCustomer, UserId,  UnitId,  Lang,  Admin);

            if (result.IsSucceeded == true)
            {
                return new ListVVHDResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    list_vv = result.list_vv,
                    list_hd = result.list_hd
                };
            }
            else
            {
                return new ListVVHDResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> GetListOrderCompleted(string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            var result = await this.orderDAL.GetListOrderCompleted(userInfoCache.UserId, userInfoCache.UnitId, dateForm, dateTo, idCustomer, page_index, page_count);

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

        public async Task<DynamicResponse> DetailOrderCompleted(string sct, string invoiceDate)
        {
            var result = await this.orderDAL.DetailOrderCompleted(userInfoCache.UserId, userInfoCache.UnitId, sct, invoiceDate);

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

        public async Task<DynamicResponse> GetListRefundOrder(string status, string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            var result = await this.orderDAL.GetListRefundOrder(status,userInfoCache.UserId, userInfoCache.UnitId, dateForm, dateTo, idCustomer, page_index, page_count);

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

        public async Task<DynamicResponse> DetailRefundOrder(string sct, string invoiceDate)
        {
            var result = await this.orderDAL.DetailRefundOrder(userInfoCache.UserId, userInfoCache.UnitId, sct, invoiceDate);

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
        
        public async Task<DynamicResponse> GetListStatusOrder(string vcCode)
        {
            string directory = Directory.GetCurrentDirectory();
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string code = json["code"].ToString().Trim();

            var result = await this.orderDAL.GetListStatusOrder(userInfoCache.UserId, vcCode??code);

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
        
        public async Task<DynamicResponse> GetListApproveOrder(string dateForm, string dateTo, int page_index, int page_count)
        {
            var result = await this.orderDAL.GetListApproveOrder(userInfoCache.UserId, userInfoCache.UnitId, dateForm, dateTo, page_index, page_count);

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
        public async Task<ItemFromBarCodeResponse> GetInforItemForBarCode(string barcode, string palet)
        {
            var result = await this.orderDAL.GetInforItemForBarCode(userInfoCache.UserId, userInfoCache.UnitId,barcode,palet);

            if (result.IsSucceeded == true)
            {
                return new ItemFromBarCodeResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    listItem = result.listItem,
                };
            }
            else
            {
                return new ItemFromBarCodeResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        public async Task<GetItemHolderDetailResponse> GetItemHolderDetail(string stt_rec)
        {
            var result = await this.orderDAL.GetItemHolderDetail(userInfoCache.UserId, userInfoCache.UnitId,stt_rec);

            if (result.IsSucceeded == true)
            {
                return new GetItemHolderDetailResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    master  =  result.master,
                    listItem = result.listItem,
                };
            }
            else
            {
                return new GetItemHolderDetailResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        public async Task<ListInfoCardResponse> GetListInfoCard(string stt_rec, string key)
        {
            var result = await this.orderDAL.GetListInfoCard(userInfoCache.UserId, userInfoCache.UnitId, stt_rec, key);

            if (result.IsSucceeded == true)
            {
                List<dynamic> listMaster = result.master;
               
                string codeProvider = "";
                foreach (var d in listMaster)
                {
                    var obj = d as IDictionary<string, object>;
                    if (obj.ContainsKey("ma_ncc"))
                    {
                        codeProvider = obj["ma_ncc"].ToString();
                    }
                }


                if (codeProvider != "" && codeProvider != "null" && codeProvider != null)
                {
                    var result2 = await this.orderDAL.GetFormatProvider(userInfoCache.UserId, userInfoCache.UnitId, codeProvider);
                    if (result.IsSucceeded == true) {
                        return new ListInfoCardResponse
                        {
                            StatusCode = StatusCodes.Status200OK,
                            Message = result.Message,
                            master = result.master,
                            listItem = result.listItem,
                            ruleActionInfoCard = result.ruleActionInfoCard,
                            formatProvider = result2.formatProvider,
                        };
                    }
                    else
                    {
                        return new ListInfoCardResponse
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = result.Message
                        };
                    }    
                }
                else
                {
                    return new ListInfoCardResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = result.Message,
                        master = result.master,
                        listItem = result.listItem,
                        ruleActionInfoCard = result.ruleActionInfoCard,
                    };
                }
            }
            else
            {
                return new ListInfoCardResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        public async Task<CommonsResponse> DeleteItemHolder(string stt_rec)
        {
            var result = await this.orderDAL.DeleteItemHolder(userInfoCache.UserId, userInfoCache.UnitId, stt_rec);

            if (result.IsSucceeded == true)
            {
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                };
            }
            else
            {
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }

        public async Task<DynamicResponse> ApproveOrder(string sctRec)
        {
            var result = await this.orderDAL.ApproveOrder(userInfoCache.UserId, userInfoCache.UnitId, sctRec);

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
        public async Task<CommonsResponse> UpdateQuantityWarehouseDelivery(UpdateQuantityWarehouseDeliveryRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.UpdateQuantityWarehouseDelivery(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> CreateDeliveryCard(CreateDeliveryRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.CreateDeliveryCard(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> UpdateItemBarcode(UpdateItemBarcodeRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.UpdateItemBarcode(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> StockTransferConfirm(UpdateItemBarcodeRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.StockTransferConfirm(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> UpdatePostPNF(UpdateQuantityPostPNFRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.UpdatePostPNF(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> CreateRefundBarcodeHistory(UpdateQuantityPostPNFRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.CreateRefundBarcodeHistory(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> ItemLocaionModify(ItemLocaionModifyRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.ItemLocaionModify(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<CommonsResponse> CreateItemHolder(CreateItemHolderRequest request)
        {
            request.UserId = userInfoCache.UserId;
            request.Lang = userInfoCache.Lang;
            request.Admin = userInfoCache.Role;
            request.UnitId = userInfoCache.UnitId;
            request.StoreId = userInfoCache.StoreId;

            var result = await this.orderDAL.CreateItemHolder(request);

            if (result.IsSucceeded == true)
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };
            else
                return new CommonsResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
        }
        public async Task<GetDynamicListResponse> GetDynamicList(string voucher_code, string status, string dateFrom, string dateTo)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetDynamicList(UserId,UnitId, voucher_code, status, dateFrom, dateTo);

            if (result.IsSucceeded == true)
            {
                return new GetDynamicListResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    listVoucher = result.listVoucher,
                    listStatus = result.listStatus,
                    Message = result.Message,
                };
            }
            else
            {
                return new GetDynamicListResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = result.Message
                };
            }
        }
        public async Task<DynamicResponse> GetHistoryDNNK(string stt_rec)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetHistoryDNNK(UserId,UnitId, stt_rec);

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
        public async Task<DynamicResponse> GetBarcodeValue(string barcode)
        {
            long UserId = userInfoCache.UserId;
            string UnitId = userInfoCache.UnitId;
            int Admin = userInfoCache.Role;

            var result = await this.orderDAL.GetBarcodeValue(UserId,UnitId, barcode);

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
    }
}