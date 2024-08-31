using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StockProj.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UseCustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public UseCustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                //can log here

                httpContext.Items["errorMsg"] = ex.Message;

                //httpContext.Response.StatusCode = 500;
                //httpContext.Response.Headers["errorMsg"] = "Can not found";

                throw;
            }
            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomExeptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseCustomExceptionHandlerMiddleware>();
        }
    }
}
