using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Notification;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            this._notificationRepository = notificationRepository;
        }

        public async Task<NotificationListViewModel> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = NotificationsPageSize)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return new NotificationListViewModel();
            }

            IQueryable<Notification> query = this._notificationRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(n => n.ApplicationUserId == userGuid)
                .OrderByDescending(n => n.CreatedOn);

            int totalCount = await query.CountAsync();

            ICollection<NotificationViewModel> notifications = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationViewModel
                {
                    Id = n.Id.ToString(),
                    Message = n.Message,
                    CreatedOn = n.CreatedOn.ToLocalTime().ToString(DateAndTimeDisplayFormat),
                    IsRead = n.IsRead
                })
                .ToListAsync();

            return new NotificationListViewModel
            {
                Notifications = notifications,
                CurrentPage = page,
                NotificationsPerPage = pageSize,
                TotalNotifications = totalCount
            };
        }

        public async Task CreateAsync(string userId, string message)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return;
            }

            Notification notification = new Notification
            {
                ApplicationUserId = userGuid,
                Message = message
            };

            await this._notificationRepository.AddAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(string notificationId)
        {
            if (!Guid.TryParse(notificationId, out Guid notificationGuid))
            {
                return false;
            }

            Notification? notification = await this._notificationRepository
                .GetByIdAsync(notificationGuid);

            if (notification == null)
            {
                return false;
            }

            this._notificationRepository.HardDelete(notification);
            await this._notificationRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return 0;
            }

            return await this._notificationRepository
                .GetAllAttached()
                .CountAsync(n => n.ApplicationUserId == userGuid && !n.IsRead);
        }

        public async Task<bool> HasUnreadNotificationsAsync(string userId)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return false;
            }

            return await this._notificationRepository
                .GetAllAttached()
                .AnyAsync(n => n.ApplicationUserId == userGuid && !n.IsRead);
        }

        public async Task<int> MarkMultipleAsReadAsync(string[] notificationIds)
        {
            List<Guid> guidIds = notificationIds
                .Select(id => Guid.TryParse(id, out Guid guid) ? guid : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToList();

            List<Notification> notifications = await this._notificationRepository
                .GetAllAttached()
                .Where(n => guidIds.Contains(n.Id) && !n.IsRead)
                .ToListAsync();

            foreach (Notification notification in notifications)
            {
                notification.IsRead = true;
            }

            await this._notificationRepository.SaveChangesAsync();
            return notifications.Count;
        }

        public async Task<int> DeleteMultipleAsync(string[] notificationIds)
        {
            List<Guid> guidIds = notificationIds
                .Select(id => Guid.TryParse(id, out Guid guid) ? guid : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToList();

            List<Notification> notifications = await this._notificationRepository
                .GetAllAttached()
                .Where(n => guidIds.Contains(n.Id))
                .ToListAsync();

            foreach (Notification notification in notifications)
            {
                this._notificationRepository.HardDelete(notification);
            }

            await this._notificationRepository.SaveChangesAsync();
            return notifications.Count;
        }
    }
}
