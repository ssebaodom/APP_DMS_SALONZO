using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class TaoPhieuTKCDDTO
    {
        public string stt_rec_lsx { get; set; }
        public string To { get; set; }
        public DateTime OrderDate { get; set; }
        public string Comment { get; set; }

        public List<TaoPhieuTKCDDetailDTO> Detail { get; set; }   
    }
    
}