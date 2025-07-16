using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using static PCShop.Services.Common.ServiceConstants;
using PCShop.Web.ViewModels.Order;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.GCommon.ExceptionMessages;

namespace PCShop.Services.Core
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
        }

        public async Task AddProductToCartAsync(AddToCartFormModel model, string userId)
        {
            Guid? productId = null;
            Guid? computerId = null;

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
            }
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

        public async Task RemoveItemAsync(string itemId, string userId)
        {
            if (!Guid.TryParse(itemId, out Guid id)) return;

            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order == null) return;

            OrderItem? item = order.OrdersItems.FirstOrDefault(i => i.Id == id);

            if (item != null)
            {
                await this._orderItemRepository.HardDeleteAsync(item);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            Order? order = await this._orderRepository
                .GetPendingOrderWithItemsAsync(userId);

            if (order != null && order.OrdersItems.Any())
            {
                foreach (OrderItem item in order.OrdersItems.ToList())
                {
                    await this._orderItemRepository.HardDeleteAsync(item);
                }
            }
        }

        public async Task FinalizeOrderAsync(string userId)
        {
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

                await this._orderRepository.UpdateAsync(order);
            }
        }

    }
}
