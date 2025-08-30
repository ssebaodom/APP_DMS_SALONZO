using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SSE.Common.Api.v1.Requests.Customer
{
    public class CustomerCreateResquest : CommonRequest
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerName2 { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
    }
    
    public class CustomerCareCreateResquest : CommonRequest
    {
        public string CustomerCode { get; set; }
        public string TypeCare { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string ListSurvey { get; set; }
        public List<CheckinListImageDTO> Detail { get; set; }
        public List<IFormFile> ListFile { get; set; }
    }
}