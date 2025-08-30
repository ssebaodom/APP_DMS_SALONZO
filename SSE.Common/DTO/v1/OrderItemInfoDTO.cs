namespace SSE.Common.DTO.v1
{
    public class OrderItemInfoDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public bool nhieu_dvt { get; set; }
        public string ndvt { get; set; }
        public string Dvt { get; set; }
        public string Descript { get; set; }
        public decimal Price { get; set; }
        public decimal Wo_price { get; set; }
        public decimal Wo_priceAfter
        {
            get
            {
                return (Wo_price - Wo_price * DiscountPercent / 100);
            }
        }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal PriceAfter
        {
            get
            {
                return (Price - Price * DiscountPercent / 100);
            }
        }      
        public string ImageUrl { get; set; }
        public decimal StockAmount { get; set; }
    }
}