using Microsoft.AspNetCore.Http;
using SSE.Common.Api.v1.Common;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.Api.v1.Requests.Todos
{
    public class ReportLocationRequest : CommonRequest
    {
        public DateTime Datetime { get; set; }
        public string Customer { get; set; }
        public string LatLong { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string NamePath { get; set; }
        public string NameFile { get; set; }
        public string CodeImage { get; set; }
        public string Path { get; set; }

        //public List<CheckinListImageDTO> Detail { get; set; }
        //public List<IFormFile> ListFile { get; set; }
    }

    public class ReportLocationRequestV2 : CommonRequest
    {
        public string Datetime { get; set; }
        public string Customer { get; set; }
        public string LatLong { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

        public List<CheckinListImageDTO> Detail { get; set; }
        public List<IFormFile> ListFile { get; set; }
    }
}