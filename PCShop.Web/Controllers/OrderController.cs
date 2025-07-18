using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            TempData["SuccessMessage"] = FinalizedSuccessfully;

            return this.RedirectToAction(nameof(Index), "Home");
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.FinalizeError, ex.Message));
            TempData["ErrorMessage"] = FinalizeFailed;

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

            if (isProfileComplete == false)
            {
                TempData["ErrorMessage"] = CompleteProfile;
                return this.RedirectToAction("EditProfile", "Account");
            }

            OrderConfirmationViewModel? model = await this._orderService.GetOrderConfirmationDataAsync(userId);

            if (model == null)
            {
                TempData["ErrorMessage"] = NoPendingOrders;
                return this.RedirectToAction(nameof(Index));
            }

            this.ModelState.Clear();

            return this.View(model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(string.Format(Order.LoadConfirmationError, ex.Message));
            TempData["ErrorMessage"] = UnableToConfirm;

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
                TempData["SuccessMessage"] = OrderConfirmedSuccessfully;
                return this.RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                TempData["ErrorMessage"] = OrderConfirmationFailed;

                decimal totalPrice = await this._orderService.GetTotalCartPriceAsync(userId);
                model.TotalProductsPrice = totalPrice;

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