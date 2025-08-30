using System;
using System.Collections.Generic;
using System.Text;

namespace SSE.Common.DTO.v1
{
    public class DNCDTO
    {
    }
    public class DNCMasterDTO
    {
        public string stt_rec { get; set; }
        public string so_ct { get; set; }
        public DateTime ngay_ct { get; set; }
        public DateTime ngay_lct { get; set; }
        public string ma_kh { get; set; }
        public string dien_giai { get; set; }
        public int status { get; set; }
        public string loai_tt { get; set; }
        public string ma_gd { get; set; }
        public string ma_nt { get; set; }
        public decimal ty_gia { get; set; }
    }
    public class DNCDetailDTO
    {
        public string stt_rec { get; set; }
        public string stt_rec0 { get; set; }
        public decimal tien_nt { get; set; }
        public string dien_giai { get; set; }
    


    }
    

    public class DNCMasterResultDTO
    {
        public string stt_rec { get; set; }
        public string so_ct { get; set; }
        public DateTime ngay_ct { get; set; }
        public DateTime ngay_lct { get; set; }
        public string ma_kh { get; set; }
        public string dien_giai { get; set; }
        public int status { get; set; }
        public string statusname { get; set; }
        public string loai_tt { get; set; }
        public string ma_gd { get; set; }
        public string ma_nt { get; set; }
        public decimal ty_gia { get; set; }
    }
    public class DNCDetailResultDTO
    {
        public decimal tien_nt { get; set; }
        public string dien_giai { get; set; }

    }

}
