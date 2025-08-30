using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.DTO.v1
{
    public class DisCountDTO
    {
    }
    public class LineDisCountDTO
    {
        public string stt_rec_ck { get; set; }
        public string ma_ck { get; set; }
        public string ten_ck { get; set; }
        public string ma_vt { get; set; }
        public decimal tl_ck { get; set; }
        public decimal gia2 { get; set; }
        public decimal ck { get; set; }
        public decimal ck_nt { get; set; }
        public int line_nbr { get; set; }
        public decimal tien2 { get; set; }

        public string loai_ct { get; set; }
        public string loai_ct_product { get; set; }
        public List<LineDisCountGiftItemDTO> gift_item {get;set;}
        public string discountProduct { get; set; }
        public string discountProductCode { get; set; }
        public string ten_ns { get; set; }
        public decimal gt_cl { get; set; }

        public decimal gt_cl_product { get; set; }
        public string ten_ns_product { get; set; }
    }
    public class LineDisCountGiftItemDTO
    {
        public string stt_rec_ck { get; set; }
        public string ma_ck { get; set; }
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public string dvt { get; set; }
        public decimal so_luong { get; set; }
        public string km_yn { get; set; }
        public string ma_kho { get; set; }
        public string ma_vi_tri { get; set; }
        public string ma_lo { get; set; }
        public string he_so { get; set; }
        public decimal gia_ton { get; set; }
        public string vi_tri_yn { get; set; }
        public string lo_yn { get; set; }
        public decimal gia_ban_nt { get; set; }
        public decimal gia_nt2 { get; set; }
        public decimal ton13 { get; set; }
        public string ten_ns { get; set; }
        public decimal gt_cl { get; set; }
        public string loai_ct { get; set; }
    }
    public class OrderDisCountDTO
    {
        public string stt_rec_ck { get; set; }
        public string ma_ck { get; set; }
        public string ten_ck { get; set; }
        public string loai_ck { get; set; }
        public decimal tl_ck_tt { get; set; }
        public decimal t_ck_tt { get; set; }
        public decimal t_ck_tt_nt { get; set; }
        public decimal gt_vip { get; set; }
        public decimal gt_vip_nt { get; set; }
        public string so_vocher { get; set; }
        public decimal t_diem_so { get; set; }
        public string loai_ct { get; set; }
        public string showGift { get; set; }
        public string note { get; set; }
        public string createvip { get; set; }
        public string ma_loai { get; set; }
        public decimal gt_vip2 { get; set; }
        public decimal gt_vip_nt2 { get; set; }
        public string ten_ns { get; set; }
        public decimal gt_cl { get; set; }
    }
    public class DisCountInfo
    {
        public string ma_ck { get; set; }
        public string ten_ck { get; set; }
        public string ten_ns { get; set; }
        public decimal gt_cl { get; set; }
        public string loai_ct { get; set; }
    }
    public class MasterDisCountDTO
    {
        public decimal ck { get; set; }
        public decimal t_tien { get; set; }
        public decimal t_tt { get; set; }
        public decimal t_ck { get; set; }
        public List<DisCountInfo> ds_ck { get; set; }

    }
    public class LineGiftItem
    {
        public string stt_rec_ck { get; set; }
        public string ten_ck { get; set; }

        public string ma_ck { get; set; }
        public string group_dk { get; set; }
        public string vt_tang { get; set; }
        public decimal sl_tang { get; set; }
        public string vt_mua { get; set; }

        public string ten_ns { get; set; }
        public decimal gt_cl { get; set; }

        public string loai_ct { get; set; }
    }
    public class DataDisCount
    {
        public MasterDisCountDTO order { get; set; }
        public List<LineDisCountDTO> line_item { get; set; }
        public List<LineDisCountGiftItemDTO> gift_item { get; set; }
        public DataDisCount()
        {
            this.order = new MasterDisCountDTO();
            this.line_item = new List<LineDisCountDTO>();
            this.gift_item = new List<LineDisCountGiftItemDTO>();

        }
    }

    public class TotalMoneyDTO
    {
        public decimal t_tien { get; set; }
        public decimal t_ck { get; set; }
        public decimal t_thanh_toan { get; set; }

    }
}