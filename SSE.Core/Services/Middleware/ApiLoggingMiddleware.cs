using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSE.Core.Services.Dapper;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SSE.Common.Services
{
    public class ApiLoggingMiddleware
    {
        private RequestDelegate next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            // Log các request gửi lên server.
            if (request.Path.StartsWithSegments(new PathString("/api")))
            {
                string body = "";
                if (request.Method == "POST" || request.Method == "PUT")
                {
                    context.Request.EnableBuffering();
                    body = await new StreamReader(request.Body).ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                _logger.LogInformation(
                    $"Http Request Information:{System.Environment.NewLine}" +
                    $"Schema:{request.Scheme} {System.Environment.NewLine}" +
                    $"Host: {request.Host} {System.Environment.NewLine}" +
                    $"Path: {request.Path} {System.Environment.NewLine}" +
                    $"QueryString: {request.QueryString} {System.Environment.NewLine}" +
                    $"Request Body: {body} {System.Environment.NewLine}"
                );
                //try
                //{
                //    //string h = Newtonsoft.Json.JsonConvert.SerializeObject(request.Headers);
                //    string sql = $"insert into app_apilog values(GETDATE(),N'{request.QueryString.ToString()}',N'{request.Headers.ToString()}',N'{body.ToString().Replace("'", "")}',N'{request.Host.ToString()}','{request.Path.ToString()}','')";
                //    dapperService.Execute(sql, null, System.Data.CommandType.Text);
                //}
                //catch (Exception e)
                //{

                //}
            }

            await next.Invoke(context);

            // Log Error. Hiện không dùng vì đang sử dụng ExceptionController.
            //try
            //{
            //    // Do work that doesn't write to the Response.
            //    await next.Invoke(context);
            //    // Do other work that doesn't write to the Response.
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("Internal Exception", exception);
            //    await HandleExceptionAsync(context, ex);
            //}
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new { status_code = 500, message = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}