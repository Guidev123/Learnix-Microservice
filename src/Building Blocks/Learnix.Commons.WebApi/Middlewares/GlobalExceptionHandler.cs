using Learnix.Commons.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Learnix.Commons.WebApi.Middlewares
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
            : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "Server failure",
                Detail = GetExceptionMessage(exception)
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static string GetExceptionMessage(Exception? exception)
        {
            return exception switch
            {
                LearnixException learnixException when learnixException.Error?.Description is not null => learnixException.Error.Description,
                _ when exception?.InnerException?.Message is not null => exception.InnerException.Message,
                _ => exception?.Message ?? "Unknown error"
            };
        }
    }
}