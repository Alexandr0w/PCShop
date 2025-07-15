using PCShop.Data.Models;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IOrderItemRepository : IAsyncRepository<OrderItem, Guid>
    {
    }
}
