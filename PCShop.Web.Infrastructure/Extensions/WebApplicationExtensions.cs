using Microsoft.AspNetCore.Builder;
using PCShop.Web.Infrastructure.Middlewares;

namespace PCShop.Web.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
