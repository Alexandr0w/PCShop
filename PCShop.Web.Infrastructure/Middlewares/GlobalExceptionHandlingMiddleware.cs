using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
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

                if (!context.Response.HasStarted)
                {
                    switch (context.Response.StatusCode)
                    {
                        case (int)HttpStatusCode.Forbidden: // 403
                            context.Response.Redirect(Unauthorized403Path);
                            break;

                        case (int)HttpStatusCode.NotFound: // 404
                            context.Response.Redirect(NotFound404Path);
                            break;
                    }
                }
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
