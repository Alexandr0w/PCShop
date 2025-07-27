using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PCShop.Web.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string InternalServerError500Path = "/Home/Error?statusCode=500";

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect(InternalServerError500Path);
                }
            }
        }
    }
}
