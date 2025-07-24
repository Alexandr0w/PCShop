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
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return null;
            }

            return await this._DbSet
                .Include(o => o.OrdersItems)
                    .ThenInclude(i => i.Product)
                .Include(o => o.OrdersItems)
                    .ThenInclude(i => i.Computer)
                .FirstOrDefaultAsync(o => o.ApplicationUserId == userGuid && o.Status == OrderStatus.Pending);
        }

        public async Task<Order?> GetPendingOrderAsync(string userId)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return null; 
            }

            return await this._DbSet
                .Include(o => o.OrdersItems)
                .FirstOrDefaultAsync(o => o.ApplicationUserId == userGuid && o.Status == OrderStatus.Pending);
        }

        public async Task<ICollection<Order>> GetAllOrdersWithItemsAsync()
        {
            return await this._DbSet
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrdersItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrdersItems)
                    .ThenInclude(oi => oi.Computer)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdWithUserAsync(string id)
        {
            return await this._DbSet
                .Include(o => o.ApplicationUser)
                .FirstOrDefaultAsync(o => o.Id.ToString().ToLower() == id.ToLower());
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return 0;
            }

            Order? order = await this._DbSet
                .Include(o => o.OrdersItems)
                .FirstOrDefaultAsync(o => o.ApplicationUserId == userGuid && o.Status == OrderStatus.Pending);

            if (order == null)
            {
                return 0;
            }

            return order.OrdersItems.Sum(i => i.Quantity);
        }
    }
}
