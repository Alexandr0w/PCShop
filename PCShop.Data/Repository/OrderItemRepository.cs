using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;

namespace PCShop.Data.Repository
{
    public class OrderItemRepository : BaseRepository<OrderItem, Guid>, IOrderItemRepository
    {
        public OrderItemRepository(PCShopDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
