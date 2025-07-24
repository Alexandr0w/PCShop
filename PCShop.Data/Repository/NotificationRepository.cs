using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;

namespace PCShop.Data.Repository
{
    public class NotificationRepository : BaseRepository<Notification, Guid>, INotificationRepository
    {
        public NotificationRepository(PCShopDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}