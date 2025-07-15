using PCShop.Data.Models;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order, Guid>, IAsyncRepository<Order, Guid>
    {
        Task<Order?> GetPendingOrderWithItemsAsync(string userId);
        Task<Order?> GetPendingOrderAsync(string userId);
    }
}
