using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Order;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ExceptionMessages;

public class OrderService : IOrderService
{
    private readonly PCShopDbContext _dbContext;

    public OrderService(PCShopDbContext dbContext)
    {
        this._dbContext = dbContext;
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
            throw new ArgumentException(QuantityMustBeGreater);
        }

        Guid userGuid = Guid.Parse(userId);

        Order? order = await this._dbContext
            .Orders
            .Include(o => o.OrdersItems)
            .FirstOrDefaultAsync(o => o.ApplicationUserId == userGuid && o.Status == OrderStatus.Pending);

        if (order == null)
        {
            order = new Order
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = userGuid,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrdersItems = new List<OrderItem>()
            };

            await this._dbContext.Orders.AddAsync(order);
            await this._dbContext.SaveChangesAsync();
        }

        OrderItem? existingItem = order.OrdersItems
            .FirstOrDefault(i =>
                (productId.HasValue && i.ProductId == productId) ||
                (computerId.HasValue && i.ComputerId == computerId));

        if (existingItem != null)
        {
            existingItem.Quantity += model.Quantity;
            this._dbContext.OrdersItems.Update(existingItem);
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

            await this._dbContext.OrdersItems.AddAsync(newItem);
        }

        await this._dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderItemViewModel>> GetCartItemsAsync(string userId)
    {
        Order? order = await this._dbContext
            .Orders
            .Include(o => o.OrdersItems)
                .ThenInclude(i => i.Product)
            .Include(o => o.OrdersItems)
                .ThenInclude(i => i.Computer)
            .FirstOrDefaultAsync(o => o.ApplicationUserId == Guid.Parse(userId) && o.Status == OrderStatus.Pending);

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

    public async Task RemoveItemAsync(string itemId, string userId)
    {
        if (!Guid.TryParse(itemId, out Guid id)) return;

        Order? order = await this._dbContext
            .Orders
            .Include(o => o.OrdersItems)
            .FirstOrDefaultAsync(o => o.ApplicationUserId == Guid.Parse(userId) && o.Status == OrderStatus.Pending);

        if (order == null) return;

        OrderItem? item = order.OrdersItems.FirstOrDefault(i => i.Id == id);

        if (item != null)
        {
            this._dbContext.OrdersItems.Remove(item);
            await this._dbContext.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        Order? order = await this._dbContext
            .Orders
            .Include(o => o.OrdersItems)
            .FirstOrDefaultAsync(o => o.ApplicationUserId == Guid.Parse(userId) && o.Status == OrderStatus.Pending);

        if (order != null)
        {
            this._dbContext.OrdersItems.RemoveRange(order.OrdersItems);
            await this._dbContext.SaveChangesAsync();
        }
    }

    public async Task FinalizeOrderAsync(string userId)
    {
        Order? order = await this._dbContext
            .Orders
            .FirstOrDefaultAsync(o => o.ApplicationUserId == Guid.Parse(userId) && o.Status == OrderStatus.Pending);

        if (order != null)
        {
            order.Status = OrderStatus.Completed;
            order.OrderDate = DateTime.UtcNow;
            await this._dbContext.SaveChangesAsync();
        }
    }
}