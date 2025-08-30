using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class CustomerInfoDTO
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerName2 { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public IEnumerable<CustomerInfoOtherDTO> OtherData { get; set; }
    }
}