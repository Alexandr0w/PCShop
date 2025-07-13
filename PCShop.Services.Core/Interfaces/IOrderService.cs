using PCShop.Web.ViewModels.Order;

namespace PCShop.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task AddProductToCartAsync(string userId, Guid productId, int quantity = 1);

        Task<IEnumerable<OrderItemViewModel>> GetCartItemsAsync(string userId);
    }
}

