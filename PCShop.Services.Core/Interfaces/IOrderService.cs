using PCShop.Web.ViewModels.Order;

namespace PCShop.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task AddProductToCartAsync(AddToCartFormModel model, string userId);

        Task<IEnumerable<OrderItemViewModel>> GetCartItemsAsync(string userId);

        Task<int> GetCartCountAsync(string userId);

        Task RemoveItemAsync(string itemId, string userId);

        Task ClearCartAsync(string userId);

        Task FinalizeOrderAsync(string userId);
    }
}

