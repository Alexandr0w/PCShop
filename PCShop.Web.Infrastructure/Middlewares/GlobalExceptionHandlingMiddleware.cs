using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.GCommon.ErrorMessages.Common;

namespace PCShop.Web.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ExceptionOccurred);

                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect(InternalServerError500Path);
                }
            }
        }
    }
}
