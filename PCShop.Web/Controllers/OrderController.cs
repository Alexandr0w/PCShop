using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.Controllers;
using PCShop.Web.ViewModels.Order;
using static PCShop.GCommon.ErrorMessages;

[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        this._orderService = orderService;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            IEnumerable<OrderItemViewModel> cartItems = await this._orderService.GetCartItemsAsync(userId);
            return this.View(cartItems);
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message);
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
                ModelState.AddModelError(string.Empty, RequiredIdMessage);
                this._logger.LogError(RequiredIdMessage);

                return this.RedirectToAction(nameof(Index), "Product");
            }

            if (model.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", QuantityMustBeMessage);
                this._logger.LogError(QuantityMustBeMessage);

                return this.RedirectToAction(nameof(Index), "Product");
            }

            await this._orderService.AddProductToCartAsync(model, userId);

            TempData["SuccessMessage"] = "Product added to cart successfully!";

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message);
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

            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            await this._orderService.RemoveItemAsync(id, userId);

            TempData["SuccessMessage"] = "Item removed from cart successfully!";

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message);
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

            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            await this._orderService.ClearCartAsync(userId);

            TempData["SuccessMessage"] = "Cart cleared successfully!";

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message);
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

            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            await this._orderService.FinalizeOrderAsync(userId);

            TempData["SuccessMessage"] = "Order finalized successfully!";

            return this.RedirectToAction(nameof(Index), "Home");
        }
        catch (Exception e)
        {
            this._logger.LogError(e.Message);
            TempData["ErrorMessage"] = "Failed to finalize order.";

            return this.RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Confirm()
    {
        try
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            bool isProfileComplete = await this._orderService.IsUserProfileCompleteAsync(userId);

            if (!isProfileComplete)
            {
                TempData["ErrorMessage"] = "Please complete your profile before placing an order.";
                return this.RedirectToAction("EditProfile", "Account");
            }

            OrderConfirmationViewModel? model = await this._orderService.GetOrderConfirmationDataAsync(userId);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No pending order found or cart is empty.";
                return this.RedirectToAction(nameof(Index));
            }

            this.ModelState.Clear();

            return this.View(model);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Failed to load order confirmation form.");
            TempData["ErrorMessage"] = "Unable to load confirmation form.";
            return this.RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Confirm(OrderConfirmationViewModel model)
    {
        try
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.TotalProductsPrice = await this._orderService.GetTotalCartPriceAsync(userId);
                return this.View(model);
            }

            bool isFinalized = await this._orderService.FinalizeOrderWithDetailsAsync(userId, model);

            if (isFinalized)
            {
                TempData["SuccessMessage"] = "Order finalized successfully!";
                return this.RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to finalize order. Please try again.";

                decimal totalPrice = await this._orderService.GetTotalCartPriceAsync(userId);
                model.TotalProductsPrice = totalPrice;

                return this.View(model);
            }
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error finalizing order");
            TempData["ErrorMessage"] = "An error occurred while finalizing your order.";

            return this.RedirectToAction(nameof(Index));
        }
    }
}