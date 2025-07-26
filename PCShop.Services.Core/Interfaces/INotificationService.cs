using PCShop.Web.ViewModels.Notification;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationListViewModel> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = NotificationsPageSize);

        Task<int> GetUnreadCountAsync(string userId);

        Task CreateAsync(string userId, string message);

        Task<bool> MarkAsReadAsync(string notificationId);

        Task<bool> MarkAllAsReadAsync(string userId);

        Task<bool> DeleteNotificationAsync(string notificationId);

        Task<bool> HasUnreadNotificationsAsync(string userId);
    }
}
