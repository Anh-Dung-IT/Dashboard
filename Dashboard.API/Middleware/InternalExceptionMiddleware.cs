using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dashboard.API.Middleware
{
    public class InternalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InternalExceptionMiddleware> _logger;

        public InternalExceptionMiddleware(RequestDelegate next, ILogger<InternalExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, context);
            }
        }

        private Task HandleExceptionAsync(Exception ex, HttpContext context)
        {
            var error = new
            {
                Id = Guid.NewGuid().ToString(),
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occurred in the API. Please use the ID and contact our support team if the problem persists.",
            };

            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
               ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogCritical(ex, "BADNESS!!! " + ex.Message + " -- {ErrorId}", error.Id);
            }

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(result);
        }
    }
}
