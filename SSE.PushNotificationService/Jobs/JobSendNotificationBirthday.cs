//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json.Linq;
//using Quartz;
//using SSE.Business.Api.v1.Interfaces;
//using SSE.Common.Api.v1.Requests.Notification;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Transshipment.Common.Constants.v1;

//namespace SSE.PushNotificationService.Jobs
//{
//    public class JobSendNotificationBirthday : IJob
//    {
//        private readonly IHRBLL _iHRBLL;
//        private readonly INotificationBLL _iNotificationBLL;
//        private readonly IConfiguration _configuration;

//        public JobSendNotificationBirthday(INotificationBLL iNotificationBLL, IConfiguration configuration, IHRBLL iHRBLL)
//        {
//            _iHRBLL = iHRBLL;
//            _iNotificationBLL = iNotificationBLL;
//            _configuration = configuration;
//        }
//        public async Task Execute(IJobExecutionContext context)
//        {
//            try
//            {
//                var resultDanhSachSinhNhanh = await _iHRBLL.GetListCustomerBirthday();
//                if (resultDanhSachSinhNhanh.Data != null && resultDanhSachSinhNhanh.Data.Count() > 0)
//                {
//                    string message = _configuration["NotificationConfig:SendNotification:Message"];
//                    var data = new JObject();
//                    data["EVENT"] = NOTIFICATION_EVENT.BIRTH_DAY;
//                    var listTaiKhoanIds = resultDanhSachSinhNhanh.Data.Distinct().ToList();

//                    var notificationRequest = new SendNotificationRequest()
//                    {
//                        Title = "Thông báo",
//                        Body = message,
//                        IdTaiKhoans = listTaiKhoanIds,
//                        Data = data
//                    };

//                    await _iNotificationBLL.SendNotification(notificationRequest);
//                }
//                //var path = System.IO.Directory.GetCurrentDirectory() + "\\Logs\\WarningNotAcceptCustomer";
//                //if (!Directory.Exists(path))
//                //{
//                //    Directory.CreateDirectory(path);
//                //}
//                //var logfile = path + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
//                //using (StreamWriter _testData = new StreamWriter(logfile, true))
//                //{
//                //    _testData.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": Service is recall"); // Write the file.
//                //}
//            }
//            catch (Exception ex)
//            {
//                var path = System.IO.Directory.GetCurrentDirectory() + "\\Logs\\SendNotificationBirthday";
//                if (!Directory.Exists(path))
//                {
//                    Directory.CreateDirectory(path);
//                }
//                var logfile = path + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
//                using (StreamWriter _testData = new StreamWriter(logfile, true))
//                {
//                    _testData.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": " + ex.Message); // Write the file.
//                }
//                throw;
//            }
//        }
//    }
//}
