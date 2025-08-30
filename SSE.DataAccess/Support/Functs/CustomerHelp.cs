using SSE.Common.Api.v1.Requests.Customer;
using SSE.Common.DTO.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SSE.DataAccess.Support.Functs
{
    class CustomerHelp
    {
        public static string CreateCareCustomer_GetQuery(CustomerCareCreateResquest request)
        {

            List<SurveyDTO> listSurvey = new List<SurveyDTO>();
            string surveyJson = request.ListSurvey;

            //var surveyList = JsonSerializer.Deserialize<List<SurveyDTO>>(surveyJson);

            var root = JsonDocument.Parse(surveyJson);
            var dataJson = root.RootElement.GetProperty("data").GetRawText();

            var surveyList = JsonSerializer.Deserialize<List<SurveyDTO>>(dataJson);

            // Xử lý logic với surveyList
            foreach (var survey in surveyList)
            {
                listSurvey.Add(survey);
            }

            int i = 0;
            string str = "";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#image') IS NOT NULL DROP TABLE #image;";

            str += Environment.NewLine + $"select top 0 * into #image from fsdImagePath";
            str += Environment.NewLine + "declare @index_item int =0";

            str += Environment.NewLine + "IF OBJECT_ID('TempDb..#detail') IS NOT NULL DROP TABLE #detail;";
            str += Environment.NewLine + $"select top 0 stt_rec,stt_rec0,ngay_ct,dien_giai,ma_td1,ma_td2 into #detail from dkh$000000";
            str += Environment.NewLine + "declare @index_item_detail int =0";

            foreach (var row in request.Detail)//select top 0 code, path_l, name, ma_album
            {
                str += Environment.NewLine + $"insert into #image (code, path_l, name, ma_album,ma_kh)";
                str += Environment.NewLine + $"SELECT '{row.CodeImage}', '{row.Path}','{row.NameImage}', '{row.codeAlbum}','{request.CustomerCode}'";
                str += Environment.NewLine + "set  @index_item = @index_item + 1";

                i += 1;
            }


            str += Environment.NewLine;
            foreach (var row in listSurvey)
            {
                str += Environment.NewLine + $"insert into #detail (stt_rec,stt_rec0,ngay_ct,dien_giai,ma_td1,ma_td2)"
                    + Environment.NewLine +
                            $" select '', '{row.stt_rec0}','{DateTime.Now}',N'{row.dien_giai}','{row.stt_rec0}','{row.ma_td2}'";

                str += Environment.NewLine + "set  @index_item_detail =@index_item_detail + 1";
            }


            str += Environment.NewLine + $"exec[app_Customer_Care_Create] @CustomerId = '{request.CustomerCode}', @Type = '{request.TypeCare}'," +
                $" @UserId = '{request.UserId}',@UnitId  = '{request.UnitId}',@Description = N'{request.Description}'," +
                $"@Feedback = N'{request.Feedback}',@Admin = '{request.Admin}',@StoreId = '{request.StoreId}'," +
                $"@Lang  = '{request.Lang}'";

            return str;
        }
    }
}
