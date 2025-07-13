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
            if (string.IsNullOrEmpty(userId)) return this.Unauthorized();

            IEnumerable<OrderItemViewModel> cartItems = await this._orderService.GetCartItemsAsync(userId);
            return this.View(cartItems);
        }
        catch (Exception e)
        {
            return this.RedirectToAction(nameof(Index), "Product");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(AddToCartFormModel model)
    {
        try
        {
            string? userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            if (string.IsNullOrEmpty(model.ProductId) && string.IsNullOrEmpty(model.ComputerId))
            {
                ModelState.AddModelError(string.Empty, "Product or Computer ID is required");
                return this.RedirectToAction(nameof(Index), "Product");
            }

            if (model.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than 0");
                return this.RedirectToAction(nameof(Index), "Product");
            }

            await this._orderService.AddProductToCartAsync(model, userId);

            TempData["SuccessMessage"] = "Product added to cart successfully!";

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = "Failed to add product to cart. Please try again.";
            return this.RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remove(string id)
    {
        try
        {
            string? userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId)) return this.Unauthorized();

            await this._orderService.RemoveItemAsync(id, userId);

            TempData["SuccessMessage"] = "Item removed from cart successfully!";

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = "Failed to remove item from cart.";
            return this.RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        try
        {
            string? userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId)) return this.Unauthorized();

            await this._orderService.ClearCartAsync(userId);

            TempData["SuccessMessage"] = "Cart cleared successfully!";

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = "Failed to clear cart.";
            return this.RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Finalize()
    {
        try
        {
            string? userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId)) return this.Unauthorized();

            await this._orderService.FinalizeOrderAsync(userId);

            TempData["SuccessMessage"] = "Order finalized successfully!";

            return this.RedirectToAction(nameof(Index), "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = "Failed to finalize order.";
            return this.RedirectToAction(nameof(Index));
        }
    }
}