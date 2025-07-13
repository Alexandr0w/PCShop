using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.Controllers;
using PCShop.Web.ViewModels.Order;

[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        this._orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.Unauthorized();
            }

            IEnumerable<OrderItemViewModel> cartItems = await this._orderService.GetCartItemsAsync(userId);
            return this.View(cartItems);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return this.RedirectToAction(nameof(Index), "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Guid productId, int quantity = 1)
    {
        try
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return this.Unauthorized();
            }

            await this._orderService.AddProductToCartAsync(userId, productId, quantity);
            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return this.RedirectToAction(nameof(Index));
        }
    }
}
