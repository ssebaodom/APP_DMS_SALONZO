using SSE.Common.Constants.v1;
using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class ReportHeaderDescDTO
    {
        public string Field { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public DataType Type { get; set; }
        public string Format { get; set; }
        public int ColWidth { get; set; }
    }
    public class ReportFileAttachDTO
    {
        public string SysKey { get; set; }
        public byte[] FileData { get; set; }
        public string fileName { get; set; }
        public string fileExt { get; set; }

    }
    public class OrderListResultDTO
    {
        public string stt_rec { get; set; }
        public string ma_dvcs { get; set; }
        public DateTime ngay_ct { get; set; }
        public string so_ct { get; set; }
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public decimal t_tt_nt { get; set; }
        public string ma_nt { get; set; }
        public string Hash { get; set; }
    }
    public class OrderListResultDTO2
    {
        public string ma_kho { get; set; }
        public string dien_thoai { get; set; }
        public string dia_chi { get; set; }
        public string ten_kho { get; set; }
    }
    public class GetStoreInfoDTO
    {
        public string ma_kho { get; set; }
        public string ten_kho { get; set; }
    }

    public class MainDataDTO
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
    public class DetailDataDTO
    {
        public List<ReportHeaderDescDTO> columns { get; set; }
        public dynamic value { get; set; }
    }
    public class FanDataDTO
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
    public class InfoDataDTO
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
}