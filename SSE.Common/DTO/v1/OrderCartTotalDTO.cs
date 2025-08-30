namespace SSE.Common.DTO.v1
{
    public class OrderCartTotalDTO
    {
        public decimal PreAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalDiscountForItem { get; set; }
        public decimal TotalDiscountForOrder { get; set; }
        public decimal Fee { get; set; }
        public decimal Amount { get; set; }
    }
}