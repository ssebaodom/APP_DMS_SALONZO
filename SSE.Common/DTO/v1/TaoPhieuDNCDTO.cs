using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class TaoPhieuDNCDTO
    {
        public string CustomerCode { get; set; }
        public string dien_giai { get; set; }
        public string loai_tt { get; set; }
        public string ma_gd { get; set; }
        public DateTime OrderDate { get; set; }

        public List<AttachFileDNCDTO> AtachFiles { get; set; }
        public List<TaoPhieuDNCDetailDTO> Detail { get; set; }   
    }
    
}