using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Web.ViewModels.Manager;
using PCShop.Web.ViewModels.Order;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddProductToCartAsync(AddToCartFormModel model, string userId);

        Task<IEnumerable<OrderItemViewModel>> GetCartItemsAsync(string userId);
        Task<int> GetCartCountAsync(string userId);

        Task<bool> RemoveItemAsync(string itemId, string userId);
        Task<bool> ClearCartAsync(string userId);

        Task<OrderConfirmationInputModel?> GetOrderConfirmationDataAsync(string userId);
        Task<decimal> GetTotalCartPriceAsync(string userId);

        Task<bool> FinalizeOrderWithDetailsAsync(string userId, OrderConfirmationInputModel model);

        Task SendOrderConfirmationEmailAsync(string userEmail, Order order);

        Task<ManagerOrdersPageViewModel> GetOrdersPagedAsync(OrderStatus? status, int currentPage, int pageSize = OrderManagerPageSize);
        Task<bool> ApproveOrderAsync(string orderId);
        Task<bool> DeleteOrderAsync(string orderId);
        Task<int> ArchiveOrdersAsync(IEnumerable<string> orderIds);
    }
}

