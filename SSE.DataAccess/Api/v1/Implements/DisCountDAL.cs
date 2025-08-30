using Dapper;
using SSE.Common.Api.v1.Common;
using SSE.Common.Api.v1.Requests.DisCountRequest;
using SSE.Common.Api.v1.Results.DisCount;
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
    public class DisCountDAL : IDisCountDAL
    {
        private readonly IDapperService dapperService;

        public DisCountDAL(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }
        public async Task<DisCountResults> GetDisCount(DisCountRequest req)
        {
            string lst_vt = "";
            string lst_sl = "";
            string lst_gia = "";
            string lst_tien = "";
            string lst_kho = "";
            foreach (LineOrder line in req.line_item)
            {
                lst_vt += line.code + ",";
                lst_sl += System.Math.Truncate(line.count).ToString() + ",";
                lst_gia += System.Math.Truncate(line.price).ToString() + ",";
                lst_tien += System.Math.Truncate(line.count * line.price).ToString() + ",";
                if (req.ma_kho != "" && req.ma_kho != null) lst_kho += req.ma_kho + ",";
            }
            if (lst_vt != "") lst_vt = lst_vt.Substring(0, lst_vt.Length - 1);
            if (lst_sl != "") lst_sl = lst_sl.Substring(0, lst_sl.Length - 1);
            if (lst_gia != "") lst_gia = lst_gia.Substring(0, lst_gia.Length - 1);
            if (lst_tien != "") lst_tien = lst_tien.Substring(0, lst_tien.Length - 1);
            if (lst_kho != "") lst_kho = lst_kho.Substring(0, lst_kho.Length - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@lst_vt", lst_vt);
            parameters.Add("@lst_sl", lst_sl);
            parameters.Add("@lst_gia", lst_gia);
            parameters.Add("@lst_tien", lst_tien);
            parameters.Add("@lst_kho", lst_kho);
            parameters.Add("@ma_kh", req.ma_kh);
            parameters.Add("@unitCode", req.UnitId);
            parameters.Add("@ngay_lct", DateTime.Now);
            parameters.Add("@voucherCode", "DXA");
            parameters.Add("@storeCode", req.ma_kho);
            parameters.Add("@UserId", req.UserId);


            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_sale_info", parameters);
            List<OrderDisCountDTO> order = gridReader.Read<OrderDisCountDTO>().ToList();

            List<LineDisCountDTO> lineck = gridReader.Read<LineDisCountDTO>().ToList();
            List<LineDisCountGiftItemDTO> giftitem = gridReader.Read<LineDisCountGiftItemDTO>().ToList();
            List<LineGiftItem> listgift = gridReader.Read<LineGiftItem>().ToList();

            DisCountResults re = new DisCountResults();
            MasterDisCountDTO masterOrder = new MasterDisCountDTO();
            masterOrder.ck = 0;
            masterOrder.ds_ck = new List<DisCountInfo>();
            foreach (OrderDisCountDTO or in order)
            {
                DisCountInfo info = new DisCountInfo();
                info.ma_ck = or.ma_ck;
                info.ten_ck = or.ten_ck;
                info.ten_ns = or.ten_ns;
                info.gt_cl = or.gt_cl;
                info.loai_ct = or.loai_ct;
                masterOrder.ds_ck.Add(info);
                masterOrder.ck += or.t_ck_tt;
            }
            masterOrder.t_tien = 0;
           
            List<LineDisCountDTO> linere = new List<LineDisCountDTO>();
            foreach (LineOrder line in req.line_item)
            {
                bool check = false;
                LineDisCountDTO temp = new LineDisCountDTO();
                masterOrder.t_tien += line.count * line.price;
                
                foreach (LineDisCountDTO or in lineck)
                {
                    if (line.code.Trim() == or.ma_vt.Trim())
                    {
                        temp = or;
                        check = true;

                        //masterOrder.t_tt = or.tien2; /// SSE - Cloud
                        masterOrder.t_tien -= or.ck_nt;
                        temp.gia2 = line.price;// - (or.ck / line.count); /// Nafaco
                    }
                }
                var f = listgift.FirstOrDefault(c => c.vt_mua == line.code.Trim());
                if (f != null)
                {
                    temp.discountProduct = f.ten_ck;
                    temp.discountProductCode = f.ma_ck;
                    temp.gt_cl_product = 0;
                    temp.loai_ct_product = "";
                    foreach(LineDisCountGiftItemDTO temps in giftitem)
                    {
                        if (temps.stt_rec_ck == f.stt_rec_ck)
                        {
                            temp.gt_cl_product = temps.gt_cl;
                            temp.loai_ct_product = temps.loai_ct;
                            temp.ten_ns_product = temps.ten_ns;
                        }
                    }
                    if (check == false)
                    {
                        temp.gia2 = line.price; ///  Nafaco show
                        temp.ck_nt = 0;
                        temp.ck = 0;
                        temp.ma_vt = line.code;
                        temp.ten_ck = "";
                        temp.tl_ck = 0;
                        check = true;
                    }
                }
                if (check) linere.Add(temp);

            }
            masterOrder.t_tt = masterOrder.t_tien - masterOrder.ck; /// Nafaco show
            //masterOrder.ck = masterOrder.t_tien - masterOrder.t_tt; /// SSE - Cloud
            if (gridReader == null)
            {
                re.IsSucceeded = false;
                return re;
            }
            re.IsSucceeded = true;
            DataDisCount data = new DataDisCount();
            data.line_item = linere;
            data.gift_item = giftitem;
            data.order = masterOrder;
            re.data = data;
            return re;
        }

        public async Task<DisCountResults> GetDisCountWhenUpdate(DisCountWhenUpdateRequest req)
        {
            string lst_vt = "";
            string lst_sl = "";
            string lst_gia = "";
            string lst_tien = "";
            string lst_kho = "";
            foreach (LineOrder line in req.line_item)
            {
                lst_vt += line.code + ",";
                lst_sl += System.Math.Truncate(line.count).ToString() + ",";
                lst_gia += System.Math.Truncate(line.price).ToString() + ",";
                lst_tien += System.Math.Truncate(line.count * line.price).ToString() + ",";
                if (req.ma_kho.Trim() != "") lst_kho += req.ma_kho + ",";
            }
            if (lst_vt != "") lst_vt = lst_vt.Substring(0, lst_vt.Length - 1);
            if (lst_sl != "") lst_sl = lst_sl.Substring(0, lst_sl.Length - 1);
            if (lst_gia != "") lst_gia = lst_gia.Substring(0, lst_gia.Length - 1);
            if (lst_tien != "") lst_tien = lst_tien.Substring(0, lst_tien.Length - 1);
            if (lst_kho != "") lst_kho = lst_kho.Substring(0, lst_kho.Length - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@stt_rec", req.stt_rec);
            parameters.Add("@lst_vt", lst_vt);
            parameters.Add("@lst_sl", lst_sl);
            parameters.Add("@lst_gia", lst_gia);
            parameters.Add("@lst_tien", lst_tien);
            parameters.Add("@lst_kho", lst_kho);
            parameters.Add("@ma_kh", req.ma_kh);
            parameters.Add("@unitCode", req.UnitId);
            parameters.Add("@ngay_lct", DateTime.Now);
            parameters.Add("@voucherCode", "DXA");
            parameters.Add("@storeCode", req.ma_kho);
            parameters.Add("@UserId", req.UserId);


            GridReader gridReader = await dapperService.QueryMultipleAsync("app_get_sale_info_when_update", parameters);
            List<OrderDisCountDTO> order = gridReader.Read<OrderDisCountDTO>().ToList();

            List<LineDisCountDTO> lineck = gridReader.Read<LineDisCountDTO>().ToList();
            List<LineDisCountGiftItemDTO> giftitem = gridReader.Read<LineDisCountGiftItemDTO>().ToList();
            List<LineGiftItem> listgift = gridReader.Read<LineGiftItem>().ToList();

            DisCountResults re = new DisCountResults();
            MasterDisCountDTO masterOrder = new MasterDisCountDTO();
            masterOrder.ck = 0;
            masterOrder.ds_ck = new List<DisCountInfo>();
            foreach (OrderDisCountDTO or in order)
            {
                DisCountInfo info = new DisCountInfo();
                info.ma_ck = or.ma_ck;
                info.ten_ck = or.ten_ck;
                info.ten_ns = or.ten_ns;
                info.gt_cl = or.gt_cl;
                info.loai_ct = or.loai_ct;
                masterOrder.ds_ck.Add(info);
                masterOrder.ck += or.t_ck_tt;
            }
            masterOrder.t_tien = 0;
            List<LineDisCountDTO> linere = new List<LineDisCountDTO>();
            foreach (LineOrder line in req.line_item)
            {
                bool check = false;
                LineDisCountDTO temp = new LineDisCountDTO();
                masterOrder.t_tien += line.count * line.price;
                foreach (LineDisCountDTO or in lineck)
                {
                    if (line.code.Trim() == or.ma_vt.Trim())
                    {
                        temp = or;
                        check = true;
                        masterOrder.t_tien -= or.ck_nt;
                        temp.gia2 = line.price - (or.ck / line.count);
                    }
                }
                var f = listgift.FirstOrDefault(c => c.vt_mua == line.code.Trim());
                if (f != null)
                {
                    temp.discountProduct = f.ten_ck;
                    temp.discountProductCode = f.ma_ck;
                    temp.gt_cl_product = 0;
                    temp.loai_ct_product = "";
                    foreach (LineDisCountGiftItemDTO temps in giftitem)
                    {
                        if (temps.stt_rec_ck == f.stt_rec_ck)
                        {
                            temp.gt_cl_product = temps.gt_cl;
                            temp.loai_ct_product = temps.loai_ct;
                            temp.ten_ns_product = temps.ten_ns;
                        }
                    }
                    if (check == false)
                    {
                        temp.gia2 = line.price;
                        temp.ck_nt = 0;
                        temp.ck = 0;
                        temp.ma_vt = line.code;
                        temp.ten_ck = "";
                        temp.tl_ck = 0;
                        check = true;
                    }
                }
                if (check) linere.Add(temp);

            }
            masterOrder.t_tt = masterOrder.t_tien - masterOrder.ck;

            if (gridReader == null)
            {
                re.IsSucceeded = false;
                return re;
            }
            re.IsSucceeded = true;
            DataDisCount data = new DataDisCount();
            data.line_item = linere;
            data.gift_item = giftitem;
            data.order = masterOrder;
            re.data = data;
            return re;
        }

        /// <summary>
        /// 19/12/2022 
        /// V2 Tổng quát chương trình khuyến mại
        /// </summary>
        /// <creater name="tiennq"></creater>
        /// <returns>list discount</returns>
        public async Task<DisCountApplyResult> ApplyDiscount(DisCountItemRequest request)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<DisCountItemRequest>(request);

            GridReader gridReader = await dapperService.QueryMultipleAsync("app_applyDiscount_v2", parameters);
            
            dynamic list_ck_tong_don = gridReader.Read<dynamic>();
            dynamic list_ck = gridReader.Read<dynamic>();
            dynamic list_ck_mat_hang = gridReader.Read<dynamic>();
           
            TotalMoneyDTO totalMoney = gridReader.ReadFirst<TotalMoneyDTO>();

            if (gridReader == null)
            {
                return new DisCountApplyResult
                {
                    IsSucceeded = false
                };
            }

            return new DisCountApplyResult
            {
                IsSucceeded = true,
                list_ck_tong_don = list_ck_tong_don,
                list_ck = list_ck,
                list_ck_mat_hang = list_ck_mat_hang,
                totalMoneyDiscount = totalMoney
            };
        }
    }
}
