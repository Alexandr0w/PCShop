using Microsoft.AspNetCore.Builder;
using PCShop.Web.Infrastructure.Middlewares;

namespace PCShop.Web.Infrastructure.Extensions
{
    public static class GlobalExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
