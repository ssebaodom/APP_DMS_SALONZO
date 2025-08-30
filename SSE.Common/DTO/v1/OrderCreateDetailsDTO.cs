using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class OrderCreateDetailsDTO
    {
        public string Code { get; set; }
        public decimal Count { get; set; }
        public string Dvt { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal ck { get; set; }
        public decimal cknt { get; set; }
        public List<string> ds_ck { get; set; }
    }
    
    public class OrderCreateDetailsV3DTO
    {
        public string nameProduction { get; set; }
        public string Code { get; set; }
        public decimal Count { get; set; }
        public string Dvt { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfter { get; set; }
        public decimal PriceOK { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal ck { get; set; }
        public decimal cknt { get; set; }
        public string ma_ck { get; set; }
        public int km_yn { get; set; }
        public string StockCode { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxValues { get; set; }
        public string TaxCode { get; set; }
        public string idVv { get; set; }
        public string idHd { get; set; }
        public int isCheBien { get; set; }
        public int isSanXuat { get; set; }
        public decimal giaGui { get; set; }
        public decimal giaSuaDoi { get; set; }
        public decimal tienGuiNT { get; set; }
        public decimal tienGui { get; set; }
        public decimal giaGuiNT { get; set; }
        public string note { get; set; }
    }
    
    public class RefundOrderCreateDetailsV1DTO
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

    public class UpdateQuantityWarehouseDeliveryDetailsDTO
    {
        public string sttRec { get; set; }
        public string sttRec0 { get; set; }
        public decimal count { get; set; }
        public int index_item { get; set; }
        public string codeProduction { get; set; }
        public string barcode { get; set; }
        public string ma_kho { get; set; }
        public string ma_lo { get; set; }
        public string hsd { get; set; }
        public string nsx { get; set; }
    }
    public class UpdateItemBarcodeDetailsDTO
    {
        public string sttRec { get; set; }
        public string sttRec0 { get; set; }
        public string ma_vt { get; set; }
        public int index_item { get; set; }
        public string barcode { get; set; }
        public string pallet { get; set; }
        public string ma_kho { get; set; }
        public string ma_lo { get; set; }
        public string so_can { get; set; }
        public string hsd { get; set; }
        public string nsx { get; set; }
    }
    public class ItemLocaionModifyDetailsDTO
    {
        public string ma_vt { get; set; }
        public string ma_vi_tri { get; set; }
        public string so_luong { get; set; }
        public string nxt { get; set; }
        public string pallet { get; set; }
        public string barcode { get; set; }
    }
}