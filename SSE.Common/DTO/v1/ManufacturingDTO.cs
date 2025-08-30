using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class ManufacturingDTO
    {
        public string stt_rec { get; set; }
        public string comment { get; set; }
        public string ma_dvcs { get; set; }
        public string ma_gd { get; set; }
        public DateTime ngay_ct { get; set; }
        public string ghi_chu { get; set; }
        public decimal t_so_luong { get; set; }
        public string ma_nc { get; set; }
        public string ma_px { get; set; }
        public string ma_lsx { get; set; }
        public string ma_cd { get; set; }
        public string ma_ca { get; set; }
        public decimal sl_nc { get; set; }
        public string gio_bd { get; set; }
        public string gio_kt { get; set; }
        public List<ManufacturingDetailDTO> Detail { get; set; }
        public List<ManufacturingRawTableDTO> RawTable { get; set; }
        public List<ManufacturingWasteTableDTO> WasteTable { get; set; }
        public List<ManufacturingMachineDTO> MachineTable { get; set; }
    }
    public class ManufacturingDetailDTO
    {
        public string ma_vt { get; set; }
        public decimal so_luong { get; set; }
        public string dvt { get; set; }
        public string ma_nc { get; set; }
        public string nh_nc { get; set; }
        public string ghi_chu { get; set; }
        public string ma_lo { get; set; }
    }
    public class ManufacturingRawTableDTO
    {
        public string ma_vt { get; set; }
        public decimal so_luong { get; set; }
        public string dvt { get; set; }
        public int rework { get; set; }
        public string ma_lo { get; set; }
        public decimal sl_tn { get; set; }
        public decimal sl_cl { get; set; }
        public decimal sl_sd { get; set; }
    }
    public class ManufacturingWasteTableDTO
    {
        public string ma_vt { get; set; }
        public decimal so_luong { get; set; }
        public string dvt { get; set; }
        public string code_store { get; set; }
    }
    public class ManufacturingMachineDTO
    {
        public string ma_may { get; set; }
        public string gio_bd { get; set; }
        public string gio_kt { get; set; }
        public decimal so_gio { get; set; }
        public string ghi_chu { get; set; }
    }
}