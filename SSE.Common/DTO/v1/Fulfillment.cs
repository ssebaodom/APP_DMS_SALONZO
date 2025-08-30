using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.DTO.v1
{
    public class Fulfillment
    {
    }
    public class GETListFulfillmentDTO
    {
        public string stt_rec { get; set; }
        public string ma_dvcs { get; set; }
        public DateTime ngay_ct { get; set; }
        public string so_ct { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public string dien_giai { get; set; }
        public decimal t_tt_nt { get; set; }
        public string ma_nt { get; set; }
        public string dia_chi { get; set; }
        public string ma_ct { get; set; }
        public string status { get; set; } 
    }
    public class DetailListFulfillmentMasterDTO
    {
        public string stt_rec { get; set; }
        public DateTime ngay_ct { get; set; }
        public string so_ct { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public decimal t_so_luong { get; set; }
        public string status { get; set; }
        public decimal t_tt_nt { get; set; }
        public decimal t_tc_tien_nt2 { get; set; }
        public decimal t_ck_nt { get; set; }
    }
    public class DetailListFulfillmentDetailDTO
    {
        public string stt_rec0 { get; set; }
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public string dvt { get; set; }
        public string ma_kho { get; set; }
        public decimal so_luong { get; set; }
        public decimal so_luong_tg { get; set; }
        public decimal sl_da_giao { get; set; }
        public decimal gia_nt2 { get; set; }
        public decimal tien_nt2 { get; set; }

        public decimal tl_ck { get; set; }
        public decimal ck_nt { get; set; }
    }
    public class DetailFulfillmentResultsDTO
    {
        public DetailListFulfillmentMasterDTO master;
        public List<DetailListFulfillmentDetailDTO> dettail;
    }
}