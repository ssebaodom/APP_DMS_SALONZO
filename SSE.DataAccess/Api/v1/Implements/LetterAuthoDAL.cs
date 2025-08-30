using Dapper;
using SSE.Common.Api.v1.Requests.LetterAutho;
using SSE.Common.Api.v1.Results.LetterAutho;
using SSE.Common.Constants.v1;
using SSE.Common.DTO.v1;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Api.v1.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.DataAccess.Api.v1.Implements
{
    public class LetterAuthoDAL : ILetterAuthoDAL
    {
        private readonly IDapperService dapperService;

        public LetterAuthoDAL(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }

        public async Task<LetterAuDisplayResult> LetterAuDisplay(LetterAuDisplayResquest letterAuDisplay)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<LetterAuDisplayResquest>(letterAuDisplay);

            var result = await dapperService.QueryAsync<LetterAuDisplayDTO>("[app_LetterAu_Display]", parameters);

            if (result == null)
            {
                return new LetterAuDisplayResult
                {
                    IsSucceeded = false
                };
            }
            return new LetterAuDisplayResult
            {
                IsSucceeded = true,
                Data = result
            };
        }

        public async Task<LetterListResult> LetterList(LetterListResquest letterList)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<LetterListResquest>(letterList);

            string ProceName = "[app_Letter_List]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);

            List<ReportHeaderDescDTO> reportHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            int TotalCount = gridReader.ReadFirst<int>();            
            var data = gridReader.Read<dynamic>().ToList();          
            List<LetterStatusDTO> letterStatuses = gridReader.Read<LetterStatusDTO>().ToList();          
            return new LetterListResult
            {
                IsSucceeded = true,
                Values = data,
                HeaderDesc = reportHeaders,
                Status = letterStatuses,
                TotalCount = TotalCount
            };
        }

        public async Task<LetterApproResult> LetterApproval(LetterApproResquest letterAppro)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<LetterApproResquest>(letterAppro);

            string result = await dapperService.ExecuteScalarAsync<string>("app_Letter_Approval", parameters, CommandType.StoredProcedure);
            int code = int.Parse(result.Split(';')[0]);
            string message = result.Split(';')[1];

            if (code != (int)QueryExcuteCode.Success)
            {
                return new LetterApproResult
                {
                    IsSucceeded = false,
                    Message = message,
                };
            }

            return new LetterApproResult
            {
                IsSucceeded = true,
                Message = message
            };
        }

        public async Task<LetterDetailResult> LetterDetail(LetterDetailResquest letterDetail)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<LetterDetailResquest>(letterDetail);

            string ProceName = "[app_Letter_Detail]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);

            List<ReportHeaderDescDTO> mainReportHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            var mainData = gridReader.ReadFirst<dynamic>();

            List<ReportHeaderDescDTO> detailReportHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            var detaiData = gridReader.Read<dynamic>().ToList();
            return new LetterDetailResult
            {
                IsSucceeded = true,
                MainHeaderDesc = mainReportHeaders,
                MainValues = mainData,
                DetailHeaderDesc = detailReportHeaders,
                DetailValues = detaiData
            };
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public async Task<LetterDetailResult2> LetterDetail2(LetterDetailResquest letterDetail)
        {
            DynamicParameters parameters = dapperService.CreateDynamicParameters<LetterDetailResquest>(letterDetail);

            string ProceName = "[app_Letter_Detail2]";
            GridReader gridReader = await dapperService.QueryMultipleAsync(ProceName, parameters);

            List<ReportHeaderDescDTO> mainHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            dynamic mainData = gridReader.Read<dynamic>().ToList().First();
            //IEnumerable<ExpandoObject> mainData = gridReader.Read<Object>().Select(x => (ExpandoObject)DapperHelpers.ToExpandoObject(x));

            List<ReportHeaderDescDTO> detailHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            var detaiData = gridReader.Read<dynamic>().ToList();

            List<ReportHeaderDescDTO> fanHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            var fanData = gridReader.ReadFirst<dynamic>();

            List<ReportHeaderDescDTO> infoHeaders = gridReader.Read<ReportHeaderDescDTO>().ToList();
            var infoData = gridReader.ReadFirst<dynamic>();

            List<ValuesFile> listFileAttach = new List<ValuesFile>();

            List<ReportFileAttachDTO> dataFileAttach = gridReader.Read<ReportFileAttachDTO>().ToList();
            
            foreach (var values in dataFileAttach) {
                ValuesFile item = new ValuesFile();
                item.valuesFile = ByteArrayToString(values.FileData);
                item.fileName = values.fileName;
                item.fileExtension = values.fileExt;

                listFileAttach.Add(item);

            }
            

            string code = letterDetail.LetterId.Substring(letterDetail.LetterId.Length - 3);
            string str_folder = Directory.GetCurrentDirectory();
            string html_master = System.IO.File.ReadAllText(str_folder + $"/Rpt/Info/{code}.html");
            string html_de_root = System.IO.File.ReadAllText(str_folder + $"/Rpt/Detail/{code}.html");
            string[] html_de = html_de_root.Split("---detailData---");



            //---------------------------- main -----------------------------------------------------------
            List<MainDataDTO> mainResult = new List<MainDataDTO>();

            foreach (var value in mainData)
            {
                string s = value.Key.ToString().Trim();

                string s2 = value.Value==null ? " " : value.Value.ToString().Trim();
                html_master =html_master.Replace("{{master."+s+"}}", $"{s2.Trim()}");
            }

            //---------------------------- detail -----------------------------------------------------------
            if (html_de.Count() > 0)
            {
                foreach (var value in detaiData)
                {
                    int systemDetail = 0;
                    foreach (var valuedetail in value)
                    {
                        string s = valuedetail.Key.ToString().Trim();
                        string s2 = valuedetail.Value== null ? " " : valuedetail.Value.ToString().Trim();
                        if (s.ToLower() == "systemdetail")
                        {
                            try
                            {
                                systemDetail = Convert.ToInt32(s2);
                            }
                            catch (Exception e)
                            {
                                systemDetail = 0;
                            }
                        }
                        html_master = html_master.Replace("{{detail." + s + "}}", $"{s2.Trim()}");
                    }
                    string temp_de = html_de[systemDetail];
                    foreach (var valuedetail in value)
                    {
                        string s = valuedetail.Key.ToString().Trim();
                        string s2 = valuedetail.Value == null ?" " : valuedetail.Value.ToString().Trim();
                        if (s.ToLower() != "systemdetail")
                        {
                            temp_de =temp_de.Replace("{{detail." + s + "}}", $"{s2.Trim()}");
                        }               
                    }
                    temp_de += Environment.NewLine + "---detailData---";
                    html_master = html_master.Replace("---detailData---", temp_de);
                }
                                                                                                                                                                                                       
            }
            html_master = html_master.Replace("---detailData---", "");
            return new LetterDetailResult2
            {
                IsSucceeded = true,
                Data = html_master,
                listValuesFile = listFileAttach
            };
        }
    }
}