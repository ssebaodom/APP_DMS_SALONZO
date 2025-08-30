using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class CheckinDTO
    {
        public string CustomerID { get; set; }
        public string LatLong { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public string IdAlbum { get; set; }
        public string IdCheckIn { get; set; }
        public int OpenStore { get; set; }

        public List<CheckinListImageDTO> Detail { get; set; }
    }

    public class CheckInDetailDTO
    {
        public string idCheckIn { get; set; }
        public string titleCheckIn { get; set; }
        public string dateCheckIn { get; set; }
        public string customerCode { get; set; }
        public string customerName { get; set; }
        public string customerAddress { get; set; }
        public string customerPhone { get; set; }
        public string customerGPS { get; set; }
        public string statusCheckIn { get; set; }
        public string timeCheckInSuccess { get; set; }
        public double customerDebt { get; set; }
        public int estimateTimeCheckOut { get; set; }
    }

    public class AlbumCheckIn
    {
        public string idAlbum { get; set; }
        public string nameAlbum { get; set; }
        public int isRequired { get; set; }
    }

    public class CheckinImageDTO
    {
        public string path_l { get; set; }
        public string ma_album { get; set; }
        public string ten_album { get; set; }
    }


    public class InventoryControlAndSaleOutDTO
    {
        public DateTime OrderDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerAddress { get; set; }
        public string idCheckIn { get; set; }
        public string AgentID { get; set; }
        public string Description { get; set; }
        public string typePayment { get; set; }
        public string dateEstDelivery { get; set; }

        public List<DetailInventoryAndSaleOutDTO> Detail { get; set; }
    }
    
    public class InventoryControlDTO
    {
        public DateTime OrderDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerAddress { get; set; }
        public string idCheckIn { get; set; }
        public string AgentID { get; set; }
        public string Description { get; set; }

        public List<DetailInventoryDTO> Detail { get; set; }
    }

    public class DataRequestOpentStoreDTO
    {
        public List<MasterRequestOpentStoreDTO> dataRequest { get; set; }
    }

    public class MasterRequestOpenStore{
        public string key_value { get; set; }
        public string ten_ch { get; set; }
        public string ten_kh { get; set; }
        public string dien_thoai { get; set; }

        public string dia_chi { get; set; }
        public string ten_tinh { get; set; }
        public string ten_quan { get; set; }
        public string email { get; set; }

        public string nguoi_tao { get; set; }
        public string ghi_chu { get; set; }
        public int trang_thai { get; set; }
        public DateTime ngay_tao { get; set; }
        public string latlong { get; set; }
    }


    public class MasterRequestOpentStoreDTO
    {
        public MasterRequestOpenStore Master { get; set; }
        public List<ImageListRequestOpentStore> imageListRequestOpenStore { get; set; }
        public MasterRequestOpentStoreDTO()
        {
            this.imageListRequestOpenStore = new List<ImageListRequestOpentStore>();
        }
    }

    public class ImageListRequestOpentStore
    {
        public string path_l { get; set; }
        public string ma_album { get; set; }
        public string ten_album { get; set; }
        public string ma_kh { get; set; }
        public string key_value { get; set; }
    }
    
    public class Khuvuc
    {
        public string khu_vuc { get; set; }
        public string ten_khu_vuc { get; set; }
    }

    public class MasterRequestOpenStoreDetailDTO
    {
        public string ma_nvbh { get; set; }
        public string ten_nvbh { get; set; }
        public string ho_ten { get; set; }
        public int ho_ten_yn { get; set; }

        public string dien_thoai { get; set; }
        public int dien_thoai_yn { get; set; }
        public string dien_thoai_dd { get; set; }
        public int dien_thoai_dd_yn { get; set; }

        public string email { get; set; }
        public int email_yn { get; set; }
        public string ma_tuyen { get; set; }
        public string ten_tuyen { get; set; }
        public int ma_tuyen_yn { get; set; }

        //
        public string ma_so_thue { get; set; }
        public int ma_so_thue_yn { get; set; }
        public string tinh_thanh { get; set; }
        public string ten_tinh { get; set; }

        public int tinh_thanh_yn { get; set; }
        public string quan_huyen { get; set; }
        public string ten_quan { get; set; }
        public int quan_huyen_yn { get; set; }
        
        public string khu_vuc { get; set; }
        public string ten_khu_vuc { get; set; }
        public int khu_vuc_yn { get; set; }

        public string xa_phuong { get; set; }
        public string ten_phuong { get; set; }
        public int xa_phuong_yn { get; set; }
        public string nguoi_lh { get; set; }
        public int nguoi_lh_yn { get; set; }

        //
        public string dia_chi { get; set; }
        public int dia_chi_yn { get; set; }
        public string ghi_chu { get; set; }
        public DateTime ngay_sinh { get; set; }
        public string mo_ta { get; set; }
        public int images_yn { get; set; }

        public string phan_loai { get; set; }
        public string ten_loai { get; set; }
        public int phan_loai_yn { get; set; }
        public string hinh_thuc { get; set; }

        public string ten_hinh_thuc { get; set; }
        public int hinh_thuc_yn { get; set; }
        public string latlong { get; set; }
        public string fax { get; set; }
        public string ma_tinh_trang { get; set; }
        public string ten_tinh_trang { get; set; }
    }

    public class RoleUpdateRequestOpenStore
    {
        public int user_role { get; set; }
        public int lead_role { get; set; }
    }

    public class MasterTicketDTO
    {
        public string ma_kh { get; set; }
        public string ten_kh { get; set; }
        public string ten_cv { get; set; }
        public string ten_nv { get; set; }

        public string id_ticket_type { get; set; }
        public string name_ticket_type { get; set; }
        public string noi_dung { get; set; }
        public string id_ticket { get; set; }

        public string thoi_gian { get; set; }
        public string status { get; set; }
        public int user_id0 { get; set; }

        public List<ImageListRequestOpentStore> imageList { get; set; }
        public MasterTicketDTO()
        {
            this.imageList = new List<ImageListRequestOpentStore>();
        }
    }

    public class MasterTimeKeeping
    {
        public string ma_nv { get; set; }
        public string ten_nv { get; set; }
        public string dien_thoai { get; set; }
        public DateTime ngay_sinh { get; set; }
        public DateTime ngay_vao { get; set; }
        public DateTime ngay_chinh_thuc { get; set; }
        public string dia_chi { get; set; }
        public string vi_tri { get; set; }
        public int phep_dn { get; set; }
        public int phep_cl { get; set; }
        public int t_di_lam { get; set; }
        public int t_cong { get; set; }
        public string location { get; set; }
        public decimal distance { get; set; }
    }

    public class MasterHistoryDetailTicketDTO
    {
        public int id { get; set; }
        public string phan_hoi { get; set; }

        public List<ImageListHistoryDetailTicketDTO> imageList { get; set; }
        public MasterHistoryDetailTicketDTO()
        {
            this.imageList = new List<ImageListHistoryDetailTicketDTO>();
        }
    }

    public class ImageListHistoryDetailTicketDTO
    {
        public string code { get; set; }
        public string path_l { get; set; }
        public string ma_album { get; set; }
        public string ten_album { get; set; }
    }

    public class FeedbackHistoryDetailTicketDTO
    {
        public int id_ticket { get; set; }
        public string phan_hoi { get; set; }
    }
}
