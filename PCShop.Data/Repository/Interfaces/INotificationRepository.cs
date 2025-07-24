using PCShop.Data.Models;

namespace PCShop.Data.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification, Guid>, IAsyncRepository<Notification, Guid>
    {
    }
}
