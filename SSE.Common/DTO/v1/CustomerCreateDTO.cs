using System;

namespace SSE.Common.DTO.v1
{
    public class CustomerCreateDTO
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerName2 { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}