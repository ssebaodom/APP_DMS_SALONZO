using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class PlanDeliveryDTO
    {
        public string stt_rec { get; set; }
        public string so_ct { get; set; }
        public string ngay_ct { get; set; }
        public DateTime orderDate { get; set; }

        public List<DetailDeliveryDTO> Detail { get; set; }
    }
    
}