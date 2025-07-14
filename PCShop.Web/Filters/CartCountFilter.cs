using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PCShop.Services.Core.Interfaces;

namespace PCShop.Web.Filters
{
    public class CartCountFilter : IAsyncActionFilter
    {
        private readonly IOrderService _orderService;

        public CartCountFilter(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Controller? controller = context.Controller as Controller;

            if (controller != null && controller.User.Identity?.IsAuthenticated == true)
            {
                string? userId = controller.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    int cartCount = await this._orderService.GetCartCountAsync(userId);
                    controller.ViewBag.CartCount = cartCount;
                }
            }

            await next();
        }
    }
}