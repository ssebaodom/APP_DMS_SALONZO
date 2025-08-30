using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSE.Common.Constants.v1;
using SSE.Core.Common.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SSE_Server.Api.v1.Base
{
    public class ExceptionController : ControllerBase
    {
        private readonly ILogger<ExceptionController> _logger;

        public ExceptionController(ILogger<ExceptionController> logger)
        {
            _logger = logger;
        }

        [Route("/exception")]
        public async Task<IActionResult> HandleExceptionAsync()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception exception = context.Error;
            int code = StatusCodes.Status500InternalServerError;

            var request = HttpContext.Request;

            string body = "";
            if (request.Method == "POST" || request.Method == "PUT")
            {
                request.EnableBuffering();
                body = await new StreamReader(request.Body).ReadToEndAsync();
                request.Body.Position = 0;
            }

            string requestInfo = (
                $"Http Request Information:{Environment.NewLine}" +
                $"Schema:{request.Scheme} {Environment.NewLine}" +
                $"Host: {request.Host} {Environment.NewLine}" +
                $"Path: {request.Path} {Environment.NewLine}" +
                $"QueryString: {request.QueryString} {Environment.NewLine}" +
                $"Request Body: {body} {Environment.NewLine}"
            );

            _logger.LogError(exception, requestInfo);

            // Return family reponse
            string message = API_STRINGS.EXCEPTION_MESS_DEFAULT;
            try
            {
                code = exception.Data["code"].ToInt32();
                message = exception.Message;
            }
            catch (Exception) { }

            switch (code)
            {
                case StatusCodes.Status404NotFound:
                    message = API_STRINGS.EXCEPTION_MESS_NOT_FOUND;
                    code = StatusCodes.Status404NotFound;
                    break;

                case StatusCodes.Status403Forbidden:
                    message = API_STRINGS.EXCEPTION_MESS_FORBIDDEN;
                    code = StatusCodes.Status403Forbidden;
                    break;
            }

            return StatusCode(code, new
            {
                status_code = code,
                message
            });
        }
    }
}