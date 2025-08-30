using Newtonsoft.Json.Linq;
using SSE.Common.Api.v1.Requests.FulfillmentRequest;
using SSE.Common.Api.v1.Requests.Order;
using SSE.Common.Api.v1.Requests.Todos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SSE.DataAccess.Support.Functs
{
    public class OrderCreateHelp
    {
        public static string OrderCreate_GetQuery(OrderCreateRequest request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim();
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            string ds_ck = "";
            if (obj.ds_ck.Count > 0)
            {
                foreach (string s in obj.ds_ck) ds_ck += s + ",";
            }
            if (ds_ck != "") ds_ck = ds_ck.Remove(ds_ck.Length - 1);

            string ck;
            string cknt;

            //if (obj.CK == null)
            //{
            //    ck = "0";
            //}
            //else
            //{
            //    ck = obj.CK;
            //}
            //if (obj.CKNT == null)
            //{
            //    cknt = "0";
            //}
            //else
            //{
            //    cknt = obj.CKNT;
            //}

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m{vc}$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d{vc}$000000";
            str += Environment.NewLine + "declare @index_item int =0,@check int =0,@ma_ck nvarchar(100)='',@ma_ck_tang_hang nvarchar(100)=''";

            str += Environment.NewLine + $"insert into #master ( stt_rec, ma_ct, so_ct, ma_dvcs, loai_ct, ma_gd, ngay_ct, ngay_lct, ma_kh, dien_giai, ma_nvbh, datetime0, datetime2, user_id0, user_id2,s3,{t_ck})" 
                + Environment.NewLine +
                        $" select '', '{code}', '', '{request.UnitId}', '', '{obj.IdTransaction}', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', N'{obj.Descript}', '{obj.SaleCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{ds_ck}',{request.Data.Total.Discount}";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"set @ma_ck ='';set @ma_ck_tang_hang='';";
                string tt = "";
                if (row.ds_ck != null)
                {
                    foreach (string s in row.ds_ck)
                    {
                        tt += s + ",";
                        str += Environment.NewLine + "if Exists(select * from scckhangtang d join scdmck e on d.stt_rec_ck = e.stt_rec_ck where d.group_dk  in (" +
                        $"SELECT group_dk FROM scckhangmua WHERE stt_rec_ck = d.stt_rec_ck AND ma_vt='{row.Code}'" +
                        $" )and  e.ma_ck = '{s.Trim()}' AND e.loai_ck='07'" +
                        " )";
                        str += Environment.NewLine + $"set @ma_ck_tang_hang ='{s.Trim()}'  else set  @ma_ck +='{s}'+','";
                    }
                }

                str += Environment.NewLine + "if @ma_ck<>'' set @ma_ck = left(@ma_ck,len(@ma_ck)-1)";
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv, ma_sp, so_luong, gia_nt2, thue_nt, {tl_ck},{ma_ck_i},{km_yn},dvt,ck,ck_nt)" + Environment.NewLine +
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.Code}', '{obj.StockCode}', '', '', {row.Count}, {row.Price}, 0, {row.DiscountPercent},@ma_ck,0,'{row.Dvt}',{row.ck},{row.cknt}";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                str += Environment.NewLine  +"if  @ma_ck_tang_hang <> ''";
                str += Environment.NewLine + "begin";
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv, ma_sp, so_luong, gia_nt2, thue_nt, {tl_ck},{ma_ck_i},{km_yn},dvt,ck,ck_nt)";
                str += Environment.NewLine + $"SELECT top 1 '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, a.ma_vt, '{obj.StockCode}', '', '',  {row.Count} * Floor( a.so_luong/c.s4), isnull((select gia_nt2 from dmgia2 where ma_vt=a.ma_vt and ma_nt='{obj.Currency}'),0), 0, 0,@ma_ck_tang_hang ,1,'{row.Dvt}',{row.ck},{row.cknt}";
                str += Environment.NewLine + "FROM dbo.scckhangtang a JOIN dbo.scdmck b ON a.stt_rec_ck =b.stt_rec_ck " +
                    " left join dbo.scckhangmua c on a.stt_rec_ck =c.stt_rec_ck and a.group_dk = c.group_dk "+
                $"WHERE a.group_dk IN(SELECT TOP 1 group_dk FROM dbo.scckhangmua WHERE ma_vt = '{row.Code.Trim()}' AND stt_rec_ck = a.stt_rec_ck) AND b.ma_ck = @ma_ck_tang_hang";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                str += Environment.NewLine + "end";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_OrderCreate_] @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Currency = '{obj.Currency}', @StockCode = '{obj.StockCode}', @PhoneCustomer = '{obj.PhoneCustomer}', @AddressCustomer = '{obj.AddressCustomer}', @Comment = N'{obj.Comment}'";
            str += Environment.NewLine + $", @PreAmount = {obj.Total.PreAmount},  @Tax = {obj.Total.Tax}, @Discount = {obj.Total.Discount}, @Fee = {obj.Total.Fee}, @Amount = {obj.Total.Amount}";
            return str;
        }

        public static string OrderCreate_GetQueryV3(OrderCreateV3Request request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim(); // truyền vào tổng chiết khấu của vật tư
            string t_ck_nt = json["t_ck_nt"].ToString().Trim(); // truyền vào tổng chiết khấu của vật tư
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            string ds_ck = "";
            /// 0 - 3: truyền -> t_ck_tt_nt
            /// 2: truyền -> fqty2
           /// Ver 2 sửa thành list master - cho phép chọn nhiều chiết khấu
            string ma_ck_tong_don = ""; 
            decimal tong_ck_tt_type_1 = 0;

            decimal tong_ck_tt_type_2 = 0;
            if (obj.ds_ck.Count > 0)
            {
                foreach (var itemCK in obj.ds_ck) {
                    ma_ck_tong_don = itemCK.ma_ck;
                    if (itemCK.kieu_ck == 2)
                    {
                        tong_ck_tt_type_2 = itemCK.t_ck_tt;
                    }
                    else {
                        tong_ck_tt_type_1 = itemCK.t_ck_tt;
                    }
                    
                }
            }
            if (ds_ck != "") ds_ck = ds_ck.Remove(ds_ck.Length - 1);

            //string ck;
            //string cknt;

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m{vc}$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d{vc}$000000";
            str += Environment.NewLine + "declare @index_item int =0,@check int =0,@ma_ck nvarchar(100)='',@ma_ck_tang_hang nvarchar(100)=''";

            /// Nafaco + Thiên Vương bỏ ma_kh2 (Mã đại lý) - ở master => values: ,'{obj.codeAgency}'
            str += Environment.NewLine + $"insert into #master " +
                $"( stt_rec, ma_ct, so_ct, ma_dvcs, loai_ct, ma_gd, ngay_ct, ngay_lct, ma_kh, dien_giai, ma_nvbh, datetime0, datetime2, " +
                $"user_id0, user_id2,s3,{t_ck},{t_ck_nt},t_ck_tt_nt,t_ck_tt,fqty2, t_tien_nt2,t_tien2,t_tt_nt,t_tt,fnote3,s7,fdate1,t_thue,t_thue_nt)"
                + Environment.NewLine +
                        $" select '', '{code}', '', '{request.UnitId}', '', '{obj.IdTransaction}', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', " +
                        $"N'{obj.Descript}', '{obj.SaleCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{ma_ck_tong_don}'," +
                        $"{request.Data.Total.TotalDiscountForItem},{request.Data.Total.TotalDiscountForItem},{request.Data.Total.TotalDiscountForOrder},{request.Data.Total.TotalDiscountForOrder},{obj.discountPercentAgency}," +
                        $"{obj.Total.PreAmount},{obj.Total.PreAmount},{obj.Total.Amount},{obj.Total.Amount}," +
                        $"'{obj.codeTypePayment}','{obj.datePayment ??  DateTime.Parse(obj.datePayment).ToString()}','{obj.dateEstDelivery}',{request.Data.Total.Tax},{request.Data.Total.Tax}";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv,ma_hd, ma_sp, so_luong, gia_nt2,ma_thue, {tl_ck},{ma_ck_i},{km_yn},dvt,ck,ck_nt,thue_suat,thue_nt,thue)"   
                    + Environment.NewLine + /// Thằng Hưng bảo sửa gia_nt2 là giá gốc
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.Code}', " +
                            $"'{row.StockCode}', '{row.idVv}', '{row.idHd}', '', {row.Count}, {row.PriceOK},'{row.TaxCode}', {row.DiscountPercent}," +/// sửa sau gia * với tỷ lệ quy đổi
                            $"'{row.ma_ck}',{row.km_yn},N'{row.Dvt}',{row.ck},{row.cknt},{row.TaxPercent},{row.TaxValues},{row.TaxValues}";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_Order_Create_v3] @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Currency = '{obj.Currency}', @StockCode = '{obj.StockCode}', @PhoneCustomer = '{obj.PhoneCustomer}', @AddressCustomer = N'{obj.AddressCustomer}', @Comment = N'{obj.Comment}'";
            str += Environment.NewLine + $", @PreAmount = {obj.Total.PreAmount},  @Tax = {obj.Total.Tax}, @Discount = {obj.Total.Discount}, @Fee = {obj.Total.Fee}, @Amount = {obj.Total.Amount}, @orderStatus = {obj.orderStatus}";
            return str;
        }

        public static string OrderUpdateV3_GetQuery(OrderUpdateRequest request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim(); // truyền vào tổng chiết khấu của vật tư
            string t_ck_nt = json["t_ck_nt"].ToString().Trim(); // truyền vào tổng chiết khấu của vật tư
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            string ds_ck = "";
            /// 0 - 3: truyền -> t_ck_tt_nt
            /// 2: truyền -> fqty2
            /// Ver 2 sửa thành list master - cho phép chọn nhiều chiết khấu
            string ma_ck_tong_don = "";
            decimal tong_ck_tt_type_1 = 0;

            decimal tong_ck_tt_type_2 = 0;
            if (obj.ds_ck.Count > 0)
            {
                foreach (var itemCK in obj.ds_ck)
                {
                    ma_ck_tong_don = itemCK.ma_ck;
                    if (itemCK.kieu_ck == 2)
                    {
                        tong_ck_tt_type_2 = itemCK.t_ck_tt;
                    }
                    else
                    {
                        tong_ck_tt_type_1 = itemCK.t_ck_tt;
                    }

                }
            }
            if (ds_ck != "") ds_ck = ds_ck.Remove(ds_ck.Length - 1);

            //string ck;
            //string cknt;

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m{vc}$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d{vc}$000000";
            str += Environment.NewLine + "declare @index_item int =0,@check int =0,@ma_ck nvarchar(100)='',@ma_ck_tang_hang nvarchar(100)=''";

            /// Nafaco + Thiên Vương bỏ ma_kh2 (Mã đại lý) - ở master => values: ,'{obj.codeAgency}'
            str += Environment.NewLine + $"insert into #master " +
                $"( stt_rec, ma_ct, so_ct, ma_dvcs, loai_ct, ma_gd, ngay_ct, ngay_lct, ma_kh, dien_giai, ma_nvbh, datetime0, datetime2, " +
                $"user_id0, user_id2,s3,{t_ck},{t_ck_nt},t_ck_tt_nt,t_ck_tt,fqty2, t_tien_nt2,t_tien2,t_tt_nt,t_tt,fnote3,s7,fdate1,t_thue,t_thue_nt)"
                + Environment.NewLine +
                        $" select '', '{code}', '', '{request.UnitId}', '', '{obj.IdTransaction}', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', " +
                        $"N'{obj.Descript}', '{obj.SaleCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{ma_ck_tong_don}'," +
                        $"{request.Data.Total.TotalDiscountForItem},{request.Data.Total.TotalDiscountForItem},{request.Data.Total.TotalDiscountForOrder},{request.Data.Total.TotalDiscountForOrder},{obj.discountPercentAgency}," +
                        $"{obj.Total.PreAmount},{obj.Total.PreAmount},{obj.Total.Amount},{obj.Total.Amount}," +
                        $"'{obj.codeTypePayment}',''";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv,ma_hd, ma_sp, so_luong, gia_nt2,ma_thue, {tl_ck},{ma_ck_i},{km_yn},dvt,ck,ck_nt,thue_suat,thue_nt,thue)"
                    + Environment.NewLine + /// Thằng Hưng bảo sửa gia_nt2 là giá gốc
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.Code}', " +
                            $"'{row.StockCode}', '{row.idVv}', '{row.idHd}', '', {row.Count}, {row.PriceOK},'{row.TaxCode}', {row.DiscountPercent}," +/// sửa sau gia * với tỷ lệ quy đổi
                            $"'{row.ma_ck}',{row.km_yn},N'{row.Dvt}',{row.ck},{row.cknt},{row.TaxPercent},{row.TaxValues},{row.TaxValues}";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_order_update_v2] @table_sufix = '{table_sufix}', @stt_rec = '{obj.stt_rec}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Currency = '{obj.Currency}', @StockCode = '{obj.StockCode}', @PhoneCustomer = '{obj.PhoneCustomer}', @AddressCustomer = N'{obj.AddressCustomer}', @Comment = N'{obj.Comment}'";
            str += Environment.NewLine + $", @PreAmount = {obj.Total.PreAmount},  @Tax = {obj.Total.Tax}, @Discount = {obj.Total.Discount}, @Fee = {obj.Total.Fee}, @Amount = {obj.Total.Amount}, @orderStatus = {obj.orderStatus}";
            return str;
        }

        public static string RefundOrderCreate_GetQueryV1(RefundOrderCreateV1Request request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim();
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;

            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
          

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m76$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d76$000000";
            str += Environment.NewLine + "declare @index_item int =0,@check int =0,@ma_ck nvarchar(100)='',@ma_ck_tang_hang nvarchar(100)=''";


            str += Environment.NewLine + $"insert into #master " +
                $"( stt_rec, ma_ct, so_ct, ma_dvcs, loai_ct, ma_gd, ngay_ct, ngay_lct, ma_kh, dien_giai, ma_nvbh, datetime0, datetime2, " +
                $"user_id0, user_id2, t_tien_nt2,t_tien2,t_tt_nt,t_tt,t_thue_nt,ma_thue,s2,tk_thue_co,tk)"
                + Environment.NewLine +
                        $" select '', 'HDF', '', '{request.UnitId}', '', '{obj.IdTransaction}', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', " +
                        $"N'{obj.Descript}', '{obj.SaleCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}'," +
                        $"{obj.Total.PreAmount},{obj.Total.PreAmount},{obj.Total.Amount},{obj.Total.Amount},{request.Data.totalTax},'{request.Data.codeTax}','{request.Data.codeSell}','{request.Data.tk}','{request.Data.tk}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv,ma_hd, ma_sp, so_luong, gia_nt2, {tl_ck},{km_yn},dvt,ck,ck_nt,stt_rec_hd,stt_rec0hd,ma_lo,ma_vi_tri,tk_cpbh,hd_so,stt_rec_dh,stt_rec0dh,tk_gv,tk_vt)"
                    + Environment.NewLine + /// Thằng Hưng bảo sửa gia_nt2 là giá gốc
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.ma_vt}', " +
                            $"'{row.ma_kho}', '', '', '', {row.so_luong}, {row.gia_nt2}, {row.tl_ck}," +
                            $"{row.km_yn},N'{row.dvt}',{row.ck_nt},{row.ck_nt},'{row.stt_rec}','{row.stt_rec0}','{row.ma_lo}','{row.ma_vi_tri}','{row.tk_cpbh}','{row.hd_so}','{row.stt_rec_dh}','{row.stt_rec0dh}','{row.tk_gv}','{row.tk_vt}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_Refund_Order_Create_v3] @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Currency = '{obj.Currency}', @StockCode = '{obj.StockCode}', @PhoneCustomer = '{obj.PhoneCustomer}', @AddressCustomer = N'{obj.AddressCustomer}', @Comment = N'{obj.Comment}'";
            str += Environment.NewLine + $", @PreAmount = {obj.Total.PreAmount},  @Tax = {obj.Total.Tax}, @Discount = {obj.Total.Discount}, @Fee = {obj.Total.Fee}, @Amount = {obj.Total.Amount}, @VCCode = '{obj.sct}'";
            return str;
        }

        public static string RefundSaleOutCreate_GetQueryV1(RefundSaleOutCreateV1Request request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim();
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";


            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m103$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d103$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec, so_ct, ma_kh, ma_dvcs, ma_ct,ong_ba,dia_chi,dien_giai)" + Environment.NewLine +
                           $" select '','', '{request.Data.codeAgency}','','','{request.Data.CustomerCode}',N'{request.Data.AddressCustomer}',N'{request.Data.Comment}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {   
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_vt, so_luong,dvt,ma_ct,ngay_ct,so_ct,{km_yn},gia_nt2,stt_rec_ct,stt_rec0ct,hd_so)";
                str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row.ma_vt}', '{row.so_luong}',N'{row.dvt}','','','',{row.km_yn},{row.gia_nt2},'{row.stt_rec}','{row.stt_rec0}','{row.hd_so}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec [app_create_sale_out_refund] @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Transaction = '5'";
            return str;
        }

        public static string OrderUpdate_GetQuery(OrderUpdateRequest request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim();
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            string ds_ck = "";
            /// 0 - 3: truyền -> t_ck_tt_nt
            /// 2: truyền -> fqty2
            /// Ver 2 sửa thành list master - cho phép chọn nhiều chiết khấu
            string ma_ck_tong_don = "";
            decimal tong_ck_tt_type_1 = 0;

            decimal tong_ck_tt_type_2 = 0;
            if (obj.ds_ck.Count > 0)
            {
                foreach (var itemCK in obj.ds_ck)
                {
                    ma_ck_tong_don = itemCK.ma_ck;
                    if (itemCK.kieu_ck == 2)
                    {
                        tong_ck_tt_type_2 = itemCK.t_ck_tt;
                    }
                    else
                    {
                        tong_ck_tt_type_1 = itemCK.t_ck_tt;
                    }

                }
            }
            if (ds_ck != "") ds_ck = ds_ck.Remove(ds_ck.Length - 1);

            //string ck;
            //string cknt;

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m{vc}$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d{vc}$000000";
            str += Environment.NewLine + "declare @index_item int =0,@check int =0,@ma_ck nvarchar(100)='',@ma_ck_tang_hang nvarchar(100)=''";

            /// Nafaco bỏ ma_kh2 (Mã đại lý) - ở master => values: ,'{obj.codeAgency}'
            str += Environment.NewLine + $"insert into #master " +
                $"( stt_rec, ma_ct, so_ct, ma_dvcs, loai_ct, ma_gd, ngay_ct, ngay_lct, ma_kh, dien_giai, ma_nvbh, datetime0, datetime2, " +
                $"user_id0, user_id2,s3,{t_ck},t_ck_tt_nt,t_ck_tt,fqty2, t_tien_nt2,t_tien2,t_tt_nt,t_tt,fnote3,s7,ma_kh2)"
                + Environment.NewLine +
                        $" select '', '{code}', '', '{request.UnitId}', '', '{obj.IdTransaction}', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', " +
                        $"N'{obj.Descript}', '{obj.SaleCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{ma_ck_tong_don}'," +
                        $"{request.Data.Total.Discount},{obj.discountPercentTypePayment},{obj.Total.Discount},{obj.discountPercentAgency}," +
                        $"{obj.Total.PreAmount},{obj.Total.PreAmount},{obj.Total.Amount},{obj.Total.Amount}," +
                        $"'{obj.codeTypePayment}','{obj.datePayment}','{obj.codeAgency}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv,ma_hd, ma_sp, so_luong, gia_nt2,ma_thue, {tl_ck},{ma_ck_i},{km_yn},dvt,ck,ck_nt,thue_suat,thue_nt,thue)"
                    + Environment.NewLine + /// Thằng Hưng bảo sửa gia_nt2 là giá gốc
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.Code}', " +
                            $"'{row.StockCode}', '{row.idVv}', '{row.idHd}', '', {row.Count}, {row.PriceOK},'{row.TaxCode}', {row.DiscountPercent}," +
                            $"'{row.ma_ck}',{row.km_yn},N'{row.Dvt}',{row.ck},{row.cknt},{row.TaxPercent},{row.TaxValues},{row.TaxValues}";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_order_update_v2] @stt_rec='{obj.stt_rec}', @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Currency = '{obj.Currency}', @StockCode = '{obj.StockCode}'";
            str += Environment.NewLine + $", @PreAmount = {obj.Total.PreAmount},  @Tax = {obj.Total.Tax}, @Discount = {obj.Total.Discount}, @Fee = {obj.Total.Fee}, @Amount = {obj.Total.Amount}, @orderStatus = {obj.orderStatus}";
            return str;
        }
        public static string TaoPhieuTKCD_GetQuery(TaoPhieuTKCDRequest request)
        {

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";

       

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m74$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d74$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec,ma_dvcs,ma_ct,so_ct,ngay_lct,ngay_ct,ma_kh,datetime0,datetime2,user_id0,user_id2,fcode1)" + Environment.NewLine +
                        $" select '','{request.UnitId}', 'PNT','', '{ngay_ct}', '{ngay_ct}', '', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{obj.To}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {                     
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_bp, ma_sp,so_lsx,sl_td3,sl_song_tt,sl_song_cp,sl_song_td,sl_song_dat,sl_song_pp,sl_in_tt,sl_in_hong,sl_in_song,sl_in_dat,sl_in_csx,sl_cb_tt,sl_cb_in,sl_cb_song,sl_cb_bx,sl_cb_dat,sl_cb_csx,sl_lv_tt,sl_lv_in,sl_lv_song,sl_lv_bx,sl_lv_dat,sl_lv_csx,sl_ht_tt,sl_ht_in,sl_ht_song,sl_ht_cb,sl_ht_ht,sl_ht_dat,sl_ht_cl )";
                str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.ma_vt}', '{row.ma_kho}', '{row.ma_bp}', '{row.ma_vt}','{row.so_lsx}',{row.sl_td3},{row.sl_song_tt},{row.sl_song_cp},{row.sl_song_td},{row.sl_song_dat},{row.sl_song_pp},{row.sl_in_tt},{row.sl_in_hong},{row.sl_in_song},{row.sl_in_dat},{row.sl_in_csx},{row.sl_cb_tt},{row.sl_cb_in},{row.sl_cb_song},{row.sl_cb_bx},{row.sl_cb_dat},{row.sl_cb_csx},{row.sl_lv_tt},{row.sl_lv_in},{row.sl_lv_song},{row.sl_lv_bx},{row.sl_lv_dat},{row.sl_lv_csx},{row.sl_ht_tt},{row.sl_ht_in},{row.sl_ht_song},{row.sl_ht_cb},{row.sl_ht_ht},{row.sl_ht_dat},{row.sl_ht_cl}";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";


                i += 1;
            }

            str += Environment.NewLine + $"exec[app_TaoPhieuTKCD] @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}',@Store = '{request.StoreId}',@stt_rec_lsx = '{request.Data.stt_rec_lsx}',@Comment = N'{request.Data.Comment}'";
     
            return str;
        }

        public static string InventoryControl_GetQuery(InventoryControlRequest request)
        {

            var obj = request.Data;

            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            int i = 0;


            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";
           
            str += Environment.NewLine + $"select top 0 * into #master from m66$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d66$000000"; ///66 cho Thiên Vương - 99 các bên khác
     
            str += Environment.NewLine + "declare @index_item int =0";
          

            str += Environment.NewLine + $"insert into #master (stt_rec, so_ct, ma_kh, ma_dvcs, ma_ct)" + Environment.NewLine +
                        $" select '','', '{request.Data.CustomerID}','',''";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_vt, so_luong,ma_ct,ngay_ct,so_ct)";
                str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row.CodeProduct}', '{row.Number}','','',''";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec[app_create_inventory_check] @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}',@idJob = '{request.Data.idCheckIn}'";

            return str;
        }
        
        public static string UpdateSaleOut_GetQuery(InventoryControlAndSaleOutRequest request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string tableSaleOut = json["table_sale_out"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            int i = 0;


            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m{tableSaleOut}$000000"; // m66 
            str += Environment.NewLine + $"select top 0 * into #detail from d{tableSaleOut}$000000"; // d66
            str += Environment.NewLine + "declare @index_item int =0";
            if (tableSaleOut.Contains("103")) {
                str += Environment.NewLine + $"insert into #master (stt_rec, so_ct, ma_kh, ma_dvcs, ma_ct,ong_ba,dia_chi,dien_giai,fdate1)" + Environment.NewLine +
                           $" select '','', '{request.Data.AgentID}','','','{request.Data.CustomerID}',N'{request.Data.CustomerAddress}',N'{request.Data.Description}','{request.Data.dateEstDelivery}'";
            }
            else {
                str += Environment.NewLine + $"insert into #master (stt_rec, so_ct, ma_kh, ma_dvcs, ma_ct)" + Environment.NewLine +
                            $" select '','', '{request.Data.CustomerID}','',''";
            }       

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                if (tableSaleOut.Contains("103"))
                {
                    str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_vt, so_luong,dvt,ma_ct,ngay_ct,so_ct,{km_yn},gia_nt2)";
                    str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row.CodeProduct}', '{row.Number}',N'{row.dvt}','','','',{row.isDiscount},{row.price}";
                }
                else
                {
                    str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_vt, so_luong,dvt,ma_ct,ngay_ct,so_ct,gia_nt2)";
                    str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row.CodeProduct}', '{row.Number}',N'{row.dvt}','','','',{row.price}";
                }
                
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec[app_create_sale_out] @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Transaction = '{obj.typePayment}'";

            return str;
        }

        public static string CheckOut_GetQuery(CheckOutRequest request)
        {

            var obj = request.Data;

            int i = 0;
            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item int =0";

            foreach (var row in request.Data.Detail)//select top 0 code, path_l, name, ma_album
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album,ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '{row.codeAlbum}','{request.CustomerID}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec[app_Checkout] @Customer = '{request.CustomerID}', @LatLong = '{request.LatLong}'," +
                $" @UserId = '{request.UserId}',@UnitId  = '{request.UnitId}',@Location = N'{request.Location}'," +
                $"@Note = N'{request.Note}',@idJob = '{request.IdCheckIn}',@open_yn = '{request.OpenStore}'," +
                $"@Lang  = '{request.Lang}',@timeStartCheckIn  = '{request.TimeStartCheckIn}', @status = '{request.Status}',@timeCheckOut ='{request.TimeCheckOut}',@AddressDifferent =N'{request.AddressDifferent}',@LatDifferent ='{request.LatDifferent}',@LongDifferent ='{request.LongDifferent}'";

            return str;
        }

        public static string ReportLocation_GetQuery(ReportLocationRequestV2 request)
        {

            int i = 0;
            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item int =0";

            foreach (var row in request.Detail)//select top 0 code, path_l, name, ma_album
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album,ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '{row.codeAlbum}','{request.Customer}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec[app_CheckinCreate_v2] @Customer = '{request.Customer}', @Datetime = '{request.Datetime}'," +
                $" @UserId = '{request.UserId}',@UnitId  = '{request.UnitId}',@Description = N'{request.Description}'," +
                $"@LatLong = N'{request.LatLong}',@Location = N'{request.Location}',@StoreId = '{request.StoreId}'," +
                $"@Lang  = '{request.Lang}',@Note = N'{request.Note}'";

            return str;
        }

        public static string CreateRequestOpenStore_GetQuery(RequestOpenStoreRequest request)
        {
            int i = 0;
            string str = "";
           
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item int =0";

            foreach (var row in request.Image)//select top 0 code, path_l, name, ma_album
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album,ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '',''";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec[app_Create_Lead] @customerName = N'{request.StoreName}', " +
                $"@city = N'{request.City}', @district = N'{request.District}',@contactPerson  = N'{request.ContactPerson}'," +
                $"@contactPhone = '{request.ContactPhone}',@email = '{request.Email}',@birth = '{request.Birthday}'," +
                $"@address = N'{request.Address}',@GPS  = '{request.GPS}',@tour  = '{request.IdTour}',@note  = N'{request.Note}'," +
                $"@area = '{request.IdArea}',@UserId  = '{request.UserId}',@ClassifyCustomer = '{request.IdTypeStore}'," +
                $"@FormsCustomer = '{request.IdStoreForm}',@MST  = '{request.MST}',@Desription = N'{request.Desc}'," +
                $"@contactPhone2 = '{request.StorePhone}',@commune = N'{request.IdCommune}',@UnitID  = N'{request.UnitId}',@location  = N'{request.Location}',@state   = N'{request.IdState}'";

            return str;
        }
        
        public static string UpdateRequestOpenStore_GetQuery(UpdateRequestOpenStoreRequest request)
        {
            int i = 0;
            string str = "";
           
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item int =0";

            foreach (var row in request.Image)//select top 0 code, path_l, name, ma_album
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album,ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '',''";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec[app_Update_Lead] @customerName = N'{request.StoreName}',@stt_rec_dm ='{request.idRequestOpenStore}', " +
                $"@city = '{request.City}', @district = '{request.District}',@contactPerson  = N'{request.ContactPerson}'," +
                $"@contactPhone = '{request.ContactPhone}',@email = '{request.Email}',@birth = '{request.Birthday}'," +
                $"@address = N'{request.Address}',@GPS  = '{request.GPS}',@tour  = '{request.IdTour}',@note  = N'{request.Note}'," +
                $"@area = '{request.IdArea}',@UserId  = '{request.UserId}',@ClassifyCustomer = '{request.IdTypeStore}'," +
                $"@FormsCustomer = '{request.IdStoreForm}',@MST  = '{request.MST}',@description = N'{request.Desc}',@contactPhone2 = '{request.StorePhone}',@commune = '{request.IdCommune}',@state  = '{request.IdState}'";

            return str;
        }
        
        public static string CreateNewTicket(CreatNewTicketRequest request)
        {
            int i = 0;
            string str = "";
           
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item int =0";

            foreach (var row in request.Image)//select top 0 code, path_l, name, ma_album
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album,ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '','{request.CustomerCode}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec[app_create_ticket] @customerID = '{request.CustomerCode}', @userID = '{request.UserId}', @taskId = '{request.TaskId}',@ticketType = '{request.TicketType}',@comment = N'{request.Comment}'";

            return str;
        }

        public static string OrderCreateFromCheckIn_GetQuery(OrderCreateFromCheckInRequest request)
        {
            string directory = Directory.GetCurrentDirectory();
            //string html = System.IO.File.ReadAllText(s+"/Rpt/tt.html");
            StreamReader r = new StreamReader(directory + "/Value/df.json");
            string str_value = r.ReadToEnd();
            JObject json = JObject.Parse(str_value);
            string vc = json["vc"].ToString();
            string code = json["code"].ToString().Trim();
            string t_ck = json["t_ck"].ToString().Trim();
            string tl_ck = json["tl_ck"].ToString().Trim();
            string ma_ck_i = json["ma_ck_i"].ToString().Trim();
            string km_yn = json["km_yn"].ToString().Trim();

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";
            string ds_ck = "";
            if (obj.ds_ck.Count > 0)
            {
                foreach (string s in obj.ds_ck) ds_ck += s + ",";
            }
            if (ds_ck != "") ds_ck = ds_ck.Remove(ds_ck.Length - 1);

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #master from m{vc}$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d{vc}$000000";
            str += Environment.NewLine + "declare @index_item int =0,@check int =0,@ma_ck nvarchar(100)='',@ma_ck_tang_hang nvarchar(100)=''";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item_image int =0";

            str += Environment.NewLine + $"insert into #master ( stt_rec, ma_ct, so_ct, ma_dvcs, loai_ct, ma_gd, ngay_ct, ngay_lct, ma_kh, dien_giai, ma_nvbh, datetime0, datetime2, user_id0, user_id2,s3,{t_ck})" + Environment.NewLine +
                        $" select '', '{code}', '', '{request.UnitId}', '', '', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', N'{obj.Descript}', '{obj.SaleCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{ds_ck}',{request.Data.Total.Discount}";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"set @ma_ck ='';set @ma_ck_tang_hang='';";
                string tt = "";
                foreach (string s in row.ds_ck)
                {
                    tt += s + ",";
                    str += Environment.NewLine + "if Exists(select * from scckhangtang d join scdmck e on d.stt_rec_ck = e.stt_rec_ck where d.group_dk  in (" +
                    $"SELECT group_dk FROM scckhangmua WHERE stt_rec_ck = d.stt_rec_ck AND ma_vt='{row.Code}'" +
                    $" )and  e.ma_ck = '{s.Trim()}' AND e.loai_ck='07'" +
                    " )";
                    str += Environment.NewLine + $"set @ma_ck_tang_hang ='{s.Trim()}'  else set  @ma_ck +='{s}'+','";
                }
                str += Environment.NewLine + "if @ma_ck<>'' set @ma_ck = left(@ma_ck,len(@ma_ck)-1)";
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv, ma_sp, so_luong, gia_nt2, thue_nt, {tl_ck},{ma_ck_i},{km_yn},dvt)" + Environment.NewLine +
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.Code}', '{obj.StockCode}', '', '', {row.Count}, {row.Price}, 0, {row.DiscountPercent},@ma_ck,0,'{row.Dvt}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                str += Environment.NewLine + "if  @ma_ck_tang_hang <> ''";
                str += Environment.NewLine + "begin";
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_vv, ma_sp, so_luong, gia_nt2, thue_nt, {tl_ck},{ma_ck_i},{km_yn},dvt)";
                str += Environment.NewLine + $"SELECT top 1 '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, a.ma_vt, '{obj.StockCode}', '', '',  {row.Count} * Floor( a.so_luong/c.s4), isnull((select gia_nt2 from dmgia2 where ma_vt=a.ma_vt and ma_nt='{obj.Currency}'),0), 0, 0,@ma_ck_tang_hang ,1,'{row.Dvt}'";
                str += Environment.NewLine + "FROM dbo.scckhangtang a JOIN dbo.scdmck b ON a.stt_rec_ck =b.stt_rec_ck " +
                    " left join dbo.scckhangmua c on a.stt_rec_ck =c.stt_rec_ck and a.group_dk = c.group_dk " +
                $"WHERE a.group_dk IN(SELECT TOP 1 group_dk FROM dbo.scckhangmua WHERE ma_vt = '{row.Code.Trim()}' AND stt_rec_ck = a.stt_rec_ck) AND b.ma_ck = @ma_ck_tang_hang";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";
                str += Environment.NewLine + "end";

                i += 1;
            }
            
            str += Environment.NewLine;
            foreach (var row in request.Image)
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album, ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '','{request.Data.CustomerCode}'";
                str += Environment.NewLine + "set  @index_item_image = @index_item_image + 1";
                i += 1;
            }

            str += Environment.NewLine + $"exec [app_Order_Create_v2] @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}', @Currency = '{obj.Currency}', @StockCode = '{obj.StockCode}', @PhoneCustomer = '{obj.PhoneCustomer}', @AddressCustomer = '{obj.AddressCustomer}', @Comment = N'{obj.Comment}'";
            str += Environment.NewLine + $", @PreAmount = {obj.Total.PreAmount},  @Tax = {obj.Total.Tax}, @Discount = {obj.Total.Discount}, @Fee = {obj.Total.Fee}, @Amount = {obj.Total.Amount}";
            return str;
        }

        public static string UpdateTKCDDraftsTKCD_GetQuery(UpdateTKCDDraftsRequest request)
        {

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";



            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m74$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d74$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec,ma_dvcs,ma_ct,so_ct,ngay_lct,ngay_ct,ma_kh,datetime0,datetime2,user_id0,user_id2,fcode1)" + Environment.NewLine +
                        $" select '','{request.UnitId}', 'PNT','', '{ngay_ct}', '{ngay_ct}', '', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{obj.To}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr, ma_vt, ma_kho, ma_bp, ma_sp,so_lsx,sl_td3,sl_song_tt,sl_song_cp,sl_song_td,sl_song_dat,sl_song_pp,sl_in_tt,sl_in_hong,sl_in_song,sl_in_dat,sl_in_csx,sl_cb_tt,sl_cb_in,sl_cb_song,sl_cb_bx,sl_cb_dat,sl_cb_csx,sl_lv_tt,sl_lv_in,sl_lv_song,sl_lv_bx,sl_lv_dat,sl_lv_csx,sl_ht_tt,sl_ht_in,sl_ht_song,sl_ht_cb,sl_ht_ht,sl_ht_dat,sl_ht_cl )";
                str += Environment.NewLine + $"SELECT '', '{row.stt_rec0}', '', '{obj.OrderDate}', '', @index_item+1, '{row.ma_vt}', '{row.ma_kho}', '{row.ma_bp}', '{row.ma_vt}','{row.so_lsx}',{row.sl_td3},{row.sl_song_tt},{row.sl_song_cp},{row.sl_song_td},{row.sl_song_dat},{row.sl_song_pp},{row.sl_in_tt},{row.sl_in_hong},{row.sl_in_song},{row.sl_in_dat},{row.sl_in_csx},{row.sl_cb_tt},{row.sl_cb_in},{row.sl_cb_song},{row.sl_cb_bx},{row.sl_cb_dat},{row.sl_cb_csx},{row.sl_lv_tt},{row.sl_lv_in},{row.sl_lv_song},{row.sl_lv_bx},{row.sl_lv_dat},{row.sl_lv_csx},{row.sl_ht_tt},{row.sl_ht_in},{row.sl_ht_song},{row.sl_ht_cb},{row.sl_ht_ht},{row.sl_ht_dat},{row.sl_ht_cl}";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec[app_CapNhatPhieuLSX] @stt_rec_lsx = '{request.Data.stt_rec_lsx}'";

            return str;
        }

        public static string TaoPhieuDNC_GetQuery(TaoPhieuDNCRequest request)
        {

            var obj = request.Data;

            int i = 0;
            int indexAttachFile = 1;
            string table_sufix = obj.OrderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.OrderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";



            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#attachFile') IS NOT NULL DROP TABLE #attachFile;";

            str += Environment.NewLine + $"select top 0 * into #master from m52$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d52$000000";
            str += Environment.NewLine + $"select top 0 * into #attachFile from sysfileinfo";
            str += Environment.NewLine + "declare @index_item int =0";
            str += Environment.NewLine + "declare @indexAttachFile int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec,ma_dvcs,ma_ct,so_ct,ngay_lct,ngay_ct,ma_kh,datetime0,datetime2,user_id0,user_id2,loai_tt,ma_gd,dien_giai)" + Environment.NewLine +
                        $" select '','{request.UnitId}', 'BPC','', '{ngay_ct}', '{ngay_ct}', '{obj.CustomerCode}', getdate(), getdate(), '{request.UserId}', '{request.UserId}','{obj.loai_tt}','{obj.ma_gd}',N'{obj.dien_giai}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr,tien_nt,dien_giai )";
                str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '{obj.OrderDate}', '', @index_item+1, '{row.tien_nt}', N'{row.dien_giai}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";


                i += 1;
            }

            foreach (var row in request.Data.AtachFiles)
            {
                str += Environment.NewLine + $"insert into #attachFile (controller,syskey,line_nbr,file_name, file_ext, file_size,file_type,file_data )";
                str += Environment.NewLine + $"SELECT '','','{indexAttachFile}','{row.file_name}','{row.file_ext}', '{row.file_size}', '0', convert(varbinary(max), cast(REPLACE(REPLACE('0x{row.file_data}',CHAR(13),''),CHAR(10),'') as varchar(max)), 1)";
                str += Environment.NewLine + "set  @indexAttachFile = @indexAttachFile + 1";


                indexAttachFile += 1;
            }

            str += Environment.NewLine + $"exec[app_TaoPhieuDNC] @table_sufix = '{table_sufix}', @VCDate = '{obj.OrderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}',@Store = '{request.StoreId}', @Encode = '{request.Encode}', @Ticket = '{request.Ticket}', @DeptId = '{request.DeptId}'";

            return str;
        }
        public static string UpdateDNC_GetQuery(UpdateDNCRequest request)
        {

            var master = request.Data.master;
            var detail = request.Data.Detail;
            int i = 0;
     

            string str = "";



            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from m52$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from d52$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec,ma_dvcs,ma_ct,so_ct,ngay_lct,ngay_ct,ma_kh,loai_tt,ma_gd,dien_giai,ma_nt,ty_gia)" + Environment.NewLine +
                        $" select '','{request.UnitId}', 'BPC','{master.so_ct}', '', '', '{master.ma_kh}','{master.loai_tt}','{master.ma_gd}',N'{master.dien_giai}','{master.ma_nt}',{master.ty_gia}";

            str += Environment.NewLine;
            foreach (var row in detail)
            {
                str += Environment.NewLine + $"insert into #detail ( stt_rec, stt_rec0, ma_ct, ngay_ct, so_ct, line_nbr,tien_nt,dien_giai )";
                str += Environment.NewLine + $"SELECT '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ), '', '', '', @index_item+1, '{row.tien_nt}', N'{row.dien_giai}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";


                i += 1;
            }

            str += Environment.NewLine + $"exec[app_Update_DNC]  @stt_rec = '{request.stt_rec}', @UserId = '{request.UserId}'";

            return str;
        }
        public static string DNCAuthorize_GetQuery(DNCAuthorizeRequest request)
        {         
            string str = "";

            str += Environment.NewLine + $"exec[rs_PostDCAuthorize_vS]  @Action = '{request.Action}', @cap_duyet = '{request.Cap_duyet}',@stt_rec ='{request.stt_rec}',@UserID ='{request.UserId}'";

            return str;
        }
        public static string Authorize_GetQuery(AuthorizeRequest request)
        {
            string str = "";

            str += Environment.NewLine + $"exec[rs_PostAuthorize_vS]  @loai_duyet = '{request.loai_duyet}',@Action = '{request.Action}',@stt_rec ='{request.stt_rec}',@UserID ={request.UserId},@Note=N'{request.Note}'";

            return str;
        }

        public static string UpdateDeliveryPlan_GetQuery(DeliveryPlanRequest request)
        {

            var obj = request.Data;

            int i = 0;
          
            string str = "";
          
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #detail from dkhgh";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail (stt_rec,stt_rec0,ma_ct,ma_vt,ten_vt,sl_td1,ngay_ct,so_ct)";
                str += Environment.NewLine + $"SELECT '','{row.stt_rec0}', '', '{row.ma_vt}', N'{row.ten_vt}', '{row.sl_xtt}', '{row.ngay_giao}',''";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec[app_CapNhatPhieuKHGH] @stt_rec_kghd = '{request.Data.stt_rec}'";

            return str;
        }

        public static string CreateDeliveryPlan_GetQuery(DeliveryPlanRequest request)
        {

            var obj = request.Data;

            int i = 0;
            string table_sufix = obj.orderDate.ToString("yyyyMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            string ngay_ct = obj.orderDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            string str = "";



            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";

            str += Environment.NewLine + $"select top 0 * into #master from mkhgh";
            str += Environment.NewLine + $"select top 0 * into #detail from dkhgh";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec, so_ct, ngay_ct, ma_dvcs, ma_ct, s1)" + Environment.NewLine +
                        $" select '','{obj.so_ct}', '{obj.ngay_ct}','{request.UnitId}', '',''";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail (stt_rec, stt_rec0, ma_vt, dvt, so_luong, s1, ma_vc, ma_ct, ngay_ct, so_ct, gc_td1)";
                str += Environment.NewLine + $"SELECT '', '{row.stt_rec0}', '{row.ma_vt}', '{row.dvt}', '{row.sl_xtt}','{row.ma_kh}' ,'{row.ma_vc}', '', '{obj.ngay_ct}', '', N'{row.ten_vt}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";


                i += 1;
            }

            str += Environment.NewLine + $"exec[app_TaoPhieuGH] @table_sufix = '{table_sufix}', @VCDate = '{obj.orderDate}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}',@Store = '{request.StoreId}',@Stt_rec = '{request.Data.stt_rec}'";

            return str;
        }
        public static string UpdateQuantityWarehouseDelivery_GetQuery(UpdateQuantityWarehouseDeliveryRequest request)
        {
           
            var obj = request.Data;

            int i = 0;
            int indexBarcode = 0;
            string str = "";
 
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#items') IS NOT NULL DROP TABLE #items;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#itemsBarcode') IS NOT NULL DROP TABLE #itemsBarcode;";

            str += Environment.NewLine + $"select top 0  stt_rec,stt_rec0, ma_vt, so_luong into #items from d81$000000";
            str += Environment.NewLine + "declare @index_item int =0";
            str += Environment.NewLine + $"select top 0 stt_rec0,[index],stt_rec, ma_vt,barcode into #itemsBarcode from b81$000000";
            str += Environment.NewLine + "declare @index_item_barcode int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #items (stt_rec,stt_rec0, ma_vt, so_luong)"
                    + Environment.NewLine + 
                            $" select '{row.sttRec}','{row.sttRec0}','{row.codeProduction}', '{row.count}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            foreach (var row in request.Data.listBarcode)
            {
                str += Environment.NewLine + $"insert into #itemsBarcode (stt_rec0,[index],stt_rec, ma_vt,barcode)"
                    + Environment.NewLine +
                            $" select '{row.sttRec0}','{indexBarcode}','{row.sttRec}','{row.codeProduction}', '{row.barcode}'";
                str += Environment.NewLine + "set  @index_item_barcode =@index_item_barcode + 1";

                indexBarcode += 1;
            }

            str += Environment.NewLine + $"exec [app_sale_warehouse_delivery_quantity_modify] @license_plates = '{obj.licensePlates}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}'";
            return str;
        }
        public static string UpdatePostPNF_GetQuery(UpdateQuantityPostPNFRequest request)
        {
            var obj = request.Data;

            int i = 0;
            string str = "";
 
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#items') IS NOT NULL DROP TABLE #items;";
       
            str += Environment.NewLine + $"select top 0  cast (0 as int ) as [index],stt_rec, ma_vt,barcode,ma_vt as ma_lo,cast(N'' as smalldatetime) as hsd,cast(N'' as smalldatetime) as nsx,cast(N'' as numeric(16,4)) as so_can into #items from b75$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                string _nsx = "";
                if (row.nsx != null)
                {
                    _nsx = $"'{row.nsx}'";
                }
                else
                {
                    _nsx = "null";
                }

                string _hsd = "";
                if (row.hsd != null)
                {
                    _hsd = $"'{row.hsd}'";
                }
                else
                {
                    _hsd = "null";
                }

                str += Environment.NewLine + $"insert into #items ([index],stt_rec, ma_vt,barcode,ma_lo,hsd, nsx, so_can)"
                   + Environment.NewLine +
                           $" select '{row.index_item}','{row.sttRec}','{row.codeProduction}','{row.barcode}','{row.ma_lo}',{_nsx},{_hsd},'{row.count}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_post_PNF] @stt_rec = '{obj.stt_rec}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}'";
            return str;
        }
        public static string CreateRefundBarcodeHistory_GetQuery(UpdateQuantityPostPNFRequest request)
        {
            var obj = request.Data;

            int i = 0;
            string str = "";
 
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#itemsBarcode') IS NOT NULL DROP TABLE #itemsBarcode;";
       
            str += Environment.NewLine + $"select top 0 [index], stt_rec, stt_rec0, ma_vt,barcode into #itemsBarcode from b76$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                string _nsx = "";
                if (row.nsx != null)
                {
                    _nsx = $"'{row.nsx}'";
                }
                else
                {
                    _nsx = "null";
                }

                string _hsd = "";
                if (row.hsd != null)
                {
                    _hsd = $"'{row.hsd}'";
                }
                else
                {
                    _hsd = "null";
                }

                str += Environment.NewLine + $"insert into #itemsBarcode ([index], stt_rec, stt_rec0, ma_vt,barcode)"
                   + Environment.NewLine +
                           $" select '{row.index_item}','{row.sttRec}','{row.sttRec0}','{row.codeProduction}','{row.barcode}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_create_refund_barcode_his] @Stt_rec = '{obj.stt_rec}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}'";
            return str;
        }
        public static string UpdateItemBarcode_GetQuery(UpdateItemBarcodeRequest request)
        {
            var obj = request.Data;

            int i = 0;
            string str = "";
 
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#items') IS NOT NULL DROP TABLE #items;";

            str += Environment.NewLine + $"select top 0 cast(N'' as char(3)) as stt_rec0,ma_vt,cast(N'' as nvarchar(128)) as barcode, ma_kho,ma_vt as ma_lo, DateTime2 as hsd,DateTime2 as nsx, cast(N'' as numeric(16,4)) as so_can,cast (0 as int ) as [index],cast('' as nvarchar(32)) as ma_pallet into #items from dmvt";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                string _nsx = "";
                if (row.nsx != null)
                {
                    _nsx = $"'{row.nsx}'";
                }
                else {
                    _nsx = "null";
                }

                string _hsd = "";
                if (row.hsd != null)
                {
                    _hsd = $"'{row.hsd}'";
                }
                else
                {
                    _hsd = "null";
                }

                str += Environment.NewLine + $"insert into #items (stt_rec0,ma_vt,barcode, ma_kho,ma_lo, hsd,nsx,so_can,[index], ma_pallet)"
                    + Environment.NewLine + 
                            $" select '{row.sttRec0}','{row.ma_vt}','{row.barcode}','{row.ma_kho}', '{row.ma_lo}', {_hsd} ,{_nsx}, '{row.so_can}', '{row.index_item}', '{row.pallet}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_update_item_barcode] @stt_rec = '{obj.stt_rec}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}'";
            return str;
        }  
        public static string StockTransferConfirm(UpdateItemBarcodeRequest request)
        {
            var obj = request.Data;

            int i = 0;
            int index_barcode = 0;

            string str = "";
 
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#items') IS NOT NULL DROP TABLE #items;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#itemsBarcode') IS NOT NULL DROP TABLE #itemsBarcode;";

            str += Environment.NewLine + $"select top 0 stt_rec,stt_rec0, ma_vt, so_luong into #items from d85$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + $"select top 0 stt_rec0,[index],stt_rec, ma_vt,barcode into #itemsBarcode from b85$000000";
            str += Environment.NewLine + "declare @index_item_barcode int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #items (stt_rec,stt_rec0, ma_vt, so_luong)"
                    + Environment.NewLine + 
                            $" select '{row.sttRec}','{row.sttRec0}','{row.ma_vt}','{row.so_can}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }

            foreach (var row in request.Data.listConfirm)
            {
                str += Environment.NewLine + $"insert into #itemsBarcode (stt_rec0,[index],stt_rec, ma_vt,barcode)"
                    + Environment.NewLine +
                            $" select '{row.sttRec0}','{row.index_item}','{row.sttRec}','{row.ma_vt}','{row.barcode}'";
                str += Environment.NewLine + "set  @index_item_barcode =@index_item_barcode + 1";

                index_barcode += 1;
            }

            str += Environment.NewLine + $"exec [app_Stock_Transfer_Confirm] @stt_rec = '{obj.stt_rec}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}'";
            return str;
        }
      
        public static string UpdateItemQuantity_GetQuery(UpdateItemBarcodeRequest request)
        {
            var obj = request.Data;

            int i = 0;
            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#items') IS NOT NULL DROP TABLE #items;";

            str += Environment.NewLine + $"select top 0 stt_rec ,ma_vt,so_luong into #items from d96$000000";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.listConfirm)
            {
                str += Environment.NewLine + $"insert into #items (stt_rec,ma_vt, so_luong)"
                    + Environment.NewLine +$"select '{row.sttRec}','{row.ma_vt}','{row.so_can}'";
                str += Environment.NewLine + "set @index_item =@index_item + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_Purchase_Receipt_Confirm] @Action = '{obj.action}',@stt_rec = '{obj.stt_rec}',@UnitId = '{request.UnitId}', @UserId = '{request.UserId}'";
            return str;
        }
        public static string ItemLocaionModify_GetQuery(ItemLocaionModifyRequest request)
        {
            var obj = request.Data;

            int i = 0;
            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#items') IS NOT NULL DROP TABLE #items;";

            str += Environment.NewLine + $"select top 0  ma_vt,ma_vi_tri, so_luong, CAST(0 as int) AS nxt , cast('' as nvarchar(32)) as ma_pallet,cast('' as nvarchar(256)) as barcode into #items from d64$000000";
            str += Environment.NewLine + "declare @index_itemBarCode int =0";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                string _pallet = "";
                if (row.pallet != null)
                {
                    _pallet = $"'{row.pallet}'";
                }
                else
                {
                    _pallet = "null";
                }
                str += Environment.NewLine + $"insert into #items (ma_vt,ma_vi_tri, so_luong, nxt,ma_pallet,barcode)"
                    + Environment.NewLine +
                            $" select '{row.ma_vt}','{row.ma_vi_tri}','{row.so_luong}', '{row.nxt}', {_pallet}, '{row.barcode}'";
                str += Environment.NewLine + "set  @index_itemBarCode =@index_itemBarCode + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_item_location_modify]  @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}'";
            return str;
        }
        public static string CreateItemHolder_GetQuery(CreateItemHolderRequest request)
        {

            var obj = request.Data;

            int i = 0;
            int indexCustomerDetail = 1;

            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#customerDetail') IS NOT NULL DROP TABLE #customerDetail;";

            str += Environment.NewLine + $"select top 0 * into #master from phgh$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from ctgh$000000";
            str += Environment.NewLine + $"select top 0 * into #customerDetail from khgh$000000";
            str += Environment.NewLine + "declare @index_item int =0";
            str += Environment.NewLine + "declare @indexCustomerDetail int =0";

            str += Environment.NewLine + $"insert into #master (stt_rec,ma_ct,ma_dvcs,ma_kh,dien_giai,ngay_het_han)" + Environment.NewLine +
                        $" select '','','{request.UnitId}','',N'{obj.comment}', '{obj.ngay_het_han}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.listItem)
            {
                str += Environment.NewLine + $"insert into #detail (stt_rec,ngay_ct,so_ct,ma_ct,stt_rec0,ma_vt,dvt,so_luong,ma_dvcs,gia,gia_nt2)";
                str += Environment.NewLine + $"SELECT '',getDate(),'','',RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row.ma_vt}','{row.dvt}','{row.so_luong}','{row.ma_dvcs}','{row.gia}','{row.gia_nt2}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";

                foreach (var row2 in row.listCustomer)
                {
                    str += Environment.NewLine + $"insert into #customerDetail (stt_rec,ngay_ct,so_ct,ma_ct,stt_rec0,ma_vt,so_luong,dvt,ma_kh,ma_dvcs)";
                    str += Environment.NewLine + $"SELECT '',getDate(),'','',RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row2.ma_vt}','{row2.so_luong}','{row2.dvt}','{row2.ma_kh}','{row2.ma_dvcs}'";
                    str += Environment.NewLine + "set  @indexCustomerDetail = @indexCustomerDetail + 1";
                    indexCustomerDetail += 1;
                }

                i += 1;
            }

            str += Environment.NewLine + $"exec[app_Item_Hold_Voucher_Modifier] @stt_rec = '{obj.stt_rec}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}',@Currency = '', @Lang = '{request.Lang}', @StockCode = '{request.StoreId}', @Comment = N'{obj.comment}', @Status = null";

            return str;
        }
    }
}