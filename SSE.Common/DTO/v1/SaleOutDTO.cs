using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class RefundSaleOutCreateV1DTO
    {
        public string CustomerCode { get; set; }
        public DateTime OrderDate { get; set; }
 
        public string Descript { get; set; }
        public string PhoneCustomer { get; set; }
        public string AddressCustomer { get; set; }
        public string Comment { get; set; }
        public string codeAgency { get; set; }
        public List<RefundSaleOutCreateDetailsV1DTO> Detail { get; set; }
        //public SaleOutCartTotalDTO Total { get; set; }

    }

    public class RefundSaleOutCreateDetailsV1DTO
    {
        public string stt_rec { get; set; }
        public string stt_rec0 { get; set; }
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public decimal so_luong { get; set; }
        public decimal sl_tra { get; set; }
        public decimal sl_cl { get; set; }
        public string dvt { get; set; }
        public string ma_kho { get; set; }
        public string ten_kho { get; set; }
        public string ma_lo { get; set; }
        public int km_yn { get; set; }
        public decimal gia_nt2 { get; set; }
        public decimal tien_nt2 { get; set; }
        public decimal tl_ck { get; set; }
        public decimal ck_nt { get; set; }
        public decimal tl_ck_tt { get; set; }
        public decimal ck_tt_nt { get; set; }
        public string ma_vv { get; set; }
        public string ma_hd { get; set; }
        public string ma_vi_tri { get; set; }
        public string tk_cpbh { get; set; }
        public string tk_gv { get; set; }
        public string tk_vt { get; set; }

        public string hd_so { get; set; }
        public string stt_rec_dh { get; set; }
        public string stt_rec0dh { get; set; }
    }

    public class SaleOutCartTotalDTO
    {
        public decimal PreAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Fee { get; set; }
        public decimal Amount { get; set; }
    }
}