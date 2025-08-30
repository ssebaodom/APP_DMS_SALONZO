using Newtonsoft.Json;

namespace SSE.Common.DTO.v1
{
    public class UserInfoDTO
    {
        public long UserId { set; get; }
        public string UserName { set; get; }
        public string HostId { set; get; }
        public string Code { set; get; }
        public string CodeName { set; get; }
        public int Role { set; get; }
        public string PhoneNumber { set; get; }
        public string Email { set; get; }
        public string FullName { get; set; }
        public int Active { get; set; }
        public string maNvbh { get; set; }
        public string maBp { get; set; }
        public string tenBp { get; set; }
        public int nghiCl { get; set; }

        [JsonIgnore]
        public string Keys { get; set; }

        [JsonIgnore]
        public string PassWord { get; set; }
    }

    public class AppSettingsDTO
    {
        public int inStockCheck { get; set; }
        public int inStockCheckSaleOut { get; set; }
        public int freeDiscount { get; set; }
        public int discountSpecial { get; set; }
        public int woPrice { get; set; }
        public int isVvHd { get; set; }
        public int isVv { get; set; }
        public int isHd { get; set; }
        public int lockStockInItem { get; set; }
        public int lockStockInCart { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public int distanceLocationCheckIn { get; set; }
        public int saleOutUpdatePrice { get; set; }
        public int afterTax { get; set; }
        public int useTax { get; set; }
        public int chooseAgency { get; set; }
        public int chooseTypePayment { get; set; }
        public int wholesale { get; set; }
        public int orderWithCustomerRegular { get; set; }
        public int chooseStockBeforeOrder { get; set; }
        public int checkGroup { get; set; }
        public int chooseAgentSaleOut { get; set; }
        public int chooseSaleOffSaleOut { get; set; }
        public int chooseStatusToCreateOrder { get; set; }
        public int enableAutoAddDiscount { get; set; }
        public int enableProductFollowStore { get; set; }
        public int enableViewPriceAndTotalPriceProductGift { get; set; }
        public int chooseStatusToSaleOut { get; set; }
        public int chooseStateWhenCreateNewOpenStore { get; set; }
        public int isVvForCustomer { get; set; }
        public int editPrice { get; set; }
        public int approveOrder { get; set; }
        public int dateEstDelivery { get; set; }
        public int typeProduction { get; set; }
        public int giaGui { get; set; }
        public int editNameProduction { get; set; }
        public int checkPriceAddToCard { get; set; }
        public int checkStockEmployee { get; set; }
        public int chooseStockBeforeOrderWithGiftProduction { get; set; }
        public int takeFirstStockInList { get; set; }
        public int chooseTypeDelivery { get; set; }
        public int noteForEachProduct { get; set; }
        public int allowsWoPriceAndTransactionType { get; set; }
        public int isCheckStockSaleOut { get; set; }
    }
}