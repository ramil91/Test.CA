using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Invalid credentials."
                };
                return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var defaultResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the middleware."
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(defaultResponse));
        }
    }
}
