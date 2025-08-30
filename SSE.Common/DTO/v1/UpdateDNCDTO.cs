using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class UpdateDNCDTO
    {

        public DNCMasterResultDTO master { get; set; }

        public List<TaoPhieuDNCDetailDTO> Detail { get; set; }   
    }
    
}