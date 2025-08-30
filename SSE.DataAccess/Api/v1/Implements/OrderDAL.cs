using Dapper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Responses.Order;
using SSE.Common.Api.v1.Results.Customer;
using SSE.Common.Api.v1.Results.Order;
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
    public class OrderDAL : IOrderDAL
    {
        private readonly IDapperService dapperService;

        public OrderDAL(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }

        public async Task<OrderItemSearchResult> SearchItem(OrderItemSearchRequest orderItemSearch)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<OrderItemSearchRequest>(orderItemSearch);
            GridReader gridReader = await dapperService.QueryMultipleAsync("[app_Order_SearchItem]", parameters);
          
            var result = await gridReader.ReadAsync<OrderItemInfoDTO>();
            var descript = await gridReader.ReadAsync<DescriptFieldDTO>();
            int TotalCount = gridReader.ReadFirst<int>();

            if (result == null)
            {
                return new OrderItemSearchResult
                {
                    IsSucceeded = false
                };
            }

            return new OrderItemSearchResult
            {
                IsSucceeded = true,
                Result = result,
                DescriptField = descript,
                TotalCount = TotalCount
            };
        }

        public async Task<OrderItemSearchResult> SearchItemV2(OrderItemSearchV2Request orderItemSearch)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<OrderItemSearchV2Request>(orderItemSearch);
            GridReader gridReader = await dapperService.QueryMultipleAsync("[app_Order_SearchItem_v2]", parameters);
           
            var result = await gridReader.ReadAsync<OrderItemInfoDTO>();
            var descript = await gridReader.ReadAsync<DescriptFieldDTO>();
            int TotalCount = gridReader.ReadFirst<int>();

            if (result == null)
            {
                return new OrderItemSearchResult
                {
                    IsSucceeded = false
                };
            }

            return new OrderItemSearchResult
            {
                IsSucceeded = true,
                Result = result,
                DescriptField = descript,
                TotalCount = TotalCount
            };
        }

        public async Task<OrderItemScanResult> ScanItem(OrderItemScanRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<OrderItemScanRequest>(request);

            //var result = await dapperService.QueryFirstOrDefaultAsync<OrderItemInfoDTO>("[app_Order_ScanItem]", parameters);

            GridReader gridReader = await dapperService.QueryMultipleAsync("[app_Order_ScanItem]", parameters);

            var result = await gridReader.ReadFirstAsync<OrderItemInfoDTO>();            

            if (result == null)
            {
                UserNotifyDTO userNotify = gridReader.ReadFirstOrDefault<UserNotifyDTO>();

                return new OrderItemScanResult
                {
                    IsSucceeded = false
                };
            }

            return new OrderItemScanResult
            {
                IsSucceeded = true,
                Result = result
            };
        }

        public async Task<OrderCreateResult> OrderCreate(OrderCreateRequest request)
        {
            string str = OrderCreateHelp.OrderCreate_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];
                       
            if (code == (int)QueryExcuteCode.Success)
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        
        public async Task<OrderCreateResult> CreateOrderV3(OrderCreateV3Request request)
        {
            string str = OrderCreateHelp.OrderCreate_GetQueryV3(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];
                       
            if (code == (int)QueryExcuteCode.Success)
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        
        public async Task<OrderCreateResult> RefundCreateOrderV1(RefundOrderCreateV1Request request)
        {
            string str = OrderCreateHelp.RefundOrderCreate_GetQueryV1(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];
                       
            if (code == (int)QueryExcuteCode.Success)
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }

        public async Task<OrderListResult> OrderList(OrderListRequest executeRequest)
        {
            //DynamicParameters parameters = dapperService.CreateDynamicParameters<OrderListRequest>(executeRequest);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", executeRequest.UserId);
            parameters.Add("@Lang", executeRequest.Lang);
            parameters.Add("@Admin", executeRequest.Admin);
            parameters.Add("@UnitId", executeRequest.UnitId);
            parameters.Add("@StoreId", executeRequest.StoreId);

            parameters.Add("@LetterTypeId", executeRequest.LetterTypeId);
            parameters.Add("@PageIndex", executeRequest.PageIndex);
            parameters.Add("@Status", executeRequest.Status);
            parameters.Add("@TimeFilter", executeRequest.TimeFilter);
            parameters.Add("@DateFrom", executeRequest.DateFrom);
            parameters.Add("@DateTo", executeRequest.DateTo);
            parameters.Add("@LastPage", executeRequest.LastPage);
            parameters.Add("@FirstElement", executeRequest.FirstElement);
            parameters.Add("@LastElement", executeRequest.LastElement);
            parameters.Add("@TotalRec", executeRequest.TotalRec);

            string ProceName = "[app_Letter_List]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);

            List<ReportHeaderDescDTO> reportHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            int TotalCount = gridReader.ReadFirst<int>();
            List<OrderListDTO> data = gridReader.Read<OrderListDTO>().ToList();
            foreach(OrderListDTO d in data)
            {
                DynamicParameters parameters2 = new DynamicParameters();
                parameters2.Add("@stt_rec", d.stt_rec);

                string ProceName2 = "app_Order_GetStore";
                GridReader gridReader2 = await dapperService.QueryMultipleAsync(ProceName2, parameters2);
                List<GetStoreInfoDTO> store2 = gridReader2.Read<GetStoreInfoDTO>().ToList();
                if(store2.Count > 0)
                {
                    d.dept_id = store2[0].ma_kho;
                    d.dept_name = store2[0].ten_kho;
                }
            }

            List<LetterStatusDTO> letterStatuses = gridReader.Read<LetterStatusDTO>().ToList();

            return new OrderListResult
            {
                IsSucceeded = true,
                Values = data,
                HeaderDesc = reportHeaders,
                Status = letterStatuses,
                TotalCount = TotalCount
            };
        }
        public async Task<OrderListResult> GetHistoryOrder(OrderListRequest executeRequest)
        {
            //DynamicParameters parameters = dapperService.CreateDynamicParameters<OrderListRequest>(executeRequest);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", executeRequest.UserId);
            parameters.Add("@Lang", executeRequest.Lang);
            parameters.Add("@Admin", executeRequest.Admin);
            parameters.Add("@UnitId", executeRequest.UnitId);
            parameters.Add("@StoreId", executeRequest.StoreId);

            parameters.Add("@LetterTypeId", executeRequest.LetterTypeId);
            parameters.Add("@PageIndex", executeRequest.PageIndex);
            parameters.Add("@Status", executeRequest.Status);
            parameters.Add("@TimeFilter", executeRequest.TimeFilter);
            parameters.Add("@DateFrom", executeRequest.DateFrom);
            parameters.Add("@DateTo", executeRequest.DateTo);
            parameters.Add("@LastPage", executeRequest.LastPage);
            parameters.Add("@FirstElement", executeRequest.FirstElement);
            parameters.Add("@LastElement", executeRequest.LastElement);
            parameters.Add("@TotalRec", executeRequest.TotalRec);


            string ProceName = "[app_Letter_List]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);


            List<ReportHeaderDescDTO> reportHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            int TotalCount = gridReader.ReadFirst<int>();
            List<OrderListDTO> data = gridReader.Read<OrderListDTO>().ToList();
            foreach(OrderListDTO d in data)
            {
                DynamicParameters parameters2 = new DynamicParameters();
                parameters2.Add("@stt_rec", d.stt_rec);

                string ProceName2 = "app_Order_GetStore";
                GridReader gridReader2 = await dapperService.QueryMultipleAsync(ProceName2, parameters2);
                List<GetStoreInfoDTO> store2 = gridReader2.Read<GetStoreInfoDTO>().ToList();
                if(store2.Count > 0)
                {
                    d.dept_id = store2[0].ma_kho;
                    d.dept_name = store2[0].ten_kho;
                }
            }

            List<LetterStatusDTO> letterStatuses = gridReader.Read<LetterStatusDTO>().ToList();

            return new OrderListResult
            {
                IsSucceeded = true,
                Values = data,
                HeaderDesc = reportHeaders,
                Status = letterStatuses,
                TotalCount = TotalCount
            };
        }
        public async Task<int> OrderCount(long UserId,string UnitId,string Lang,int Admin )
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);
            parameters.Add("@UnitId", UnitId);

            string ProceName = "app_Order_Count";

            int re = await dapperService.ExecuteScalarAsync<int>(ProceName, parameters);
            return re;
        }

        public async Task<OrderDetailResult> OrderDetail(long UserId, string UnitId, int Admin,string stt_rec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Admin", Admin);
            parameters.Add("@LetterId",stt_rec);

            string ProceName = "app_Order_Detail";

            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);
            List<OrderDetailMasterDTO> master = gridReader.Read<OrderDetailMasterDTO>().ToList();
            List<OrderDetailDeResultDTO> listProduct = gridReader.Read<OrderDetailDeResultDTO>().ToList();
            List<OrderDetailPaymentResultDTO> infoPayment = gridReader.Read<OrderDetailPaymentResultDTO>().ToList();
           

            //OrderDetailMasterResultDTO re_master = new OrderDetailMasterResultDTO();
            //List<OrderDetailDeResultDTO> re_detail = new List<OrderDetailDeResultDTO>();

            if (master.Count == 0) return new OrderDetailResult
            {
                IsSucceeded = false
            };
            OrderDetailPaymentResultDTO infoDetailPayment = new OrderDetailPaymentResultDTO();

            OrderDetailMasterResultDTO m = new OrderDetailMasterResultDTO();
            m.stt_rec = master[0].stt_rec;
            m.so_ct = master[0].so_ct;
            m.ngay_ct = master[0].ngay_ct;
            m.ma_kh = master[0].ma_kh;
            m.ten_kh = master[0].ten_kh;
            m.t_tc_tien_nt2 = master[0].t_tc_tien_nt2;
            m.t_ck_tt_nt = master[0].t_ck_tt_nt;
            m.t_tt_nt = master[0].t_tt_nt;
            m.status = master[0].status;
            m.ma_kho = master[0].ma_kho;

            m.ma_gd = master[0].ma_gd;
            m.ten_gd = master[0].ten_gd;
            m.ma_daily = master[0].ma_daily;
            m.ten_daily = master[0].ten_daily;
            m.dien_giai = master[0].dien_giai;

            m.HTTT = master[0].HTTT;
            m.han_tt = master[0].han_tt;
            m.kieu_kh = master[0].kieu_kh;

            DisCountInfo discountInfo = new DisCountInfo();
            discountInfo.ma_ck = master[0].ma_ck;
            discountInfo.ten_ck = master[0].ten_ck;
            discountInfo.gt_cl = master[0].gt_cl;
            discountInfo.loai_ct = master[0].loai_ct;
            m.ck = discountInfo;

            infoDetailPayment.t_tien = infoPayment[0].t_tien;
            infoDetailPayment.t_ck_tt_nt = infoPayment[0].t_ck_tt_nt;
            infoDetailPayment.t_tt_nt = infoPayment[0].t_tt_nt;
            infoDetailPayment.t_thue_nt = infoPayment[0].t_thue_nt;

            //foreach(OrderDetailDeDTO d in detail)
            //{
            //    if(d.km_yn != 1)
            //    {
            //        OrderDetailDeResultDTO temp_d = new OrderDetailDeResultDTO();
            //        temp_d.ma_vt = d.ma_vt;
            //        temp_d.ten_vt = d.ten_vt;
            //        temp_d.so_luong = d.so_luong;
            //        temp_d.gia = d.gia;
            //        temp_d.ck_nt = d.ck_nt;
            //        temp_d.ma_ck = d.ma_ck;
            //        temp_d.ten_ck = d.ten_ck;
            //        temp_d.tl_ck = d.tl_ck;
            //        temp_d.tien_nt = d.tien_nt;
            //        temp_d.name2 = d.name2;
            //        temp_d.dvt = d.dvt;
            //        temp_d.discountPercent = d.discountPercent;
            //        temp_d.imageUrl = d.imageUrl;
            //        temp_d.priceAfter = d.priceAfter;
            //        temp_d.stockAmount = d.stockAmount;


            //        temp_d.ds_ck = new List<DisCountInfo>();
            //        discountInfo = new DisCountInfo();
            //        discountInfo.ma_ck =d.ma_ck;
            //        discountInfo.ten_ck =d.ten_ck;
            //        discountInfo.gt_cl = d.gt_cl;
            //        discountInfo.loai_ct = d.loai_ct;
            //        temp_d.ds_ck.Add(discountInfo);
            //        re_detail.Add(temp_d);
            //    }
            //}
            //foreach (OrderDetailDeDTO d in detail)
            //{
            //    if (d.km_yn == 1)
            //    {
            //        OrderDetailMapItemDTO temp = mapItem.Find(c => c.hang_tang.Trim() == d.ma_vt.Trim());
            //        if (temp == null) continue;
            //        OrderDetailDeResultDTO temp_de = re_detail.Find(c => c.ma_vt == temp.hang_mua);
            //        if (temp_de == null) continue;
            //        discountInfo = new DisCountInfo();
            //        discountInfo.ma_ck = d.ma_ck;
            //        discountInfo.ten_ck = d.ten_ck;
            //        discountInfo.gt_cl = d.gt_cl;
            //        discountInfo.loai_ct = d.loai_ct;
            //        temp_de.ds_ck.Add(discountInfo);
            //    }
            //}
            return new OrderDetailResult
            {
                IsSucceeded = true,
                master = m,
                listProduct = listProduct,
                infoPayment = infoDetailPayment
            };

        }

        public async Task<OrderCartResult> OrderCart(OrderCartRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Lang", request.Lang);
            parameters.Add("@Admin", request.Admin);

            string ItemParam = string.Empty;

            foreach (var ele in request.Items)
            {
                ItemParam += ele.ItemCode + "$" + ele.Quantity.ToString() + ";";
            }

            ItemParam = ItemParam.Remove(ItemParam.Length - 1);
            parameters.Add("@Items", ItemParam);
            string ProceName = "app_Order_Cart";

            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);
            List<OrderItemInfoDTO> Items = gridReader.Read<OrderItemInfoDTO>().ToList();
            OrderCartTotalDTO Total = gridReader.ReadFirst<OrderCartTotalDTO>();
            return new OrderCartResult
            {
                IsSucceeded = true,
                Items = Items,
                Total = Total
            };
        }

        public async Task<ItemDetailResult> ItemDetail(string ItemCode, string Currency, long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ItemCode", ItemCode);
            parameters.Add("@Currency", Currency);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            var result = await dapperService.QueryAsync<dynamic>("[app_Item_Detail]", parameters);

            if (result == null)
            {
                return new ItemDetailResult
                {
                    IsSucceeded = false
                };
            }

            return new ItemDetailResult
            {
                IsSucceeded = true,
                Data = result
            };
        }

        public async Task<ItemGroupResult> ItemGroup(int GroupType, int Level, long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GroupType", GroupType);
            parameters.Add("@Level", Level);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            var result = await dapperService.QueryAsync<ItemGroupDTO>("[app_Item_Group]", parameters);

            if (result == null)
            {
                return new ItemGroupResult
                {
                    IsSucceeded = false
                };
            }

            return new ItemGroupResult
            {
                IsSucceeded = true,
                Data = result
            };
        }
        public async Task<BannerAdResult> GetBannerAd(long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
           
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            var result = await dapperService.QueryAsync<BannerAdDTO>("[app_BannerAd]", parameters);

            if (result == null)
            {
                return new BannerAdResult
                {
                    IsSucceeded = false
                };
            }

            return new BannerAdResult
            {
                IsSucceeded = true,
                Data = result
            };
        }
        public async Task<OrderUpdateResult> OrderUpdate(OrderUpdateRequest request)
        {
            string str = OrderCreateHelp.OrderUpdateV3_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new OrderUpdateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new OrderUpdateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        public async Task<OrderCancelResult> OrderCancel(long UserId, string UnitId, int Admin, string stt_rec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Admin", Admin);
            parameters.Add("@LetterId", stt_rec);

            string ProceName = "app_Order_Cancel";

            string result = await dapperService.ExecuteScalarAsync<string>(ProceName, parameters);
            return new OrderCancelResult
            {
                IsSucceeded = true
            };

        }

        public async Task<DynamicResult> GetProductList(GetProductListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetProductListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Product_List", parameters);

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

        public async Task<DynamicResult> GetProduceOrderList(GetProduceOrderListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetProduceOrderListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Produce_Order_List", parameters);

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
        public async Task<DynamicResult> GetCommandList(GetCommandListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetCommandListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Command_List", parameters);

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
        public async Task<DynamicResult> GetCommandDetail(string stt_rec,string ma_kh)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", stt_rec);
            parameters.Add("@ma_kh", ma_kh);
            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Command_List_Detail", parameters);
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
        public async Task<OrderCreateResult> TaoPhieuTKCD(TaoPhieuTKCDRequest request)
        {
            string str = OrderCreateHelp.TaoPhieuTKCD_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }

        public async Task<OrderCreateResult> UpdateTKCDDraftsTKCD(UpdateTKCDDraftsRequest request)
        {
            string str = OrderCreateHelp.UpdateTKCDDraftsTKCD_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new OrderCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }

        public async Task<DynamicResult> FindProductAllStore(string codeProduct, long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
           
            parameters.Add("@ItemCode", codeProduct);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_CheckItemStockInSite", parameters);
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
                Data = data
            };
        }

        public async Task<ListStoreAndGroupResult> GetListStoreAndGroup(string codeProduct, long UserId, string UnitId, string Lang, int Admin, string listKeyGroup, int checkGroup, int checkStock,int checkStockEmployee)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ItemCode", codeProduct);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            parameters.Add("@CheckStock", checkStock);
            parameters.Add("@CheckGroup", checkGroup);
            parameters.Add("@KeyGroup", listKeyGroup);
            parameters.Add("@CheckStockEmployee ", checkStockEmployee);


            GridReader gridReader = await dapperService.QueryMultipleAsync("app_CheckItemStockInSite", parameters);
            List<GenralDTO> listMessage = gridReader.Read<GenralDTO>().ToList();
            List<dynamic> listGroup = gridReader.Read<dynamic>().ToList();
            List<dynamic> listStore = gridReader.Read<dynamic>().ToList();

            string codeMessage = "";
            string valuesMessage = "";
            if (listMessage != null)
            {
                foreach (GenralDTO data in listMessage)
                {
                    codeMessage = Convert.ToString(data.code);
                    valuesMessage = Convert.ToString(data.message);
                }
            }

            int code = int.Parse(codeMessage);
            string message = valuesMessage;

            if (code == (int)QueryExcuteCode.Success)
            {
                return new ListStoreAndGroupResult()
                {
                    IsSucceeded = true,
                    Message = message,
                    listStore = listStore,
                    listGroup = listGroup
                };
            }
            else {
                return new ListStoreAndGroupResult
                {
                    IsSucceeded = false,
                    Message = message
                };
            }

           
            

            //return new ListStoreAndGroupResult
            //{
            //    IsSucceeded = true,
            //    message =listMessage
                
            //};
        }

        public async Task<DynamicResult> GetListTax(long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
           
            parameters.Add("@UserId", UserId);
            parameters.Add("@UnitId", UnitId);
            parameters.Add("@Lang", Lang);
            parameters.Add("@Admin", Admin);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_list_tax", parameters);
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
                Data = data
            };
        }

        public async Task<DNCCreateResult> TaoPhieuDNC(TaoPhieuDNCRequest request)
        {
            string str = OrderCreateHelp.TaoPhieuDNC_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new DNCCreateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new DNCCreateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        public async Task<DynamicResult> GetDNCList(GetDNCListRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", request.DateFrom);
            parameters.Add("@DateTo", request.DateTo);
            parameters.Add("@UserID", request.UserId);
            parameters.Add("@type", request.Type);
            parameters.Add("@UnitId", request.UnitId);
            parameters.Add("@Admin", request.Admin);
            parameters.Add("@page_index", request.PageIndex);
            parameters.Add("@page_count", request.PageCount);



            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_DNC_List", parameters);

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
        public async Task<DNCResult> GetDNCDetail(string stt_rec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", stt_rec);

            string ProceName = "app_Get_DNC_Detail";

            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);
            List<DNCMasterDTO> master = gridReader.Read<DNCMasterDTO>().ToList();
            List<DNCDetailDTO> detail = gridReader.Read<DNCDetailDTO>().ToList();


            List<DNCDetailResultDTO> re_detail = new List<DNCDetailResultDTO>();

            if (master.Count == 0) return new DNCResult
            {
                IsSucceeded = false
            };
            DNCMasterResultDTO m = new DNCMasterResultDTO();
            m.stt_rec = master[0].stt_rec;
            m.so_ct = master[0].so_ct;
            m.ngay_lct = master[0].ngay_lct;
            m.ngay_ct = master[0].ngay_ct;
            m.ma_kh = master[0].ma_kh;
            m.status = master[0].status;
            m.loai_tt = master[0].loai_tt;
            m.ma_gd = master[0].ma_gd;
            m.ma_nt = master[0].ma_nt;
            m.ty_gia = master[0].ty_gia;
            m.dien_giai = master[0].dien_giai;



            foreach (DNCDetailDTO d in detail)
            {

                DNCDetailResultDTO temp_d = new DNCDetailResultDTO();

                temp_d.tien_nt = d.tien_nt;
                temp_d.dien_giai = d.dien_giai;

                re_detail.Add(temp_d);

            }

            return new DNCResult
            {
                IsSucceeded = true,
                master = m,
                detail = re_detail
            };

        }
        public async Task<DNCUpdateResult> UpdateDNC(UpdateDNCRequest request)
        {
            string str = OrderCreateHelp.UpdateDNC_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new DNCUpdateResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new DNCUpdateResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        public async Task<DNCAuthorizeResult> AuthorizeDNC(DNCAuthorizeRequest request)
        {
            string str = OrderCreateHelp.DNCAuthorize_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
            {
                return new DNCAuthorizeResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            }
            else
            {
                return new DNCAuthorizeResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        public async Task<DynamicResult> GetSiteList(GetSiteListRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetSiteListRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_Site_List", parameters);

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
        
        public async Task<ListVVHDResult> GetListVVHD(string idCustomer,long UserId, string UnitId, string Lang, int Admin)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Customer", idCustomer);
            parameters.Add("@UserId", UserId);
            parameters.Add("@UserId", UserId);
            parameters.Add("@Lang", Lang);
           

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_dmvv_dmhd", parameters);

            List<dynamic> list_vv = gridReader.Read<dynamic>().ToList();
            List<dynamic> list_hd = gridReader.Read<dynamic>().ToList();

            if (gridReader == null)
            {
                return new ListVVHDResult
                {
                    IsSucceeded = false
                };
            }

            return new ListVVHDResult
            {
                IsSucceeded = true,
                list_vv = list_vv,
                list_hd = list_hd
            };
        }

        public async Task<DynamicResult> GetListDNCHistory(GetDNCListHistoryRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<GetDNCListHistoryRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_Get_DNC_Histories_List", parameters);

            dynamic data = gridReader.Read<dynamic>();
            int TotalCount = gridReader.ReadFirst<int>();
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
                TotalPage = TotalCount

            };
        }

        public async Task<DynamicResult> GetListOrderCompleted(long UserId, string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@Customer", idCustomer);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_order_completed_list", parameters);
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

        public async Task<DynamicResult> DetailOrderCompleted(long UserId, string unitID, string sct, string invoiceDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Stt_rec", sct);
            parameters.Add("@Invoice_date", invoiceDate);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_order_completed_detail", parameters);
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

        public async Task<DynamicResult> GetListRefundOrder(string status,long UserId, string unitID, string dateForm, string dateTo, string idCustomer, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Status", status);
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@Customer", idCustomer);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_refund_order_list", parameters);
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

        public async Task<DynamicResult> DetailRefundOrder(long UserId, string unitID, string sct, string invoiceDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Stt_rec", sct);
            parameters.Add("@Invoice_date", invoiceDate);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_refund_order_detail", parameters);
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
        
        public async Task<DynamicResult> GetListStatusOrder(long UserId, string vcCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@VCCode", vcCode);
            parameters.Add("@Language", "V");
            parameters.Add("@UserID", UserId);
            parameters.Add("@Admin", 1);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_voucher_status", parameters);
            List<dynamic> data = gridReader.Read<dynamic>().ToList();

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
        
        public async Task<DynamicResult> GetListApproveOrder(long UserId, string unitID, string dateForm, string dateTo, int page_index, int page_count)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DateFrom", dateForm);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@PageIndex", page_index);
            parameters.Add("@PageCount", page_count);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_order_list", parameters);
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
        public async Task<ItemFromBarCodeResult> GetInforItemForBarCode(long UserId, string unitID, string barcode, string palet)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Barcode ", barcode);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);
            parameters.Add("@Pallet", palet);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_item_by_barcode", parameters);
            List<ItemFromBarCodeDTO> listItem = gridReader.Read<ItemFromBarCodeDTO>().ToList();

            if (gridReader == null)
            {
                return new ItemFromBarCodeResult
                {
                    IsSucceeded = false
                };
            }

            return new ItemFromBarCodeResult
            {
                IsSucceeded = true,
                listItem = listItem
            };
        }
        
        public async Task<ListInfoCardResult> GetListInfoCard(long UserId, string unitID, string stt_rec, string key)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", stt_rec);
            parameters.Add("@key", key);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_order_info", parameters);
            List<dynamic> master = gridReader.Read<dynamic>().ToList();
            List<dynamic> listItem = gridReader.Read<dynamic>().ToList();
            List<RuleActionInforCard> ruleActionInforCard = gridReader.Read<RuleActionInforCard>().ToList();

          
            RuleActionInforCard r = new RuleActionInforCard();
            r = ruleActionInforCard[0];

            if (gridReader == null)
            {
                return new ListInfoCardResult
                {
                    IsSucceeded = false
                };
            }

            return new ListInfoCardResult
            {
                IsSucceeded = true,
                master = master,
                listItem = listItem,
                ruleActionInfoCard = r
            };
        }

        public async Task<FormatProviderResult> GetFormatProvider(long UserId, string unitID, string codeProvider)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Provider", codeProvider);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_formula_provider", parameters);
            List<FormatProvider> formatProvider = gridReader.Read<FormatProvider>().ToList();

            FormatProvider formatProviderResult = new FormatProvider();

            formatProviderResult = formatProvider[0];

            if (gridReader == null)
            {
                return new FormatProviderResult
                {
                    IsSucceeded = false
                };
            }

            return new FormatProviderResult
            {
                IsSucceeded = true,
                formatProvider = formatProviderResult
            };
        }

        public async Task<DynamicResult> ApproveOrder(long UserId, string unitID, string sctRec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec ", sctRec);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_order_approve", parameters);
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
        public async Task<DynamicResult> DeleteItemHolder(long UserId, string unitID, string sctRec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec ", sctRec);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_delete_voucher", parameters);
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
        public async Task<CommonsResult> UpdateQuantityWarehouseDelivery(UpdateQuantityWarehouseDeliveryRequest request)
        {
            string str = OrderCreateHelp.UpdateQuantityWarehouseDelivery_GetQuery(request);

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
        public async Task<CommonsResult> UpdateItemBarcode(UpdateItemBarcodeRequest request)
        {
            string str = OrderCreateHelp.UpdateItemBarcode_GetQuery(request);

            string result = await dapperService.ExecuteScalarAsync<string>(str, null, CommandType.Text);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (true)
            {
                string strsql = OrderCreateHelp.UpdateItemQuantity_GetQuery(request);
                string resultUpdateQuantity = await dapperService.ExecuteScalarAsync<string>(strsql, null, CommandType.Text);
                //int codeUpdateQuantity = int.Parse(resultUpdateQuantity.Split(';')[0]);
                //string messageUpdateQuantity = resultUpdateQuantity.Split(';')[1];
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
            else
            {
                return new CommonsResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
            }
        }
        public async Task<CommonsResult> StockTransferConfirm(UpdateItemBarcodeRequest request)
        {
            string str = OrderCreateHelp.StockTransferConfirm(request);

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
        public async Task<CommonsResult> CreateDeliveryCard(CreateDeliveryRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", request.Data.stt_rec);
            parameters.Add("@license_plates", request.Data.licensePlates);
            parameters.Add("@ma_nvbh", request.Data.codeTransfer);
            parameters.Add("@UserID", request.UserId);
            parameters.Add("@UnitId", request.UnitId);

            string result = await dapperService.ExecuteScalarAsync<string>("app_SSELIB$App$Voucher$AutogenDelivery", parameters, CommandType.StoredProcedure);

            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code == (int)QueryExcuteCode.Success)
                return new CommonsResult()
                {
                    IsSucceeded = true,
                    Message = message
                };
            else
                return new CommonsResult()
                {
                    IsSucceeded = false,
                    Message = message
                };
        }
        public async Task<CommonsResult> UpdatePostPNF(UpdateQuantityPostPNFRequest request)
        {
            string str = OrderCreateHelp.UpdatePostPNF_GetQuery(request);

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
        public async Task<CommonsResult> CreateRefundBarcodeHistory(UpdateQuantityPostPNFRequest request)
        {
            string str = OrderCreateHelp.CreateRefundBarcodeHistory_GetQuery(request);

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
        public async Task<CommonsResult> ItemLocaionModify(ItemLocaionModifyRequest request)
        {
            string str = OrderCreateHelp.ItemLocaionModify_GetQuery(request);

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
        public async Task<ItemHolderDetailResult> GetItemHolderDetail(long UserId, string unitID, string stt_rec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec ", stt_rec);
            parameters.Add("@UserID", UserId);
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_item_holer_detail", parameters);
            List<ItemHolderDetailMasterDTO> listmaster = gridReader.Read<ItemHolderDetailMasterDTO>().ToList();
            List<ItemHolderDTO> listItem = gridReader.Read<ItemHolderDTO>().ToList();
            List<ItemHolderForCustomerDTO> listCustomer = gridReader.Read<ItemHolderForCustomerDTO>().ToList();

            ItemHolderDetailMasterDTO master = listmaster[0];

            List<ItemHolderDTO> listItemHolder = new List<ItemHolderDTO>();

            foreach (ItemHolderDTO itemProduct in listItem)
            {
                ItemHolderDTO itemHolderDTO = new ItemHolderDTO();
                itemHolderDTO = itemProduct;
                foreach (ItemHolderForCustomerDTO itemCustomer in listCustomer)
                {
                    if (itemProduct.ma_vt.ToString().Trim() == itemCustomer.ma_vt.ToString().Trim()) {
                        itemHolderDTO.listCustomer.Add(itemCustomer);  
                    }
                }
                listItemHolder.Add(itemHolderDTO);
            }

           
            if (gridReader == null)
            {
                return new ItemHolderDetailResult
                {
                    IsSucceeded = false
                };
            }

            return new ItemHolderDetailResult
            {
                IsSucceeded = true,
                master = master,
                listItem = listItemHolder
            };
        }
        public async Task<CommonsResult> CreateItemHolder(CreateItemHolderRequest request)
        {
            string str = OrderCreateHelp.CreateItemHolder_GetQuery(request);

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
        public async Task<GetDynamicListResult> GetDynamicList(long UserId, string unitID, string voucher_code, string status, string dateFrom, string dateTo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Voucher_code", voucher_code);
            parameters.Add("@Status", status);
            parameters.Add("@DateFrom", dateFrom);
            parameters.Add("@DateTo", dateTo);
            parameters.Add("@UserID", UserId);  
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_dynamic_get_voucher_list", parameters);
            List<dynamic> listVoucher = gridReader.Read<dynamic>().ToList();
            List<dynamic> listStatus = gridReader.Read<dynamic>().ToList();
            

            if (gridReader == null)
            {
                return new GetDynamicListResult
                {
                    IsSucceeded = false
                };
            }

            return new GetDynamicListResult
            {
                IsSucceeded = true,
                listVoucher = listVoucher,
                listStatus = listStatus,
            };
        }
        public async Task<DynamicResult> GetHistoryDNNK(long UserId, string unitID, string stt_rec)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", stt_rec);
            parameters.Add("@UserID", UserId);  
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_barcode_his", parameters);
            List<dynamic> data = gridReader.Read<dynamic>().ToList();
           

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
        public async Task<DynamicResult> GetBarcodeValue(long UserId, string unitID, string barcode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@barcode", @barcode);
            parameters.Add("@UserID", UserId);  
            parameters.Add("@UnitId", unitID);

            GridReader gridReader = await dapperService.QueryMultipleAsync("get_barcode_value", parameters);
            List<dynamic> data = gridReader.Read<dynamic>().ToList();

            if (gridReader == null)
            {
                return new DynamicResult
                {
                    IsSucceeded = false,
                    Message = "Mã Barcode chưa được định dạng"
                };
            }

            return new DynamicResult
            {
                IsSucceeded = true,
                Data = data,
                Message = "Lấy thông tin barcode thành công"
            };
        }
    }
}