using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using SSE.PushNotificationService.Common.Requests;
using SSE.PushNotificationService.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using WindowService_POC.Contract;

namespace WindowService_POC.Tasks
{
    public class Task1 : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public static IConfiguration _configuration;


        public Task1(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        //public Task1(IServiceProvider serviceProvider, INotificationBLL iNotificationBLL)
        //{
        //    _serviceProvider = serviceProvider;
        //    //_iNotificationBLL = iNotificationBLL;
        //}

        public async Task Execute(IJobExecutionContext context)
        {
            var x = 100;
            var y = 2000;
            var connection = new SqlConnection(_configuration["ConnectionStrings:GlobalConnectionString"]);
            connection.StatisticsEnabled = true;
            connection.FireInfoMessageEventOnUserErrors = true;
            connection.Open();

            using (DbCommand command = connection.CreateCommand())
            {
               
                command.CommandText = "exec getCustomerBirthInDay";
                var reader = command.ExecuteReader();
             
                Console.WriteLine("\r\nPush notification:");

                string message = _configuration["NotificationConfig:SendNotification:Message"];
                var data = new JObject();
                data["EVENT"] = NOTIFICATION_EVENT.BIRTH_DAY;
                List<string> listTaiKhoanIds = new List<string>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        listTaiKhoanIds.Add($"{reader["uuid"]}");
                    }
                    //List<string> list = new List<string>();
                    //list.Add("faGYmO6sSheLsnJJwXhSju:APA91bGA2Jn6AewRZqaN22CXR9GJY8HY7O1vA4LwlL-ydvB0OyygXCxOdDj-biEO1KBCQBsERXhAbEFed6vjjYW5ZIC8CUb55yRFWwrc4CaEBB8nCxof4snLp32qBjmb2xviVWGydeWn");

                    var resultSendNotification = await SendNotification(new SendNotificationRequest()
                    {
                        Title = "Bạn có KH sinh nhật vào ngày hôm nay",
                        Body = "Vui lòng vào báo cáo để xem chi tiết.",
                        Type = NOTIFICATION_TYPE.NotificationOnly,
                        Data = data,
                        DriverTokens = listTaiKhoanIds
                    }); ;

                }
               
            }
            connection.Close();

            using var scope = _serviceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<ITaskLogTime>();
            await svc.DoWork(context.CancellationToken);
            await Task.CompletedTask;
        }

        public async Task<bool> SendNotification(SendNotificationRequest notificationRequest)
        {
            bool isSuccess = false;

            var fb = _configuration.GetSection(NOTIFICATION_EVENT.FIRE_BASE);
            string fireBaseUrl = fb.GetSection(NOTIFICATION_EVENT.FIRE_BASE_URL).Value;
            string fireBaseServerKey = fb.GetSection(NOTIFICATION_EVENT.FIRE_BASE_AUTHORIZATION).Value;

            dynamic notificationInformation;

            if (notificationRequest.Type == NOTIFICATION_TYPE.NotificationAndReloadData || notificationRequest.Type == NOTIFICATION_TYPE.NotificationOnly)
            {
                notificationInformation = new
                {
                    registration_ids = notificationRequest.DriverTokens.ToList(),
                    notification = new
                    {
                        title = notificationRequest.Title,
                        body = notificationRequest.Body,
                        sound = "default",
                        priority = "high",
                        content_available = true,
                        mutable_content = true
                    },
                    data = notificationRequest.Data
                };
            }
            else
            {
                notificationInformation = new
                {
                    registration_ids = notificationRequest.DriverTokens.ToList(),
                    data = notificationRequest.Data
                };
            }

            string jsonMessage = JsonConvert.SerializeObject(notificationInformation);
            var request = new HttpRequestMessage(HttpMethod.Post, fireBaseUrl);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + fireBaseServerKey);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage send_result = null;

            using (var client = new HttpClient())
            {
                try
                {
                    send_result = await client.SendAsync(request);
                    isSuccess = send_result.IsSuccessStatusCode;
                    //loggerManager.LogInformation("Send message status: " + send_result.IsSuccessStatusCode + " - Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }
                catch (System.Exception ex)
                {
                   // loggerManager.LogError(ex.Message, ex);
                }
            }

            return isSuccess;
        }
    }


//    using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Primitives;
//using Newtonsoft.Json;
//using Quartz;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Security.Cryptography.Xml;
//using System.Security.Policy;
//using System.Text;
//using System.Threading.Tasks;


//namespace WindowService_POC.Tasks
//    {
//        public class Task1 : IJob
//        {
//            private readonly IServiceProvider _serviceProvider;
//            public static IConfiguration _configuration;
//            private readonly HttpClient _httpClient;

//            private const string URL = "https://hiepphong-cloud.sse.net.vn/api/v1/hr/push-notification-birthday";
//            private string urlParameters = "?api_key=123";

//            public Task1(IServiceProvider serviceProvider, IConfiguration configuration, HttpClient client)
//            {
//                _serviceProvider = serviceProvider;
//                _configuration = configuration;
//                _httpClient = client;
//            }

//            //public Task1(IServiceProvider serviceProvider, INotificationBLL iNotificationBLL)
//            //{
//            //    _serviceProvider = serviceProvider;
//            //    //_iNotificationBLL = iNotificationBLL;
//            //}

//            public async Task Execute(IJobExecutionContext context)
//            {
//                var x = 100;
//                var y = 2000;
//                using var client = new HttpClient();
//                //client.BaseAddress = new Uri(URL);
//                // Add an Accept header for JSON format.
//                var id = new { id = 0 };
//                var jsonPut = JsonConvert.SerializeObject(id);
//                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                // Get data response
//                var response = client.PutAsync(URL, new StringContent(jsonPut, Encoding.UTF8, "application/json")).Result;

//                if (response.IsSuccessStatusCode)
//                {
//                    // Parse the response body
//                    var dataObjects = response.Content
//                                   .ReadAsAsync<IEnumerable<DataObject>>().Result;
//                    foreach (var d in dataObjects)
//                    {
//                        Console.WriteLine("{0}", d.Data);
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("{0} ({1})", (int)response.StatusCode,
//                                  response.ReasonPhrase);
//                }


//                //using var scope = _serviceProvider.CreateScope();
//                //var svc = scope.ServiceProvider.GetRequiredService<ITaskLogTime>();
//                //await svc.DoWork(context.CancellationToken);

//                //await Task.CompletedTask;
//            }
//        }
//    }

}
