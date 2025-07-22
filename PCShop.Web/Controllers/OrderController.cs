using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Text;
using System.Text.Json;
using PCShop.Data.Models.Enum;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Order;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.MessageConstants.OrderMessages;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Web.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly StripeSettings _stripeSettings;

        public OrderController(
            IOrderService orderService,
            ILogger<OrderController> logger,
            IOptions<StripeSettings> stripeSettings)
        {
            this._orderService = orderService;
            this._logger = logger;
            this._stripeSettings = stripeSettings.Value;
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
                TempData["ErrorMessage"] = ConfirmationFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
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

                decimal deliveryFee = model.DeliveryMethod switch
                {
                    DeliveryMethod.Econt => EcontFee,
                    DeliveryMethod.Speedy => SpeedyFee,
                    DeliveryMethod.ToAddress => ToAddressFee,
                    _ => 0m
                };

                model.TotalProductsPrice = await this._orderService.GetTotalCartPriceAsync(userId);
                decimal totalAmount = model.TotalProductsPrice + deliveryFee;

                if (model.PaymentMethod == OrderPaymentMethod.Card) 
                {
                    StripePaymentViewModel stripeModel = new StripePaymentViewModel
                    {
                        Amount = totalAmount,
                        FullName = model.FullName,
                        Email = model.Email,
                        StripePublicKey = this._stripeSettings.PublicKey,
                        TotalProductsPrice = model.TotalProductsPrice,
                        DeliveryMethod = model.DeliveryMethod,
                        PhoneNumber = model.PhoneNumber ?? string.Empty,
                        City = model.City ?? string.Empty,
                        Address = model.Address ?? string.Empty,
                        PostalCode = model.PostalCode ?? string.Empty,
                        Comment = model.Comment ?? string.Empty
                    };

                    HttpContext.Session.SetString("PendingOrder", JsonSerializer.Serialize(model));

                    return this.View("StripeCheckout", stripeModel);
                }
                else if (model.PaymentMethod == OrderPaymentMethod.CashOnDelivery) 
                {
                    bool isFinalized = await this._orderService.FinalizeOrderWithDetailsAsync(userId, model);

                    if (isFinalized)
                    {
                        TempData["SuccessMessage"] = ConfirmedSuccessfully;
                        return this.RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = ConfirmationFailed;
                        model.TotalProductsPrice = await this._orderService.GetTotalCartPriceAsync(userId);

                        return this.View(model);
                    }
                }

                ModelState.AddModelError(string.Empty, Order.PaymentMethodError);
                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Order.ConfirmError, ex.Message));
                TempData["ErrorMessage"] = ConfirmationFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] StripePaymentViewModel model)
        {
            try
            {
                string secretKey = this._stripeSettings.SecretKey;

                if (string.IsNullOrEmpty(secretKey))
                {
                    this._logger.LogError(Order.StripeSettingsError);
                }

                StripeConfiguration.ApiKey = secretKey;

                SessionCreateOptions options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = model.Amount * 100,
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "PCShop Order",
                            },
                        },
                        Quantity = 1,
                    }
                },
                    Mode = "payment",
                    SuccessUrl = Url.Action("Success", "Order", null, Request.Scheme)!,
                    CancelUrl = Url.Action("Cancel", "Order", null, Request.Scheme)!,
                    CustomerEmail = model.Email,

                    Metadata = new Dictionary<string, string>
                {
                    {"orderId", Guid.NewGuid().ToString()},
                    {"userId", this.GetUserId() ?? ""}
                }
                };

                SessionService service = new SessionService();
                Session session = await service.CreateAsync(options);

                return this.Json(new { id = session.Id });
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Order.FailedPaymentError, ex.Message));
                TempData["ErrorMessage"] = PaymentError;

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Success()
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                if (HttpContext.Session.TryGetValue("PendingOrder", out byte[]? bytes))
                {
                    string json = Encoding.UTF8.GetString(bytes);
                    OrderConfirmationInputModel? orderModel = JsonSerializer.Deserialize<OrderConfirmationInputModel>(json);

                    if (orderModel != null)
                    {
                        bool isFinalized = await this._orderService.FinalizeOrderWithDetailsAsync(userId, orderModel);
                        HttpContext.Session.Remove("PendingOrder");

                        if (isFinalized)
                        {
                            TempData["SuccessMessage"] = PaymentSuccessfully;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = PaymentFailed;
                        }
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = OrderDataNotFound;
                }

                return this.RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Order.SuccessPaymentError, ex.Message));
                TempData["ErrorMessage"] = PaymentFailed;

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public IActionResult Cancel()
        {
            try
            {
                TempData["ErrorMessage"] = PaymentCanceled;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Order.PaymentCanceledError, ex.Message));
                TempData["ErrorMessage"] = PaymentCancelFailed;

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}