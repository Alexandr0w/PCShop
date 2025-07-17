using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Data.Repository.Interfaces;

namespace PCShop.Data.Repository
{
    public class OrderRepository : BaseRepository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(PCShopDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<Order?> GetPendingOrderWithItemsAsync(string userId)
        {
            return await this._DbSet
                .Include(o => o.OrdersItems)
                    .ThenInclude(i => i.Product)
                .Include(o => o.OrdersItems)
                    .ThenInclude(i => i.Computer)
                .FirstOrDefaultAsync(o => o.ApplicationUserId.ToString().ToLower() == userId.ToLower() && o.Status == OrderStatus.Pending);
        }

        public async Task<Order?> GetPendingOrderAsync(string userId)
        {
            return await this._DbSet
                .Include(o => o.OrdersItems)
                .FirstOrDefaultAsync(o => o.ApplicationUserId.ToString().ToLower() == userId.ToLower() && o.Status == OrderStatus.Pending);
        }
    }
}
