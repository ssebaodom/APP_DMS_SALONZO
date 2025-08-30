using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class CustomerListDTO
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerName2 { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string kieu_kh { get; set; }
    }

    public class CareCustomerListDTO
    {
        public string stt_rec { get; set; }
        public string ngay_ct { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public string dia_chi { get; set; }
        public string dien_thoai { get; set; }
        public string dien_giai { get; set; }
        public string loai_cs { get; set; }
        public string kh_ph { get; set; }
        public List<ImageListRequestOpentStore> imageList { get; set; }
        public CareCustomerListDTO()
        {
            this.imageList = new List<ImageListRequestOpentStore>();
        }
    }
}