using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class OrderCreateDTO
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
        public string IdTransaction { get; set; }
      
        public List<string> ds_ck { get; set; }
        public List<OrderCreateDetailsDTO> Detail { get; set; }
        public OrderCartTotalDTO Total { get; set; }

    }


    public class RefundOrderCreateV1DTO
    {
        public string sct { get; set; }
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
        public string IdTransaction { get; set; }
        public string discountPercentAgency { get; set; }
        public string discountPercentTypePayment { get; set; }
        public string codeAgency { get; set; }
        public string codeTypePayment { get; set; }
        public string datePayment { get; set; }
        public string codeTax { get; set; }
        public decimal totalTax { get; set; }
        public string codeSell { get; set; }
        public string tk { get; set; }
        public List<RefundOrderCreateDetailsV1DTO> Detail { get; set; }
        public OrderCartTotalDTO Total { get; set; }

    }

    public class ItemCKDTO
    {
        public string ma_ck { get; set; }
        public decimal t_ck_tt { get; set; }
        public int kieu_ck { get; set; }
    }

    public class OrderCreateV3DTO
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
        public string IdTransaction { get; set; }
        public string idVv { get; set; }
        public string idHd { get; set; }
        public decimal discountPercentAgency { get; set; }
        public decimal discountPercentTypePayment { get; set; }
        public string codeAgency { get; set; }
        public string codeTypePayment { get; set; }
        public string datePayment { get; set; }
        public string dateEstDelivery { get; set; }
        public string typeDelivery { get; set; }
        public int orderStatus { get; set; }
        public List<ItemCKDTO> ds_ck { get; set; }
        public List<OrderCreateDetailsV3DTO> Detail { get; set; }
        public OrderCartTotalDTO Total { get; set; }

    }

    public class OrderUpdateDTO
    {
        public string stt_rec { get; set; }
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
        public string IdTransaction { get; set; }
        public string idVv { get; set; }
        public string idHd { get; set; }
        public decimal discountPercentAgency { get; set; }
        public decimal discountPercentTypePayment { get; set; }
        public string codeAgency { get; set; }
        public string codeTypePayment { get; set; }
        public string datePayment { get; set; }
        public int orderStatus { get; set; }
        public List<ItemCKDTO> ds_ck { get; set; }
        public List<OrderCreateDetailsV3DTO> Detail { get; set; }
        public OrderCartTotalDTO Total { get; set; }
    }
    public class UpdateQuantityWarehouseDeliveryDTO
    {
        public string licensePlates { get; set; }
        public List<UpdateQuantityWarehouseDeliveryDetailsDTO> Detail { get; set; } 
        public List<UpdateQuantityWarehouseDeliveryDetailsDTO> listBarcode { get; set; } 
    }
    public class UpdateQuantityPostPNFDTO
    {
        public string stt_rec { get; set; }
        public List<UpdateQuantityWarehouseDeliveryDetailsDTO> Detail { get; set; } 
        public List<UpdateQuantityWarehouseDeliveryDetailsDTO> listBarcode { get; set; } 
    }
    public class CreateDeliveryCardDTO
    {
        public string stt_rec { get; set; }
        public string licensePlates { get; set; }
        public string codeTransfer { get; set; }
    }
    public class UpdateItemBarcodeDTO
    {
        public string stt_rec { get; set; }
        public int action { get; set; }
        public List<UpdateItemBarcodeDetailsDTO> Detail { get; set; }
        public List<UpdateItemBarcodeDetailsDTO> listConfirm { get; set; }
    }
    public class ItemLocaionModifyDTO
    {
        public List<ItemLocaionModifyDetailsDTO> Detail { get; set; }
    }
    public class CreateItemHolderDTO
    {
        public string stt_rec { get; set; }
        public string comment { get; set; }
        public DateTime ngay_het_han { get; set; }
        public List<ItemHolderDTO> listItem { get; set; }
    }
}