using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Filters
{
    public class UnhandledExceptionMiddleware
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message.ToString(),
                    $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");

                await context.Response.WriteAsJsonAsync(
                       new { msg = exception.Message.ToString() , path = context.Request?.Path.Value }
                       );

            }
        }
    }
}
