using SSE.Common.Api.v1.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.DTO.v1
{
    public class OrderDTO
    {
    }
    public class OrderDetailMasterDTO
    {
        public string stt_rec { get; set; }
        public string so_ct { get; set; }
        public DateTime ngay_ct { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public decimal t_tc_tien_nt2 { get; set; }
        public decimal t_ck_tt_nt { get; set; }
        public decimal t_tt_nt { get; set; }
        public int status { get; set; }
        public string ma_ck { get; set; }
        public string ten_ck { get; set; }
        public decimal gt_cl { get; set; }
        public string loai_ct { get; set; }
        public string ma_kho { get; set; }
        public string ten_kho { get; set; }

        public string ma_gd { get; set; }
        public string ten_gd { get; set; }
        public string ma_daily { get; set; }
        public string ten_daily { get; set; }
        public string HTTT { get; set; }
        public DateTime han_tt { get; set; }
        public string kieu_kh { get; set; }
        public string dien_giai { get; set; }
    }
    public class OrderDetailDeDTO
    {
        public string ma_vt { get; set; }
        public string ten_vt{ get; set; }
        public int km_yn { get; set; }
        public decimal so_luong { get; set; }
        public decimal gia { get; set; }
        public decimal ck_nt { get; set; }
        public decimal tien_nt { get; set; }
        public string ma_ck { get; set; }
        public string ten_ck { get; set; }
        public decimal tl_ck { get; set; }
        public decimal gt_cl { get; set; }
        public string loai_ct { get; set; }
        public string name2 { get; set; }
        public string dvt { get; set; }
        public decimal discountPercent { get; set; }
        public string imageUrl { get; set; }
        public decimal priceAfter { get; set; }
        public decimal stockAmount { get; set; }
    }
    public class OrderDetailMapItemDTO
    {
        public string ma_ck { get; set; }
        public string group_dk { get; set; }
        public string hang_mua { get; set; }
        public string hang_tang { get; set; }
    }

    public class OrderDetailMasterResultDTO
    {
        public string stt_rec { get; set; }
        public string so_ct { get; set; }
        public DateTime ngay_ct { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public decimal t_tc_tien_nt2 { get; set; }
        public decimal t_ck_tt_nt { get; set; }
        public decimal t_tt_nt { get; set; }
        public int status { get; set; }
        public string ma_kho { get; set; }
        public string HTTT { get; set; }
        public DateTime han_tt { get; set; }
        public string ma_gd { get; set; }
        public string ten_gd { get; set; }
        public string kieu_kh { get; set; }
        public string ma_daily { get; set; }
        public string ten_daily { get; set; }
        public string dien_giai { get; set; }

        public DisCountInfo ck { get; set; }
    }
    
    public class OrderDetailPaymentResultDTO
    {
        public decimal t_tien { get; set; }
        public decimal t_ck_tt_nt { get; set; }
        public decimal t_tt_nt { get; set; }
        public decimal t_thue_nt { get; set; }
    }
    public class OrderDetailDeResultDTO
    {
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public int km_yn { get; set; }
        public decimal so_luong { get; set; }
        public decimal gia { get; set; }
        public decimal ck_nt { get; set; }
        public string ma_ck { get; set; }
        public string ten_ck { get; set; }
        public decimal tl_ck { get; set; }
        public decimal tien_nt { get; set; }
        public string name2 { get; set; }
        public string dvt { get; set; }
        public decimal discountPercent { get; set; }
        public string imageUrl { get; set; }
        public decimal priceAfter { get; set; }
        public decimal stockAmount { get; set; }
        public string ma_thue { get; set; }
        public decimal thue_suat { get; set; }
        public decimal thue_nt { get; set; }
        public string ma_kho { get; set; }
        public string ten_kho { get; set; }
        public string ma_vv { get; set; }
        public string ten_vv { get; set; }
        public string ma_hd { get; set; }
        public string ten_hd { get; set; }
        public decimal gia_net { get; set; }
        public decimal gia_min { get; set; }
        public List<DisCountInfo> ds_ck { get; set; }
    }

    public class GenralDTO
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class RuleActionInforCard
    {
        public int status { get; set; }
        public string statusname { get; set; }
        public string statusname2 { get; set; }
    }
    
    public class FormatProvider
    {
        public int can_yn { get; set; }
        public int can_tu { get; set; }
        public int can_den { get; set; }
        public string don_vi { get; set; }
        public string so_thap_phan { get; set; }
        public int hsd_yn { get; set; }
        public int hsd_tu { get; set; }
        public int hsd_den { get; set; }
        public string dateformat { get; set; }
    }
    public class ItemHolderDetailMasterDTO
    {
        public string stt_rec { get; set; }
        public string ma_dvcs { get; set; }
        public string ma_ct { get; set; }
        public string ngay_ct { get; set; }
        public string so_ct { get; set; }
        public string ma_nvbh { get; set; }
        public string ten_nvbh { get; set; }
        public string dien_giai { get; set; }
        public decimal t_so_luong { get; set; }
        public string status { get; set; }
        public string statusname { get; set; }
        public DateTime ngay_het_han { get; set; }
    }
    public class ItemFromBarCodeDTO
    {
        public string ma_in { get; set; }
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public DateTime hsd { get; set; }
        public decimal so_luong { get; set; }
    }    
    
    public class StatusDTO
    {
        public string status { get; set; }
        public string statusname { get; set; }
        public string statusname2 { get; set; }
    }
    public class ItemHolderDTO
    {
        public string stt_rec { get; set; }
        public string stt_rec0 { get; set; }
        public string ma_dvcs { get; set; }
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public string dvt { get; set; }
        public string ten_dvt { get; set; }
        public decimal so_luong { get; set; }
        public decimal gia { get; set; }
        public decimal gia_nt2 { get; set; }
        public List<ItemHolderForCustomerDTO> listCustomer { get; set; }
        public ItemHolderDTO()
        {
            this.listCustomer = new List<ItemHolderForCustomerDTO>();
        }
    }
    public class ItemHolderForCustomerDTO
    {
        public string stt_rec { get; set; }
        public string stt_rec0 { get; set; }
        public string ma_dvcs { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public string dvt { get; set; }
        public string ten_dvt { get; set; }
        public decimal so_luong { get; set; }
    }
   
}
