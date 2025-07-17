using PCShop.Web.ViewModels.Order;

namespace PCShop.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddProductToCartAsync(AddToCartFormModel model, string userId);
        Task<IEnumerable<OrderItemViewModel>> GetCartItemsAsync(string userId);
        Task<int> GetCartCountAsync(string userId);

        Task<bool> RemoveItemAsync(string itemId, string userId);
        Task<bool> ClearCartAsync(string userId);
        Task<bool> FinalizeOrderAsync(string userId);

        Task<OrderConfirmationViewModel?> GetOrderConfirmationDataAsync(string userId);
        Task<bool> FinalizeOrderWithDetailsAsync(string userId, OrderConfirmationViewModel model);
        Task<bool> IsUserProfileCompleteAsync(string userId);
        Task<decimal> GetTotalCartPriceAsync(string userId);

    }
}

