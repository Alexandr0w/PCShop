using Microsoft.AspNetCore.Identity;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Order;
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

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
            this._userManager = userManager;
        }

        public async Task<bool> AddProductToCartAsync(AddToCartFormModel model, string userId)
        {
            Guid? productId = null;
            Guid? computerId = null;

            bool isAdded = false;

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

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null)
            {
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = Guid.Parse(userId),
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    Comment = string.Empty,
                    DeliveryFee = 0m,
                    DeliveryMethod = 0,
                    DeliveryAddress = string.Empty,
                    OrdersItems = new List<OrderItem>()
                };

                await this._orderRepository.AddAsync(order);
            }

            OrderItem? existingItem = order.OrdersItems.FirstOrDefault(i =>
                (productId.HasValue && i.ProductId == productId) ||
                (computerId.HasValue && i.ComputerId == computerId));

            if (existingItem != null)
            {
                existingItem.Quantity += model.Quantity;
                await this._orderItemRepository.UpdateAsync(existingItem);
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

                isAdded = true;
            }

            return isAdded;
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
            return order?.OrdersItems.Sum(i => i.Quantity) ?? 0;
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

        public async Task<bool> FinalizeOrderAsync(string userId)
        {
            bool isFinalized = false;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                order.OrderDate = DateTime.UtcNow;

                decimal totalPrice = 0m;

                foreach (OrderItem item in order.OrdersItems)
                {
                    decimal unitPrice = item.Product?.Price ?? item.Computer?.Price ?? 0;
                    totalPrice += unitPrice * item.Quantity;
                }

                order.TotalPrice = totalPrice;
                isFinalized = true;

                await this._orderRepository.UpdateAsync(order);
            }

            return isFinalized;
        }

        public async Task<OrderConfirmationViewModel?> GetOrderConfirmationDataAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(UserIdNullOrEmptyMessage);

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            if (user == null)
                return null;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null || !order.OrdersItems.Any())
                return null;

            decimal totalPrice = order.OrdersItems.Sum(item =>
            {
                decimal unitPrice = item.Product?.Price ?? item.Computer?.Price ?? 0;
                return unitPrice * item.Quantity;
            });

            return new OrderConfirmationViewModel
            {
                FullName = user.FullName ?? string.Empty,
                Address = user.Address ?? string.Empty,
                City = user.City ?? string.Empty,
                PostalCode = user.PostalCode ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                TotalProductsPrice = totalPrice,
                DeliveryMethod = DeliveryMethod.None,
                Comment = string.Empty
            };
        }

        public async Task<bool> FinalizeOrderWithDetailsAsync(string userId, OrderConfirmationViewModel model)
        {
            bool isFinalized = false;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order != null && order.OrdersItems.Any())
            {
                decimal totalPrice = order.OrdersItems.Sum(item =>
                {
                    decimal unitPrice = item.Product?.Price ?? item.Computer?.Price ?? 0;
                    return unitPrice * item.Quantity;
                });

                order.DeliveryMethod = model.DeliveryMethod;
                order.Comment = model.Comment;
                order.DeliveryAddress = $"{model.City}, {model.PostalCode}, {model.Address}";
                order.DeliveryFee = CalculateDeliveryFee(model.DeliveryMethod);
                order.TotalPrice = totalPrice + order.DeliveryFee;
                order.OrderDate = DateTime.UtcNow;
                order.Status = OrderStatus.Completed;

                await this._orderRepository.UpdateAsync(order);
                isFinalized = true;
            }

            return isFinalized;
        }

        public async Task<bool> IsUserProfileCompleteAsync(string userId)
        {
            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            if (user == null) return false;

            return !string.IsNullOrWhiteSpace(user.FullName) &&
                   !string.IsNullOrWhiteSpace(user.Address) &&
                   !string.IsNullOrWhiteSpace(user.City) &&
                   !string.IsNullOrWhiteSpace(user.PostalCode) &&
                   !string.IsNullOrWhiteSpace(user.PhoneNumber);
        }

        public async Task<decimal> GetTotalCartPriceAsync(string userId)
        {
            Order? order = await _orderRepository.GetPendingOrderWithItemsAsync(userId);

            if (order == null)
                return 0m;

            decimal total = 0m;

            foreach (var item in order.OrdersItems)
            {
                decimal unitPrice = item.Product?.Price ?? item.Computer?.Price ?? 0;
                total += unitPrice * item.Quantity;
            }

            return total;
        }

        private decimal CalculateDeliveryFee(DeliveryMethod method)
        {
            return method switch
            {
                DeliveryMethod.Econt => EcontFee,
                DeliveryMethod.Speedy => SpeedyFee,
                DeliveryMethod.ToAddress => ToAddressFee,
                _ => throw new ArgumentOutOfRangeException(nameof(method), UnknownDeliveryMethodMessage)
            };
        }
    }
}
