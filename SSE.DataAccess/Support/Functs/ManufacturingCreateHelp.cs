using Newtonsoft.Json.Linq;
using SSE.Common.Api.v1.Requests.Manufacturing;
using System;
using System.Collections.Generic;
using System.IO;

namespace SSE.DataAccess.Support.Functs
{
    public class ManufacturingCreateHelp
    {
        public static string ManufacturingCreate_GetQueryV3(ManufacturingRequest request)
        {
            var obj = request.Data;

            int i = 0;
        
            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#master') IS NOT NULL DROP TABLE #master;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#rawTable') IS NOT NULL DROP TABLE #rawTable;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#wasteTable') IS NOT NULL DROP TABLE #wasteTable;";
            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#machineTable ') IS NOT NULL DROP TABLE #machineTable ;";

            str += Environment.NewLine + $"select top 0 * into #master from ms1$000000";
            str += Environment.NewLine + $"select top 0 * into #detail from ds1$000000";
            str += Environment.NewLine + $"select top 0 * into #rawTable from es1$000000";
            str += Environment.NewLine + $"select top 0 * into #wasteTable from fs1$000000";
            str += Environment.NewLine + $"select top 0 * into #machineTable from gs1$000000";
            str += Environment.NewLine + "declare @index_item int =0";
            str += Environment.NewLine + "declare @indexRawTable int =0";
            str += Environment.NewLine + "declare @indexWasteTable int =0";
            str += Environment.NewLine + "declare @indexMachineTable int =0";

           
            str += Environment.NewLine + $"insert into #master" +
                $"(stt_rec,ma_dvcs,ma_gd,ngay_ct,ghi_chu,t_so_luong,ma_nc, ma_px, ma_lsx, ma_cd,ma_ca,sl_nc,gio_bd,gio_kt)"
                +Environment.NewLine +
                        $" select '','{request.UnitId}', '{obj.ma_gd}', '{obj.ngay_ct}', N'{obj.ghi_chu}', '{obj.t_so_luong}', '{obj.ma_nc}', '{obj.ma_px}', '{obj.ma_lsx}', '{obj.ma_cd}', " +
                        $"'{obj.ma_ca}', '{obj.sl_nc}', '{obj.gio_bd}','{obj.gio_kt}'";

            str += Environment.NewLine;
            foreach (var row in request.Data.Detail)
            {
                str += Environment.NewLine + $"insert into #detail (stt_rec,stt_rec0,ma_vt,so_luong,dvt,ma_nc,nh_nc,ghi_chu,ma_lo)" 
                    + Environment.NewLine +
                            $" select '', RIGHT('000' + CAST((@index_item + 1) AS NVARCHAR(10)), 3 ),'{row.ma_vt}','{row.so_luong}',N'{row.dvt}', '{row.ma_nc}','{row.nh_nc}', " +
                            $"N'{row.ghi_chu}', '{row.ma_lo}'";
                str += Environment.NewLine + "set  @index_item =@index_item + 1";

                i += 1;
            }
            str += Environment.NewLine;

            foreach (var row in request.Data.RawTable)
            {
                str += Environment.NewLine + $"insert into #rawTable (stt_rec,stt_rec0,ma_vt,so_luong,dvt,rework,ma_lo,sl_tn,sl_cl,sl_sd)"
                    + Environment.NewLine +
                            $" select '', RIGHT('000' + CAST((@indexRawTable + 1) AS NVARCHAR(10)), 3 ),'{row.ma_vt}','{row.so_luong}',N'{row.dvt}', '{row.rework}','{row.ma_lo}', " +
                            $"'{row.sl_tn}', '{row.sl_cl}', '{row.sl_sd}'";
                str += Environment.NewLine + "set  @indexRawTable =@indexRawTable + 1";

                i += 1;
            }

            str += Environment.NewLine;

            foreach (var row in request.Data.WasteTable)
            {
                str += Environment.NewLine + $"insert into #wasteTable (stt_rec,stt_rec0,ma_vt,so_luong,dvt,ma_kho)"
                    + Environment.NewLine +
                            $" select '', RIGHT('000' + CAST((@indexWasteTable + 1) AS NVARCHAR(10)), 3 ),'{row.ma_vt}','{row.so_luong}',N'{row.dvt}','{row.code_store}'";
                str += Environment.NewLine + "set  @indexWasteTable =@indexWasteTable + 1";

                i += 1;
            }

            str += Environment.NewLine;

            foreach (var row in request.Data.MachineTable)
            {
                str += Environment.NewLine + $"insert into #machineTable (stt_rec,stt_rec0,ma_may,gio_bd,gio_kt,so_gio,ghi_chu)"
                    + Environment.NewLine +
                            $" select '', RIGHT('000' + CAST((@indexMachineTable + 1) AS NVARCHAR(10)), 3 ),'{row.ma_may}','{row.gio_bd}','{row.gio_kt}', '{row.so_gio}', " +
                            $"N'{row.ghi_chu}'";
                str += Environment.NewLine + "set  @indexMachineTable =@indexMachineTable + 1";

                i += 1;
            }

            str += Environment.NewLine + $"exec [app_Factory_Transaction_voucher_modifier] @stt_rec  = '{obj.stt_rec}', @Comment = '{obj.ghi_chu}', @Status = '{1}', @UnitId = '{request.UnitId}', @UserId = '{request.UserId}', @Lang = '{request.Lang}'";
            return str;
        }
    }
}