using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Data.Models.Enum;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.Controllers;
using PCShop.Web.ViewModels.Order;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.MessageConstants.OrderMessages;

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
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(LoadPage.IndexError, ex.Message));
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
                ModelState.AddModelError(string.Empty, Common.RequiredId);
                this._logger.LogError(Common.RequiredId);

                return this.RedirectToAction(nameof(Index), "Product");
            }

            if (model.Quantity <= 0)
            {
                ModelState.AddModelError(string.Empty, Common.QuantityGreaterThanZero);
                this._logger.LogError(Common.QuantityGreaterThanZero);

                return this.RedirectToAction(nameof(Index), "Product");
            }

            await this._orderService.AddProductToCartAsync(model, userId);

            TempData["SuccessMessage"] = AddedToCartSuccessfully;

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.AddError, ex.Message));
            TempData["ErrorMessage"] = AddToCartFailed;

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

            TempData["SuccessMessage"] = ItemRemovedSuccessfully;

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.RemoveError, ex.Message));
            TempData["ErrorMessage"] = RemoveItemFailed;

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

            TempData["SuccessMessage"] = ClearedSuccessfully;

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.ClearError, ex.Message));
            TempData["ErrorMessage"] = ClearCartFailed;

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

            OrderConfirmationInputModel? model = await this._orderService.GetOrderConfirmationDataAsync(userId);

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.LoadConfirmationError, ex.Message));
            TempData["ErrorMessage"] = OrderConfirmationFailed;

            return this.RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Confirm(OrderConfirmationInputModel model)
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

            if (model.DeliveryMethod == DeliveryMethod.None)
            {
                ModelState.AddModelError(nameof(model.DeliveryMethod), Order.DeliveryMethodError);
                return this.View(model);
            }

            bool isFinalized = await this._orderService.FinalizeOrderWithDetailsAsync(userId, model);

            if (isFinalized)
            {
                TempData["SuccessMessage"] = OrderConfirmedSuccessfully;
                return this.RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                TempData["ErrorMessage"] = OrderConfirmationFailed;
                model.TotalProductsPrice = await this._orderService.GetTotalCartPriceAsync(userId);
                return this.View(model);
            }
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.ConfirmError, ex.Message));
            TempData["ErrorMessage"] = OrderConfirmationFailed;

            return this.RedirectToAction(nameof(Index));
        }
    }
}