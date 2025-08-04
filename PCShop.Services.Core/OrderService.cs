using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Manager;
using PCShop.Web.ViewModels.Order;
using System.Globalization;
using System.Text;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly INotificationService _notificationService;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            INotificationService notificationService)
        {
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._notificationService = notificationService;
        }

        public async Task<bool> AddProductToCartAsync(AddToCartFormModel model, string userId)
        {
            Guid? productId = null;
            Guid? computerId = null;

            bool isValidUser = Guid.TryParse(userId, out Guid parsedUserId);

            if (!string.IsNullOrEmpty(model.ProductId) && Guid.TryParse(model.ProductId, out Guid parsedProductId))
            {
                productId = parsedProductId;
            }

            if (!string.IsNullOrEmpty(model.ComputerId) && Guid.TryParse(model.ComputerId, out Guid parsedComputerId))
            {
                computerId = parsedComputerId;
            }

            if (productId == null && computerId == null)
            {
                throw new ArgumentException(InvalidIdMessage);
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException(QuantityMustBeGreaterMessage);
            }

            Order? order = await this._orderRepository.GetPendingOrderWithItemsAsync(userId);

            if (order == null && isValidUser)
            {
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = parsedUserId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    Comment = string.Empty,
                    DeliveryFee = 0m,
                    DeliveryMethod = 0,
                    DeliveryAddress = string.Empty,
                    OrdersItems = new List<OrderItem>()
                };

                await this._orderRepository.AddAsync(order);

                OrderItem newItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = productId,
                    ComputerId = computerId,
                    Quantity = model.Quantity
                };

                await this._orderItemRepository.AddAsync(newItem);

                return true;
            }
            else if (order != null)
            {
                OrderItem? existingItem = order.OrdersItems.FirstOrDefault(i =>
                    (productId.HasValue && i.ProductId == productId) ||
                    (computerId.HasValue && i.ComputerId == computerId));

                if (existingItem != null)
                {
                    existingItem.Quantity += model.Quantity;
                    bool updateResult = await this._orderItemRepository.UpdateAsync(existingItem);

                    return updateResult;
                }
                else
                {
                    OrderItem newItem = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = productId,
                        ComputerId = computerId,
                        Quantity = model.Quantity
                    };

                    await this._orderItemRepository.AddAsync(newItem);
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<OrderItemViewModel>> GetCartItemsAsync(string userId)
        {
            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null) return Enumerable.Empty<OrderItemViewModel>();

            return order.OrdersItems.Select(i =>
            {
                bool isProduct = i.ProductId != null;
                string? name = isProduct ? i.Product?.Name : i.Computer?.Name;
                string? imageUrl = isProduct ? i.Product?.ImageUrl : i.Computer?.ImageUrl;
                decimal price = isProduct ? i.Product?.Price ?? 0 : i.Computer?.Price ?? 0;

                return new OrderItemViewModel
                {
                    Id = i.Id.ToString(),
                    Name = name ?? "Unknown",
                    ImageUrl = imageUrl ?? $"/{ImagesFolder}/{NoImageUrl}",
                    Price = price,
                    Quantity = i.Quantity,
                    OrderId = order.Id.ToString(),
                    ProductId = i.ProductId?.ToString(),
                    ComputerId = i.ComputerId?.ToString()
                };
            });
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            Order? order = await this._orderRepository.GetPendingOrderAsync(userId);

            if (order == null)
            {
                return 0;
            }

            return order.OrdersItems.Sum(i => i.Quantity);
        }

        public async Task<bool> RemoveItemAsync(string itemId, string userId)
        {
            if (!Guid.TryParse(itemId, out Guid id)) return false;

            bool isRemoved = false;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null) return false;

            OrderItem? item = order.OrdersItems.FirstOrDefault(i => i.Id == id);

            if (item != null)
            {
                await this._orderItemRepository.HardDeleteAsync(item);

                isRemoved = true;
            }

            return isRemoved;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            bool isCleared = false;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order != null && order.OrdersItems.Any())
            {
                foreach (OrderItem item in order.OrdersItems.ToList())
                {
                    await this._orderItemRepository.HardDeleteAsync(item);

                    isCleared = true;
                }
            }

            return isCleared;
        }

        public async Task<OrderConfirmationInputModel?> GetOrderConfirmationDataAsync(string userId)
        {
            OrderConfirmationInputModel? confirmModel = null;

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null || !order.OrdersItems.Any())
            {
                return null;
            }

            decimal totalPrice = SumTotalPrice(order);

            if (user != null && order != null)
            {
                confirmModel = new OrderConfirmationInputModel
                {
                    FullName = user.FullName,
                    Address = user.Address,
                    City = user.City,
                    PostalCode = user.PostalCode,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    TotalProductsPrice = totalPrice,
                    DeliveryMethod = DeliveryMethod.None,
                    Comment = string.Empty
                };
            }

            return confirmModel;
        }

        public async Task<bool> FinalizeOrderWithDetailsAsync(string userId, OrderConfirmationInputModel model)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(DeliveryMethod), model.DeliveryMethod) ||
                !Enum.IsDefined(typeof(OrderPaymentMethod), model.PaymentMethod))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.City) ||
                string.IsNullOrWhiteSpace(model.PostalCode) ||
                string.IsNullOrWhiteSpace(model.Address))
            {
                return false;
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            bool isFinalized = false;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order != null && order.OrdersItems.Any() && model.DeliveryMethod != DeliveryMethod.None)
            {
                decimal totalPrice = SumTotalPrice(order);

                order.DeliveryMethod = model.DeliveryMethod;
                order.Comment = model.Comment;
                order.DeliveryAddress = $"{model.City}, {model.PostalCode}, {model.Address}, {model.PhoneNumber}";
                order.DeliveryFee = CalculateDeliveryFee(model.DeliveryMethod);
                order.TotalPrice = totalPrice + order.DeliveryFee;
                order.OrderDate = DateTime.UtcNow.ToLocalTime();
                order.PaymentMethod = model.PaymentMethod;
                order.Status = OrderStatus.Completed;

                await this._orderRepository.UpdateAsync(order);
                isFinalized = true;

                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    await this.SendOrderConfirmationEmailAsync(user.Email, order);
                }

                await this._notificationService.CreateAsync(order.ApplicationUserId.ToString(), "Your order has been confirmed. Check your email address.");
            }

            return isFinalized;
        }

        public async Task<decimal> GetTotalCartPriceAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return 0m;
            }

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null)
            {
                return 0m;
            }

            decimal total = SumTotalPrice(order);

            return total;
        }

        public async Task SendOrderConfirmationEmailAsync(string userEmail, Order order)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"<p>Hello {order.ApplicationUser?.FullName ?? "Customer"},</p>");
            sb.AppendLine($"<p>Thank you for your order <strong>#{order.Id}</strong>.</p>");
            sb.AppendLine($"<p>Delivery address: <strong>{order.DeliveryAddress}</strong></p>");
            sb.AppendLine($"<p>Total amount: <strong>{order.TotalPrice:F2} €</strong></p>");
            sb.AppendLine($"<p>Delivery method: <strong>{order.DeliveryMethod}</strong></p>");
            sb.AppendLine($"<p>Payment method: <strong>{order.PaymentMethod}</strong></p>");
            sb.AppendLine($"<p>Comment: <strong>{order.Comment ?? "None"}</string></p>");
            sb.AppendLine("<p>We will notify you once your order is shipped.</p>");
            sb.AppendLine("<br><p>Best regards,<br>PCShop Team</p>");


            string subject = $"Order Confirmation - #{order.Id}";
            string message = sb.ToString();

            await this._emailSender.SendEmailAsync(userEmail, subject, message);
        }

        public async Task<ManagerOrdersPageViewModel> GetOrdersByStatusPagedAsync(OrderStatus status, int currentPage, int pageSize = OrderManagerPageSize)
        {
            ICollection<Order> allOrders = (await this._orderRepository.GetAllOrdersWithItemsAsync())
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            int totalOrders = allOrders.Count;
            int totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            IEnumerable<ManagerOrderViewModel> pagedOrders = allOrders
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new ManagerOrderViewModel
                {
                    Id = o.Id.ToString(),
                    CustomerName = o.ApplicationUser.FullName,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderDate.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture),
                    DeliveryMethod = o.DeliveryMethod.ToString(),
                    Status = o.Status.ToString(),
                    SendDate = o.Status == OrderStatus.Sent ? o.SendDate?.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture) : null
                });

            return new ManagerOrdersPageViewModel
            {
                Orders = pagedOrders,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };
        }

        public async Task<ManagerOrdersPageViewModel> GetAllOrdersPagedAsync(int currentPage, int pageSize = OrderManagerPageSize)
        {
            IEnumerable<Order> allOrders = await this._orderRepository
                .GetAllOrdersWithItemsAsync();

            int totalOrders = allOrders.Count();
            int totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            IEnumerable<ManagerOrderViewModel> pagedOrders = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new ManagerOrderViewModel
                {
                    Id = o.Id.ToString(),
                    CustomerName = o.ApplicationUser.FullName,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderDate.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture),
                    DeliveryMethod = o.DeliveryMethod.ToString(),
                    Status = o.Status.ToString(),
                    SendDate = o.SendDate?.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture)
                });

            return new ManagerOrdersPageViewModel
            {
                Orders = pagedOrders,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                CurrentStatusFilter = null
            };
        }

        public async Task<bool> ApproveOrderAsync(string orderId)
        {
            Order? order = await this._orderRepository.GetByIdWithUserAsync(orderId);

            if (order == null || order.Status != OrderStatus.Completed)
                return false;

            order.Status = OrderStatus.Sent;
            order.SendDate = DateTime.UtcNow.ToLocalTime();

            await this._orderRepository.UpdateAsync(order);

            if (!string.IsNullOrWhiteSpace(order.ApplicationUser?.Email))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"<p>Hello <strong>{order.ApplicationUser.FullName}</strong>,</p>");
                sb.AppendLine($"<p>Your order <strong>#{order.Id}</strong> has been <span style='color:green;'>approved</span> and will be shipped soon.</p>");
                sb.AppendLine($"<p>Delivery address: <strong>{order.DeliveryAddress}</strong></p>");
                sb.AppendLine($"<p>Total amount: <strong>{order.TotalPrice:F2} €</strong></p>");
                sb.AppendLine($"<p>Delivery method: <strong>{order.DeliveryMethod}</strong></p>");
                sb.AppendLine($"<p>Payment method: <strong>{order.PaymentMethod}</strong></p>");
                sb.AppendLine($"<p>Comment: <strong>{order.Comment ?? "None"}</string></p>");
                sb.AppendLine("<p>Thank you for shopping with us!</p>");
                sb.AppendLine("<hr/>");
                sb.AppendLine("<p>Best regards,<br/>PCShop Team</p>");

                string subject = "Your Order Has Been Approved";
                string messageBody = sb.ToString();

                await this._notificationService.CreateAsync(order.ApplicationUserId.ToString(), "Your order has been shipped. Check your email address.");
                await this._emailSender.SendEmailAsync(order.ApplicationUser.Email, subject, messageBody);
            }

            return true;
        }

        public async Task<bool> DeleteOrderAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out Guid parsedOrderId))
            {
                return false;
            }

            Order? order = await this._orderRepository
                .GetByIdAsync(parsedOrderId);

            if (order == null)
            {
                return false;
            }

            if (order.Status != OrderStatus.Pending || order.DeliveryMethod != DeliveryMethod.None)
            {
                return false;
            }

            await this._orderRepository.HardDeleteAsync(order);

            return true;
        }

        public decimal CalculateDeliveryFee(DeliveryMethod method)
        {
            return method switch
            {
                DeliveryMethod.Econt => EcontFee,
                DeliveryMethod.Speedy => SpeedyFee,
                DeliveryMethod.ToAddress => ToAddressFee,
                _ => throw new ArgumentOutOfRangeException(nameof(method), UnknownDeliveryMethodMessage)
            };
        }

        private static decimal SumTotalPrice(Order order)
        {
            return order.OrdersItems.Sum(item =>
            {
                decimal unitPrice = item.Product?.Price ?? item.Computer?.Price ?? 0;
                return unitPrice * item.Quantity;
            });
        }
    }
}
