namespace SSE.Common.DTO.v1
{
    public class ItemDetailInfoDTO
    {
        public string ma_vt { get; set; }
        public string ten_vt { get; set; }
        public string ten_vt2 { get; set; }
        public string dvt { get; set; }
        public bool nhieu_dvt { get; set; }
        public bool lo_yn { get; set; }
        public bool kk_yn { get; set; }
        public bool in_yn { get; set; }
        public string gia_ton { get; set; }
        public string loai_vt { get; set; }
        public string ten_loai_vt { get; set; }
        public string nh_vt1 { get; set; }
        public string ten_nh_vt1 { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal StockAmount { get; set; }
        public string ImageUrl { get; set; }
    }
}