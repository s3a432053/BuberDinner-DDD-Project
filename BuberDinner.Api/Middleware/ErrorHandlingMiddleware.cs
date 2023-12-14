using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BuberDinner.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // ASP.NET Core 捕捉到 Exception 後 會自動執行 名稱叫 Invoke 的 Function
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Create a ProblemDetails object
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message
            };

            // Set the content type to application/problem+json
            context.Response.ContentType = "application/problem+json";

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = JsonSerializer.Serialize(problemDetails);
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
