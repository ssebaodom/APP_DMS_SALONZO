using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class CheckinListImageDTO
    {
        public string Path { get; set; }
        public string CodeImage { get; set; }
        public string NameImage { get; set; }
        public string codeAlbum { get; set; }
    }
    public class SurveyDTO
    {
        public string stt_rec0 { get; set; }
        public string dien_giai { get; set; }
        public string ma_td2 { get; set; }

    }
    public class FileImageBase64DTO
    {
        public string PathBase64 { get; set; }
        public string NameImage { get; set; }
    }

    public class DetailInventoryAndSaleOutDTO
    {
        public string CodeProduct { get; set; }
        public string NameProduct { get; set; }
        public double Number { get; set; }
        public string dvt { get; set; }
        public decimal price { get; set; }
        public int isDiscount { get; set; }
    }
    public class DetailInventoryDTO
    {
        public string CodeProduct { get; set; }
        public string NameProduct { get; set; }
        public double Number { get; set; }
        public string dvt { get; set; }
     
    }

    public class OrderCreateFromCheckInDTO
    {
        public string CustomerCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string SaleCode { get; set; }
        public string Dvt { get; set; }
        public string Currency { get; set; }
        public string StockCode { get; set; }
        public string Descript { get; set; }
        public string PhoneCustomer { get; set; }
        public string AddressCustomer { get; set; }
        public string Comment { get; set; }

        public List<string> ds_ck { get; set; }
        public List<OrderCreateDetailsDTO> Detail { get; set; }
        public List<FileImageBase64DTO> ListImage { get; set; }
        public OrderCartTotalDTO Total { get; set; }

    }
}