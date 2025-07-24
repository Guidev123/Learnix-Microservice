using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Diagnostics;

namespace Learnix.Commons.WebApi.Middlewares
{
    public sealed class LogContextTraceLoggingMiddleware(RequestDelegate next)
    {
        public Task Invoke(HttpContext context)
        {
            var traceId = Activity.Current?.TraceId.ToString();

            using (LogContext.PushProperty("TraceId", traceId))
            {
                return next.Invoke(context);
            }
        }
    }
}